using Antlr4.Runtime;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Visitors;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace asp_interpreter_test;

public class VisitorTests
{
    private string _graphCode;
    
    [SetUp]
    public void Setup()
    {
        _graphCode = """
                     node(a).
                     node(b).
                     node(c).
                     edge(a, b).
                     edge(b, c).
                     edge(X, Y) :- node(X), node(Y), edge(Y, X).
                     separate(X, Y) :- node(X), node(Y), not edge(X, Y).

                     edge(X, b)?
                     """;
    }
    
    [Test]
    public void HandlesTermsInHeadCorrectly()
    {
        AspProgram program = GetProgram(_graphCode);

        var headLiteral = program.Statements[6].Head?.Literal; 
        var firstTerm = headLiteral.Terms[0] as VariableTerm;
        var secondTerm = headLiteral.Terms[1] as VariableTerm;

        Assert.That(
            headLiteral is { Identifier: "separate", Negated: false, Terms.Count: 2 } &&
            firstTerm.Name == "X" && secondTerm.Name == "Y");
    }

    [Test]
    public void HandlesLiteralInQueryCorrectly()
    {
        AspProgram program = GetProgram(_graphCode);

        var queryLiteral = program.Query?.ClassicalLiteral;
        var firstTerm = queryLiteral.Terms[0] as VariableTerm;
        var secondTerm = queryLiteral.Terms[1] as BasicTerm;

        Assert.That(
            queryLiteral is { Identifier: "edge", Negated: false, Terms.Count: 2 } &&
            firstTerm.Name == "X" && secondTerm.Identifier == "b");
    }
    
    // Helper method to get a program from a given code
    private AspProgram GetProgram(string code)
    {
        var inputStream = new AntlrInputStream(code);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.program();
        var visitor = new ProgramVisitor();
        return visitor.VisitProgram(context);
    }
}