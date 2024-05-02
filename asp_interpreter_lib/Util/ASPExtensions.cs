using System.Text;
using Antlr4.Runtime;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Visitors;

namespace asp_interpreter_lib.Util;

public static class AspExtensions
{
    private static readonly PrefixOptions _commonPrefixes = new(
        "rwh_", 
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
}