namespace Asp_interpreter_lib.Preprocessing.NMRCheck
{
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Preprocessing.DualRules;
    using Asp_interpreter_lib.Types;
    using Asp_interpreter_lib.Types.TypeVisitors;
    using Asp_interpreter_lib.Types.TypeVisitors.Copy;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Data;
    using System.Linq;

    public class NmrChecker
    {
        private readonly PrefixOptions options;

        private readonly ILogger logger;

        private readonly GoalToLiteralConverter goalToLiteralConverter;

        private readonly DualRuleConverter dualConverter;

        public NmrChecker(PrefixOptions options, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(logger);

            this.options = options;
            this.logger = logger;
            this.goalToLiteralConverter = new GoalToLiteralConverter();
            this.dualConverter = new DualRuleConverter(options, logger.GetDummy(), true);
        }

        public List<Statement> GetConstraintRules(AspProgram program)
        {
            ArgumentNullException.ThrowIfNull(program);

            List<Statement> statements = new List<Statement>();
            // Dictionary<(string, int), List<Literal>> distinctRules = new Dictionary<(string, int), List<Literal>>();
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

        private List<Statement> GetDualsForCheck(List<Statement> statements)
        {
            ArgumentNullException.ThrowIfNull(statements);

            List<Statement> duals = [];

            var withoutAnonymous = statements.Select(dualConverter.Replacer.Replace);
            var headComputed = withoutAnonymous.Select(dualConverter.ComputeHead).ToList();

            foreach (var statement in statements)
            {
                var head = statement.Head.GetValueOrThrow("Constraint rules must be given a head!");
                var kv = new KeyValuePair<(string, int, bool), List<Statement>>((head.Identifier, head.Terms.Count, head.HasStrongNegation), [statement]);
                duals.AddRange(dualConverter.ToConjunction(kv));
            }

            duals.ForEach(d => logger.LogDebug(d.ToString()));

            return duals;
        }

        public List<Statement> GetNmrCheck(List<Statement> olonRules, bool notAsName = true)
        {
            ArgumentNullException.ThrowIfNull(olonRules);

            logger.LogInfo("Generating NMR check...");
            if (olonRules.Count == 0)
            {
                logger.LogDebug("Finished generation because no OLON rules found in program.");
                var emptyCheck = new Statement();
                emptyCheck.AddHead(new Literal("_nmr_check", false, false, []));
                return [emptyCheck];
            }

            // 1) append negation of OLON Rule to its body (If not already present)
            List<Statement> preprocessedRules = PreprocessRules(olonRules);

            // 2) generate dual for modified rules
            var tempOlonRules =
                preprocessedRules.Select(r => r.Accept(new StatementCopyVisitor()).GetValueOrThrow()).ToList();

            List<Statement> duals = [];
            // 3) assign unique head (e.g. chk0) 
            duals = GetDualsForCheck(olonRules.ToList());
            AddMissingPrefixes(duals, "_");

            Statement nmrCheck = GetCheckRule(tempOlonRules, notAsName);
            AddForallToCheck(nmrCheck);

            duals.Insert(0, nmrCheck);

            return duals;
        }

        private void AddMissingPrefixes(List<Statement> duals, string prefix)
        {
            ArgumentNullException.ThrowIfNull(prefix);
            ArgumentNullException.ThrowIfNull(duals);

            foreach (var dual in duals)
            {
                //At this point in the program no rule should be headless
                var head = dual.Head.GetValueOrThrow();

                if (head.Identifier == "not")
                {
                    if (head.Terms.Count != 1)
                    {
                        throw new InvalidOperationException("Expected exactly one term in the not literal");
                    }

                    var basicTerm = head.Terms[0].Accept(new TermToBasicTermConverter()).GetValueOrThrow();

                    if (!basicTerm.Identifier.StartsWith(prefix))
                    {
                        basicTerm.Identifier = prefix + basicTerm.Identifier;
                    }
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

        private Statement GetCheckRule(IEnumerable<Statement> olonRules, bool notAsName = true)
        {
            ArgumentNullException.ThrowIfNull(olonRules);

            Statement nmrCheck = new();
            nmrCheck.AddHead(new Literal("_nmr_check", false, false, []));

            // add modified duals to the NMR check goal if it is not already in there
            var nmrBody = new List<Goal>();
            foreach (var rule in olonRules)
            {
                var head = rule.Head.GetValueOrThrow();
                head.HasNafNegation = !notAsName;
                nmrBody.Add(DualRuleConverter.WrapInNot(head));
            }

            nmrCheck.AddBody(nmrBody);
            return nmrCheck;
        }

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
                    string name = "_" + options.CheckPrefix + (i + 1) + "_";
                    rule.AddHead(new Literal(name, false, false, []));
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

                head.Identifier = "_" + options.CheckPrefix + (i + 1) + "_";
            }

            return olonRules;
        }

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
    }

}