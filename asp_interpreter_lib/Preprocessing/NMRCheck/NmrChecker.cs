using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Solving.NMRCheck;

public class NmrChecker(PrefixOptions options, ILogger logger)
{
    private readonly PrefixOptions _options = options ?? 
        throw new ArgumentNullException(nameof(options), "The given argument must not be null!");

    private readonly ILogger _logger = logger ?? 
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    private static readonly GoalToLiteralConverter _goalToLiteralConverter = new();

    public List<Statement> GetSubCheckRules(List<Statement> olonRules)
    {
        ArgumentNullException.ThrowIfNull(olonRules);

        _logger.LogInfo("Generating NMR check...");
        if (olonRules.Count == 0)
        {
            _logger.LogDebug("Finished generation because no OLON rules found in program.");
            return olonRules;
        }
        
        // 1) append negation of OLON Rule to its body (If not already present)
        List<Statement> preprocessedRules = PreprocessRules(olonRules);
        
        // 2) generate dual for modified rule
        // dummy logger is enough because we do not want to log twice
        DualRuleConverter converter = new DualRuleConverter(_options, _logger.GetDummy());
        var tempOlonRules = 
            olonRules.Select(r => r.Accept(new StatementCopyVisitor()).GetValueOrThrow());
        var duals = converter.GetDualRules(tempOlonRules);
        
        // 3) assign unique head (e.g. chk0)
        duals.ForEach(d => 
            d.Head.GetValueOrThrow().Identifier = _options.CheckPrefix + d.Head.GetValueOrThrow().Identifier);
        
        
        Statement nmrCheck = GetCheckForDuals(olonRules);
        AddForallToCheck(nmrCheck);
        
        duals.Insert(0, nmrCheck);

        _logger.LogDebug("NMR check for programm: ");
        duals.ForEach(d => _logger.LogDebug(d.ToString()));

        return duals;
    }

    private Statement GetCheckForDuals(List<Statement>olonRules)
    {
        Statement nmrCheck = new();
        nmrCheck.AddHead(new Literal("nmr_check", false, false, []));

        // 4) add modified duals to the NMR check goal if it is not already in there
        var distinctOlon = olonRules.DistinctBy(r =>
        {
            var head = r.Head.GetValueOrThrow();
            return head.Identifier + head.Terms.Count + head.HasStrongNegation;
        });

        var nmrBody = new List<Goal>();
        foreach (var rule in distinctOlon)
        {
            var head = rule.Head.GetValueOrThrow();
            head.Identifier = _options.CheckPrefix + _options.DualPrefix + head.Identifier;
            nmrBody.Add(head);
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
        
        // 1) append negation of OLON Rule to its body (If not already present)
        int emptyHeadCount = 0;
        foreach (var rule in olonRules)
        {
            if (!rule.HasHead)
            {
                string name = _options.EmptyHeadPrefix + emptyHeadCount++;
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
        }

        return olonRules;
    }

    private static void AddForallToCheck(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        //Variable are body variables implicitly
        List<string> variables = [];
        VariableFinder variableFinder = new();
        foreach (var goal in statement.Body)
        {
            variables.AddRange(goal.Accept(variableFinder).
                GetValueOrThrow("Cannot retrieve variables from body!").
                Select(v => v.Identifier));
        }

        for (var i = 0; i < statement.Body.Count; i++)
        {
            var literal = statement.Body[i].Accept(_goalToLiteralConverter);
            
            if (!literal.HasValue)
            {
                continue;
            }

            var innerGoal = literal.GetValueOrThrow();

            var variablesInGoal = innerGoal.Accept(variableFinder).GetValueOrThrow();

            if (variablesInGoal.Count == 0)
            {
                continue;
            }
            
            var forall = DualRuleConverter.NestForall(variables.ToList(), innerGoal);
            statement.Body[i] = forall;
        }
    }
}