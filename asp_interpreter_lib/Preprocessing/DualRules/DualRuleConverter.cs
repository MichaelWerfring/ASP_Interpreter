//-----------------------------------------------------------------------
// <copyright file="DualRuleConverter.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Preprocessing.DualRules
{
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Types.TypeVisitors.Copy;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using VariableTerm = Asp_interpreter_lib.Types.Terms.VariableTerm;

    /// <summary>
    /// Class to convert a conventional ASP program to its dual.
    /// </summary>
    public class DualRuleConverter
    {
        private readonly VariableFinder variableFinder;

        private readonly PrefixOptions options;

        private readonly AnonymousVariableReplacer replacer;

        private readonly ILogger logger;

        private readonly bool wrapInNot;

        /// <summary>
        /// Initializes a new instance of the <see cref="DualRuleConverter"/> class.
        /// </summary>
        /// <param name="options">Prefixes the converter should append onto generated rules.</param>
        /// <param name="logger">A logger to log conversion state and errors.</param>
        /// <param name="wrapInNot">A boolean value indicating whether literals should be wrapped into
        /// negation as failure instead of setting a boolean flag.</param>
        /// <exception cref="ArgumentNullException">Is thrown if...
        /// ...options is null.
        /// ...logger is null.</exception>
        public DualRuleConverter(
            PrefixOptions options,
            ILogger logger,
            bool wrapInNot = false)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(logger);

            this.options = options;
            this.logger = logger;
            this.wrapInNot = wrapInNot;
            this.replacer = new AnonymousVariableReplacer(options);
            this.variableFinder = new VariableFinder();
        }

        /// <summary>
        /// Gets the replacer for anonymous variables.
        /// </summary>
        public AnonymousVariableReplacer Replacer
        {
            get
            {
                return this.replacer;
            }
        }

        /// <summary>
        /// Wraps a literal into a combination of classical negation an negation as failure.
        /// </summary>
        /// <param name="literal">The literal to be wrapped.</param>
        /// <returns>The literal wrapped into a combination of classical negation and negation as failure.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the literal is null.</exception>
        public static Literal WrapInNot(Literal literal)
        {
            ArgumentNullException.ThrowIfNull(literal);

            var terms =
                    literal.Terms.Select(t => t.Accept(new TermCopyVisitor()).
                        GetValueOrThrow("Failed to parse term!")).ToList();

            if (literal.HasStrongNegation)
            {
                return new Literal(
                    "not",
                    false,
                    false,
                    [new NegatedTerm(new ParenthesizedTerm(new BasicTerm(literal.Identifier.ToString(), terms)))]);
            }

            return new Literal(
                "not",
                false,
                false,
                [new BasicTerm(literal.Identifier.ToString(), terms)]);
        }

        /// <summary>
        /// Adds a forall quantifier to a given list of variables and a goal.
        /// </summary>
        /// <param name="bodyVariables">The body variables that should be treated by the forall.</param>
        /// <param name="innerGoal">The literal to be at the innermost position of the forall.</param>
        /// <returns>The innermost goal wrapped into multiple forall statements.</returns>
        /// /// <exception cref="ArgumentNullException">Is thrown if...
        /// ...bodyVariables is null.
        /// ...innerGoal is null.</exception>
        public static Goal NestForall(List<string> bodyVariables, Literal innerGoal)
        {
            ArgumentNullException.ThrowIfNull(bodyVariables);
            ArgumentNullException.ThrowIfNull(innerGoal);

            if (bodyVariables.Count == 0)
            {
                return innerGoal;
            }

            string variable = bodyVariables[0];
            bodyVariables.RemoveAt(0);

            var result = NestForall(bodyVariables, innerGoal);

            return new Forall(new VariableTerm(variable), result);
        }

        /// <summary>
        /// Moves atoms from the head of a rule to its body and renames variables,
        /// if the statement has no head it will not be altered.
        /// </summary>
        /// <param name="rule">The rule to be processed.</param>
        /// <returns>The rule without any atoms in the head and potentially a different body.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the statement is null.</exception>
        public Statement ComputeHead(Statement rule)
        {
            ArgumentNullException.ThrowIfNull(rule);

            HeadAtomEliminator rewriter = new(this.options);
            var statement = rewriter.Rewrite(rule);

            this.logger.LogTrace("Rewrite head from: " + rule.ToString() + " to: " + statement.ToString());
            return statement;
        }

        /// <summary>
        /// Converts a given set of rules to its duals.
        /// </summary>
        /// <param name="rules">The rules to be converted.</param>
        /// <param name="prefix">The prefix to append onto new rules, if no value is provided it will not rename rules.</param>
        /// <returns>The input rules converted to their duals.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if...
        /// ...rules is null.
        /// ...prefix is null.</exception>
        public List<Statement> GetDualRules(IEnumerable<Statement> rules, string prefix = "")
        {
            ArgumentNullException.ThrowIfNull(rules);
            ArgumentNullException.ThrowIfNull(prefix);

            this.logger.LogInfo("Generating dual rules...");

            List<Statement> duals = new();

            var statements = rules.ToList();
            var withoutAnonymous = statements.Select(this.replacer.Replace);

            var headComputed = withoutAnonymous.Select(this.ComputeHead).ToList();
            var bodyGoals = this.GetGoalsOnlyAppearingInBody(headComputed);
            var disjunctions = this.PreprocessRules(headComputed);
            foreach (var disjunction in disjunctions)
            {
                duals.AddRange(this.ToConjunction(disjunction, prefix));
            }

            duals.AddRange(bodyGoals);

            this.logger.LogDebug("The dual rules for program: ");
            duals.ForEach(d => this.logger.LogDebug(d.ToString()));

            return duals;
        }

        /// <summary>
        /// Wraps the given rule into a new rule and generates new rules for each goal in the body,
        /// if it has no head it will be skipped.
        /// </summary>
        /// <param name="rule">The rule to be converted.</param>
        /// <param name="prefix">The prefix to append to generated rules.</param>
        /// <returns>New rules resulting from the conversion.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if...
        /// ...rules is null.
        /// ...prefix is null.</exception>
        public IEnumerable<Statement> ToDisjunction(Statement rule, string prefix = "")
        {
            ArgumentNullException.ThrowIfNull(rule);
            ArgumentNullException.ThrowIfNull(prefix);

            if (!rule.HasHead)
            {
                return [rule];
            }

            List<Statement> duals = new();
            var head = rule.Head.GetValueOrThrow();

            // If it is a fact, just add the prefix
            if (!rule.HasBody)
            {
                Literal newHead;

                if (this.wrapInNot)
                {
                    newHead = WrapInNot(head);
                }
                else
                {
                    newHead = new Literal(
                        head.Identifier,
                        true,
                        head.HasStrongNegation,
                        head.Terms);
                }

                var newFact = new Statement();
                newFact.AddHead(newHead);
                duals.Add(newFact);
                return duals;
            }

            bool forallApplicable = this.GetBodyVariables(rule).Count != 0;

            for (var i = 0; i < rule.Body.Count; i++)
            {
                if (forallApplicable)
                {
                    duals.AddRange(this.AddForall(rule, prefix));
                    continue;
                }

                var goal = rule.Body[i];
                var dualGoal = GoalNegator.Negate(goal, this.wrapInNot);

                Literal newHead;
                if (this.wrapInNot)
                {
                    newHead = WrapInNot(new Literal(
                        prefix + head.Identifier,
                        true,
                        head.HasStrongNegation,
                        head.Terms));
                }
                else
                {
                    newHead = new Literal(
                        prefix + head.Identifier,
                        true,
                        head.HasStrongNegation,
                        head.Terms);
                }

                var body = new List<Goal>();

                // Add preceding goals
                if (this.wrapInNot)
                {
                    body = rule.Body[0..i].Select(WrapInNot).ToList();
                }
                else
                {
                    body = rule.Body[0..i];
                }

                body.Add(dualGoal);

                // Add new statement to duals
                var dualStatement = new Statement();
                dualStatement.AddHead(newHead);
                dualStatement.AddBody(body);

                duals.Add(dualStatement);
            }

            return duals;
        }

        /// <summary>
        /// Adds the forall quantifier to a rule and generates new rules for each goal in the body,
        /// if it has a head and any variables only appearing in the body.
        /// </summary>
        /// <param name="rule">The rule to add forall to.</param>
        /// <param name="prefix">The prefix to add to generated rules.</param>
        /// <returns>New rules resulting from the conversion.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if...
        /// ...rules is null.
        /// ...prefix is null.</exception>
        public IEnumerable<Statement> AddForall(Statement rule, string prefix = "")
        {
            ArgumentNullException.ThrowIfNull(rule);
            ArgumentNullException.ThrowIfNull(prefix);

            var bodyVariables = this.GetBodyVariables(rule);

            // Headless statements are treated by the NMR Check
            if (!rule.HasHead || bodyVariables.Count == 0)
            {
                return [rule];
            }

            // Copy the statement before
            var ruleCopy = rule.Accept(new StatementCopyVisitor()).GetValueOrThrow("Cannot copy rule!");
            var ruleCopyHead = ruleCopy.Head.GetValueOrThrow("Cannot retrieve head from copy!");

            // get all variables from body
            var variables = ruleCopy.Accept(this.variableFinder).GetValueOrThrow("Cannot retrieve variables from body!")
                .DistinctBy(v => v.Identifier);

            // put all variables form body into head
            ruleCopyHead.Terms = new();
            ruleCopyHead.Identifier = this.options.ForallPrefix + ruleCopyHead.Identifier;

            foreach (var variable in variables)
            {
                ruleCopyHead.Terms.Add(new VariableTerm(variable.Identifier));
            }

            // generate duals normally
            var duals = this.ToDisjunction(ruleCopy, prefix).ToList();

            // add forall over the new predicate
            var innerGoal = duals.First().Head.GetValueOrThrow();

            // append body with (nested) forall
            rule.Body.Clear();
            rule.Body.AddRange([NestForall([.. bodyVariables], innerGoal)]);

            // prefix like dual
            var head = rule.Head.GetValueOrThrow();
            head.Identifier = prefix + head.Identifier;

            if (this.wrapInNot)
            {
                rule.Head = new Some<Literal>(WrapInNot(head));
            }
            else
            {
                head.HasNafNegation = true;
            }

            duals.Insert(0, rule);

            return duals;
        }

        /// <summary>
        /// Converts the given disjunctions to conjunctions by wrapping into new rules.
        /// </summary>
        /// <param name="disjunction">The identifier, arity and classical negation, mapped to all its rules within the program.</param>
        /// <param name="prefix">The prefix to append to generated rules.</param>
        /// <returns>The dual rules resulting form the conversion.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if...
        /// ...disjunction is null.
        /// ...prefix is null.</exception>
        public IEnumerable<Statement> ToConjunction(
            KeyValuePair<(string Id, int Arity, bool ClassicalNeg), List<Statement>> disjunction, string prefix = "")
        {
            ArgumentNullException.ThrowIfNull(disjunction);
            ArgumentNullException.ThrowIfNull(prefix);

            List<Statement> duals = new();

            // 1) generate new rule at top (named like input)
            Statement wrapper = new();

            if (this.wrapInNot)
            {
                wrapper.AddHead(WrapInNot(new Literal(
                    disjunction.Key.Id,
                    false,
                    disjunction.Key.ClassicalNeg,
                    AspExtensions.GenerateVariables(disjunction.Key.Arity, this.options.VariablePrefix))));
            }
            else
            {
                wrapper.AddHead(new Literal(
                    disjunction.Key.Id,
                    true,
                    disjunction.Key.ClassicalNeg,
                    AspExtensions.GenerateVariables(disjunction.Key.Arity, this.options.VariablePrefix)));
            }

            List<Goal> wrapperBody = new();

            for (var i = 0; i < disjunction.Value.Count; i++)
            {
                // 2) rename old rule heads
                var goal = disjunction.Value[i];
                var head = goal.Head.GetValueOrThrow();
                head.Identifier += i + 1;

                // 3) add heads to body of new rule

                // 3.1 insert variables from head into body goals
                var copy = goal.Head.GetValueOrThrow().Accept(new LiteralCopyVisitor(new TermCopyVisitor()))
                    .GetValueOrThrow("Cannot copy rule!");

                copy.Terms.Clear();
                copy.Terms.AddRange(AspExtensions.GenerateVariables(disjunction.Key.Arity, this.options.VariablePrefix));

                copy.Identifier = prefix + copy.Identifier;

                if (this.wrapInNot)
                {
                    copy = WrapInNot(copy);
                }
                else
                {
                    copy.HasNafNegation = true;
                }

                wrapperBody.Add(copy);

                // 4) generate duals for old rules
                // 5) add duals to list
                // If the goal is an atom we do not want its duals
                if (goal.HasBody)
                {
                    duals.AddRange(this.ToDisjunction(goal, prefix));
                }
            }

            wrapper.AddBody(wrapperBody);
            duals.Insert(0, wrapper);

            return duals;
        }

        /// <summary>
        /// Iterates through the rules to find all disjunctions.
        /// </summary>
        /// <param name="rules">The rules to retrieve the disjunctions for.</param>
        /// <returns>A dictionary containing the identifier, arity and classical negation mapped to all applicable statements.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the rules are null.</exception>
        public Dictionary<(string Id, int Arity, bool ClassicalNeg), List<Statement>> PreprocessRules(IEnumerable<Statement> rules)
        {
            ArgumentNullException.ThrowIfNull(rules);

            // heads mapped to all bodies occurring in the program
            Dictionary<(string, int, bool), List<Statement>> disjunctions = new();
            this.logger.LogTrace("Started preprocessing for dual rules.");

            foreach (var rule in rules)
            {
                // Headless rules will be treated within nmr check
                if (!rule.HasHead)
                {
                    continue;
                }

                var head = rule.Head.GetValueOrThrow("Headless rules must be treated by the NMR check!");
                var signature = (head.Identifier, head.Terms.Count, head.HasStrongNegation);
                var converted = this.ComputeHead(rule);

                if (!disjunctions.TryAdd(signature, [converted]))
                {
                    disjunctions[signature].Add(converted);
                    this.logger.LogTrace("Disjunction found: " +
                                     signature.HasStrongNegation +
                                     signature.Identifier + "/" + signature.Count);
                }
            }

            this.logger.LogTrace("Finished preprocessing for dual rules.");
            return disjunctions;
        }

        /// <summary>
        /// Wraps a goal into negation literals if it is negated.
        /// </summary>
        /// <param name="goal">The goal to be wrapped.</param>
        /// <returns>The goal wrapped into a not literal, if applicable.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the goal is null.</exception>
        private static Goal WrapInNot(Goal goal)
        {
            ArgumentNullException.ThrowIfNull(goal);

            var optionLiteral = goal.Accept(new GoalToLiteralConverter());

            if (!optionLiteral.HasValue)
            {
                return goal;
            }

            var literal = optionLiteral.GetValueOrThrow();

            if (literal.HasNafNegation)
            {
                return WrapInNot(literal);
            }

            if (literal.HasStrongNegation)
            {
                return new Literal("-", false, false, [new BasicTerm(literal.Identifier, literal.Terms)]);
            }

            return literal;
        }

        /// <summary>
        /// Retrieves all variables from the body of a rule that are not contained in its head.
        /// </summary>
        /// <param name="rule">The rule to retrieve body variables form.</param>
        /// <returns>A list of all the variables only contained in the body of the statement, but not in the head.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the rule is null.</exception>
        private List<string> GetBodyVariables(Statement rule)
        {
            ArgumentNullException.ThrowIfNull(rule);

            if (!rule.HasHead && rule.HasBody)
            {
                List<string> variables = new();
                foreach (var goal in rule.Body)
                {
                    variables.AddRange(goal.Accept(this.variableFinder).GetValueOrThrow("Cannot retrieve variables from body!")
                        .Select(v => v.Identifier));
                }

                return variables;
            }

            if (rule.HasHead && !rule.HasBody)
            {
                return new List<string>();
            }

            if (rule.HasHead && rule.HasBody)
            {
                List<string> bodyVar = [];
                foreach (var goal in rule.Body)
                {
                    bodyVar.AddRange(goal.Accept(this.variableFinder).GetValueOrThrow("Cannot retrieve variables from body!")
                        .Select(v => v.Identifier));
                }

                var headVar = rule.Head.GetValueOrThrow().Accept(this.variableFinder)
                    .GetValueOrThrow("Cannot retrieve variables from head!").Select(v => v.Identifier);

                return bodyVar.Except(headVar).ToList();
            }

            return new List<string>();
        }

        /// <summary>
        /// Returns all goals that only appear in the body of a rule, but nowhere else, to compute Clark's completion.
        /// </summary>
        /// <param name="rules">The rules to be searched for distinct goals.</param>
        /// <returns>The rules only appearing in the body of the given rules.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the rules are null.</exception>
        private IEnumerable<Statement> GetGoalsOnlyAppearingInBody(List<Statement> rules)
        {
            ArgumentNullException.ThrowIfNull(rules);

            HashSet<(string, int, bool)> heads = new();

            // First find all heads
            foreach (var rule in rules)
            {
                rule.Head.IfHasValue(h => heads.Add(
                    (h.Identifier, h.Terms.Count, h.HasStrongNegation)));
            }

            var literalVisitor = new GoalToLiteralConverter();

            List<Statement> statements = new List<Statement>();

            for (int i = 0; i < rules.Count; i++)
            {
                Statement rule = rules[i];

                foreach (var goal in rule.Body)
                {
                    var literal = goal.Accept(literalVisitor);
                    if (!literal.HasValue)
                    {
                        continue;
                    }

                    var actual = literal.GetValueOrThrow();

                    // Already found it
                    if (heads.Contains((actual.Identifier, actual.Terms.Count, actual.HasStrongNegation)))
                    {
                        continue;
                    }

                    Statement newStatement = new();

                    if (this.wrapInNot)
                    {
                        newStatement.AddHead(WrapInNot(actual));
                    }
                    else
                    {
                        newStatement.AddHead(new Literal(
                            actual.Identifier,
                            true,
                            actual.HasStrongNegation,
                            actual.Terms));
                    }

                    statements.Add(newStatement);
                }
            }

            return statements;
        }
    }
}