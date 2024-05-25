using System.Text;
using Antlr4.Runtime;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Types.TypeVisitors.Copy;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Visitors;
using Asp_interpreter_lib.Preprocessing;

namespace Asp_interpreter_lib.Util;

public static class AspExtensions
{
    private static readonly PrefixOptions _commonPrefixes = new(
        "fa_",
        "eh",
        "chk_",
        "not_",
        "V");
    
    public static PrefixOptions CommonPrefixes { get; } = _commonPrefixes;
    
    // Helper method to get a program from a given code
    public static AspProgram GetProgram(string code, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(logger);

        var inputStream = new AntlrInputStream(code);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.program();
        var visitor = new ProgramVisitor(logger);
        var program = visitor.VisitProgram(context); 
        
        return program.GetValueOrThrow();
    }

    public static string GetCopy(this string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        string copy = string.Empty;

        for (int i = 0; i < input.Length; i++)
        {
            copy += input[i];
        }

        return copy;
    }
    
    /// <summary>
    /// Provides a readable string representation for lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ListToString<T>(this List<T> list)
    {
        if (list.Count == 0) return string.Empty;

        var sb = new StringBuilder();

        for (int i = 0; i < list.Count - 1; i++)
        {
            sb.Append($"{list[i].ToString()}, ");
        }

        sb.Append($"{list[^1].ToString()}");

        return sb.ToString();
    }

    public static List<Statement> Duplicate(this List<Statement> statements)
    {
        var newStatements = new List<Statement>();
        var visitor = new StatementCopyVisitor();
        foreach (var statement in statements)
        {
            newStatements.Add(statement.Accept(visitor).GetValueOrThrow());
        }

        return newStatements;
    }

    public static AspProgram Duplicate(this AspProgram program)
    {
        var statements = program.Statements.Duplicate();

        if (!program.Query.HasValue)
        {
            return new AspProgram(statements, new None<Query>(), program.Explanations);
        }

        var queryCopy = new List<Goal>();
        var query = program.Query.GetValueOrThrow();
        var goalCopyVisitor = new GoalCopyVisitor(new TermCopyVisitor());

        foreach (var goal in query.Goals)
        {
            queryCopy.Add(goal.Accept(goalCopyVisitor).GetValueOrThrow());
        }
        
        return new AspProgram(statements, new Some<Query>(new Query(queryCopy)),program.Explanations);
    }

    public static string SimplifyMapping(VariableMapping mapping)
    {
        var postProcessor = new VariableMappingPostprocessor();
        var simplifiedMapping = postProcessor.Postprocess(mapping);
        var sb = new StringBuilder();

        sb.Append("{ ");

        foreach (var pair in simplifiedMapping)
        {
            if (pair.Value is TermBinding termBinding)
            {
                sb.Append($"{pair.Key} = {termBinding.Term}");
            }
            else if (pair.Value is ProhibitedValuesBinding binding)
            {
                sb.AppendLine($"{pair.Key} \\= {{ {binding.ProhibitedValues.ToList().ListToString()} }}");
            }

            if(simplifiedMapping.Count > 1)
            {
                sb.Append(", ");
            }
        }

        sb.Append(" }");

        return sb.ToString();
    }

    public static List<Statement> PrefixNestedTerms(this List<Statement> statements, string prefix)
    {
        ArgumentNullException.ThrowIfNull(statements);
        ArgumentNullException.ThrowIfNull(prefix);

        var result = new List<Statement>();
        var basicTermConverter = new TermToBasicTermConverter();

        foreach (var statement in statements)
        {
            

            result.Add(statement);
        }

        return result;
    }

    public static List<ITerm> GenerateVariables(int numberOfVariables, string variableName)
    {
        ArgumentException.ThrowIfNullOrEmpty(variableName);
        ArgumentOutOfRangeException.ThrowIfNegative(numberOfVariables);

        List<ITerm> vars = [];

        for (int i = 0; i < numberOfVariables; i++)
        {
            vars.Add(new VariableTerm(variableName + (i + 1)));
        }

        return vars;
    }

    public static bool CompareGoal(Goal goal, bool naf, bool neg, string id, string[] terms)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(terms);
        ArgumentNullException.ThrowIfNull(goal);

        var maybeLiteral = goal.Accept(new GoalToLiteralConverter());

        if (!maybeLiteral.HasValue)
        {
            return false;
        }

        var literal = maybeLiteral.GetValueOrThrow();

        if (literal.HasNafNegation != naf ||
            literal.HasStrongNegation != neg ||
            literal.Identifier != id)
        {
            return false;
        }

        if (terms.Length != literal.Terms.Count)
        {
            return false;
        }

        for (int i = 0; i < literal.Terms.Count; i++)
        {
            ITerm term = literal.Terms[i];

            if (term.ToString() != terms[i])
            {
                return false;
            }
        }

        return true;
    }
}