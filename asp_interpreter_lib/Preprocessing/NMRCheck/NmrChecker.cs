//-----------------------------------------------------------------------
// <copyright file="NmrChecker.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Preprocessing.NMRCheck
{
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Preprocessing.DualRules;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Types.TypeVisitors.Copy;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// A class to provide functionality to generate the NMR check for a asp programs.
    /// </summary>
    public class NmrChecker
    {
        private readonly PrefixOptions options;

        private readonly ILogger logger;

        private readonly GoalToLiteralConverter goalToLiteralConverter;

        private readonly DualRuleConverter dualConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="NmrChecker"/> class.
        /// </summary>
        /// <param name="options">The prefixes to be used.</param>
        /// <param name="logger">The logger to display messages.</param>
        /// <exception cref="ArgumentNullException">Thrown if the given options or logger is null.</exception>
        public NmrChecker(PrefixOptions options, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(logger);

            this.options = options;
            this.logger = logger;
            this.goalToLiteralConverter = new GoalToLiteralConverter();
            this.dualConverter = new DualRuleConverter(options, logger.GetDummy(), true);
        }

        /// <summary>
        /// Filters all literal from the given program and returns a list of constraints
        /// for every literal that occurs with negation and without negation.
        /// </summary>
        /// <param name="program">The program to search.</param>
        /// <returns>The rules containing the literals found twice.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the given program is null.</exception>
        public List<Statement> GetConstraintRules(AspProgram program)
        {
            ArgumentNullException.ThrowIfNull(program);

            List<Statement> statements = new List<Statement>();

            HashSet<(bool, string, int)> literals = new HashSet<(bool, string, int)>();

            var literalConverter = new GoalToLiteralConverter();

            // Find literals in query
            if (program.Query.HasValue)
            {
                foreach (var goal in program.Query.GetValueOrThrow().Goals)
                {
                    goal.Accept(literalConverter).IfHasValue(
                        v => literals.Add((v.HasStrongNegation, v.Identifier, v.Terms.Count)));
                }
            }

            // Find literals in rules
            foreach (var rule in program.Statements)
            {
                if (rule.HasHead)
                {
                    rule.Head.GetValueOrThrow().Accept(literalConverter).IfHasValue(
                        v => literals.Add((v.HasStrongNegation, v.Identifier, v.Terms.Count)));
                }

                foreach (var goal in rule.Body)
                {
                    goal.Accept(literalConverter).IfHasValue(
                           v => literals.Add((v.HasStrongNegation, v.Identifier, v.Terms.Count)));
                }
            }

            // Find constraints
            foreach (var literal in literals)
            {
                // Check for same name and arity but different negation
                if (!literals.Contains((!literal.Item1, literal.Item2, literal.Item3)))
                {
                    continue;
                }

                Statement constraint = new Statement();
                Literal positive = new Literal(
                    literal.Item2,
                    false,
                    false,
                    AspExtensions.GenerateVariables(literal.Item3, this.options.VariablePrefix));

                Literal negative = new Literal(
                    literal.Item2,
                    false,
                    true,
                    AspExtensions.GenerateVariables(literal.Item3, this.options.VariablePrefix));

                constraint.AddBody([negative, positive]);
                statements.Add(constraint);

                // Remove or its found twice
                literals.Remove(literal);
            }

            return statements;
        }

        /// <summary>
        /// Generates the NMR check for the given statements, assuming they are either olon rules or constraints.
        /// </summary>
        /// <param name="olonRules">The rules to retrieve the check for.</param>
        /// <returns>The resulting nmr check rule and its sub check rules.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the given rules are null.</exception>
        public List<Statement> GetNmrCheck(List<Statement> olonRules)
        {
            ArgumentNullException.ThrowIfNull(olonRules);

            this.logger.LogInfo("Generating NMR check...");
            if (olonRules.Count == 0)
            {
                this.logger.LogDebug("Finished generation because no OLON rules found in program.");
                var emptyCheck = new Statement();
                emptyCheck.AddHead(new Literal("_nmr_check", false, false, new List<ITerm>()));
                return [emptyCheck];
            }

            // 1) append negation of OLON Rule to its body (If not already present)
            List<Statement> preprocessedRules = this.PreprocessRules(olonRules);

            // 2) generate dual for modified rules
            var tempOlonRules =
                preprocessedRules.Select(r => r.Accept(new StatementCopyVisitor()).GetValueOrThrow()).ToList();

            List<Statement> duals = new List<Statement>();

            // 3) assign unique head (e.g. chk0)
            duals = this.GetDualsForCheck(olonRules.ToList());
            this.AddMissingPrefixes(duals, "_");

            Statement nmrCheck = this.GetCheckRule(tempOlonRules);
            this.AddForallToCheck(nmrCheck);

            duals.Insert(0, nmrCheck);

            return duals;
        }

        /// <summary>
        /// A simplified version of the dual rule generation that treats every rule separately.
        /// </summary>
        /// <param name="statements">The rules to get the duals for.</param>
        /// <returns>The dual rules for the input rules.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the given rules are null.</exception>
        private List<Statement> GetDualsForCheck(List<Statement> statements)
        {
            ArgumentNullException.ThrowIfNull(statements);

            List<Statement> duals = new List<Statement>();

            var withoutAnonymous = statements.Select(this.dualConverter.Replacer.Replace);
            var headComputed = withoutAnonymous.Select(this.dualConverter.ComputeHead).ToList();

            foreach (var statement in statements)
            {
                var head = statement.Head.GetValueOrThrow("Constraint rules must be given a head!");
                var kv = new KeyValuePair<(string, int, bool), List<Statement>>((head.Identifier, head.Terms.Count, head.HasStrongNegation), [statement]);
                duals.AddRange(this.dualConverter.ToConjunction(kv));
            }

            duals.ForEach(d => this.logger.LogDebug(d.ToString()));

            return duals;
        }

        /// <summary>
        /// Iterates through the rules and appends the negation of the head
        /// to the body of the rule if its not present.
        /// </summary>
        /// <param name="olonRules">The rules to preprocess.</param>
        /// <returns>The preprocessed rules.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the given rules are null.</exception>
        private List<Statement> PreprocessRules(List<Statement> olonRules)
        {
            ArgumentNullException.ThrowIfNull(olonRules);

            if (olonRules.Count == 0)
            {
                return olonRules;
            }

            // append negation of OLON Rule to its body(If not already present)
            for (int i = 0; i < olonRules.Count; i++)
            {
                Statement rule = olonRules[i];
                if (!rule.HasHead)
                {
                    string name = "_" + this.options.CheckPrefix + (i + 1) + "_";
                    rule.AddHead(new Literal(name, false, false, new List<ITerm>()));
                    continue;
                }

                var head = rule.Head.GetValueOrThrow("Could not parse head!");

                var negatedHead = head.Accept(new LiteralCopyVisitor(
                    new TermCopyVisitor())).GetValueOrThrow("Could not parse negated head!");
                negatedHead.HasNafNegation = !negatedHead.HasNafNegation;

                bool containsHead = rule.Body.Find(b => b.ToString() == negatedHead.ToString()) != null;

                if (!containsHead)
                {
                    rule.Body.Add(negatedHead);
                }

                head.Identifier = "_" + this.options.CheckPrefix + (i + 1) + "_";
            }

            return olonRules;
        }

        /// <summary>
        /// Adds forall to the nmr check rule if applicable.
        /// </summary>
        /// <param name="statement">The check to add forall to.</param>
        private void AddForallToCheck(Statement statement)
        {
            ArgumentNullException.ThrowIfNull(statement);

            VariableFinder variableFinder = new();

            for (var i = 0; i < statement.Body.Count; i++)
            {
                var literal = statement.Body[i].Accept(this.goalToLiteralConverter);

                if (!literal.HasValue)
                {
                    continue;
                }

                var innerGoal = literal.GetValueOrThrow();

                var variablesInGoal = innerGoal.Accept(variableFinder).GetValueOrThrow().Select(v => v.Identifier).ToList();

                if (variablesInGoal.Count == 0)
                {
                    continue;
                }

                var forall = DualRuleConverter.NestForall(variablesInGoal, innerGoal);
                statement.Body[i] = forall;
            }
        }

        /// <summary>
        /// Adds the given prefix to all literals in the given list of duals.
        /// </summary>
        /// <param name="duals">The duals to add the prefix to.</param>
        /// <param name="prefix">The prefix to add.</param>
        /// <exception cref="InvalidOperationException">Is thrown if any head has not as an identifier
        /// but does not contain exactly one inner literal.</exception>
        /// <exception cref="ArgumentNullException">Is thrown if the given duals or prefix is null.</exception>"
        private void AddMissingPrefixes(List<Statement> duals, string prefix)
        {
            ArgumentNullException.ThrowIfNull(prefix);
            ArgumentNullException.ThrowIfNull(duals);

            foreach (var dual in duals)
            {
                // At this point in the program no rule should be headless
                var head = dual.Head.GetValueOrThrow();

                if (head.Identifier == "not")
                {
                    if (head.Terms.Count != 1)
                    {
                        throw new InvalidOperationException("Expected exactly one term in the not literal");
                    }

                    // normal literal
                    head.Terms[0].Accept(new TermToBasicTermConverter()).IfHasValue(
                        v =>
                        {
                            if (!v.Identifier.StartsWith(prefix))
                            {
                                v.Identifier = prefix + v.Identifier;
                            }
                        });
                }
                else
                {
                    if (!head.Identifier.StartsWith(prefix))
                    {
                        head.Identifier = prefix + head.Identifier;
                    }
                }
            }
        }

        /// <summary>
        /// Generates a nmr check rule and adds the necessary rules from the input to its body.
        /// </summary>
        /// <param name="olonRules">The rules to add to the body.</param>
        /// <returns>A single nmr check rule.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the given rules are null.</exception>
        private Statement GetCheckRule(IEnumerable<Statement> olonRules)
        {
            ArgumentNullException.ThrowIfNull(olonRules);

            Statement nmrCheck = new();
            nmrCheck.AddHead(new Literal("_nmr_check", false, false, new List<ITerm>()));

            // add modified duals to the NMR check goal if it is not already in there
            var nmrBody = new List<Goal>();
            foreach (var rule in olonRules)
            {
                var head = rule.Head.GetValueOrThrow();
                nmrBody.Add(DualRuleConverter.WrapInNot(head));
            }

            nmrCheck.AddBody(nmrBody);
            return nmrCheck;
        }
    }
}