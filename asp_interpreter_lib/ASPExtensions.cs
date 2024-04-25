using Antlr4.Runtime;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Visitors;

namespace asp_interpreter_lib;

public static class ASPExtensions
{
    private static PrefixOptions _commonPrefixes = new PrefixOptions(
        "rwh_", "fa_", "eh", "chk_", "not_", "V");
    
    public static PrefixOptions CommonPrefixes { get; } = _commonPrefixes;
    
    // Helper method to get a program from a given code
    public static AspProgram GetProgram(string code, IErrorLogger logger)
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
    
    public static string GenerateUniqeName(string currentName, HashSet<string> variables, string prefix, bool useUnderline = true)
    {
        ArgumentNullException.ThrowIfNull(variables);
        
        //int rewriteCount = variables.Count(v => v.StartsWith(prefix) && v.EndsWith(currentName));
        //string newVariable = prefix + rewriteCount + "_" + currentName;
        //variables.Add(newVariable);
        //return newVariable;
        
        int count = 0;
        string newVariable;

        do
        {
            newVariable= prefix + count + (useUnderline ? "_" : "") + currentName;
            count++;
        } while (variables.Contains(newVariable));
        
        variables.Add(newVariable);
        return newVariable;
    }
    
    public static string GenerateUniqeName(string currentName, List<string> variables, string prefix, bool useUnderline = true)
    {
        ArgumentNullException.ThrowIfNull(variables);
        
        int count = 0;
        string newVariable;
        do
        {
            newVariable= prefix + count + (useUnderline ? "_" : "") + currentName;
            count++;
        } while (variables.Contains(newVariable));
        
        variables.Add(newVariable);
        return newVariable;
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
}