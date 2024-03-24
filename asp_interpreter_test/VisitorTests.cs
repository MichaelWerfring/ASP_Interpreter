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
    
    [Test]
    public void HandlesBodyLiteralsCorrectly()
    {
        AspProgram program = GetProgram(_graphCode);

        var body = program.Statements[6].Body;
        var firstLiteral = body.Literals[0];
        var secondLiteral = body.Literals[1];
        var thirdLiteral = body.Literals[2];

        Assert.That(
            body.Literals.Count == 3 &&
            firstLiteral.Literal.Identifier == "node" && !firstLiteral.Literal.Negated &&
            secondLiteral.Literal.Identifier == "node" && !secondLiteral.Literal.Negated &&
            thirdLiteral.Negated && thirdLiteral.Literal.Identifier == "edge");
    }

    [Test]
    public void HandlesBodyTermsCorrectly()
    {
        var program = GetProgram(_graphCode);
        
        var body = program.Statements[6].Body;
        var firstLiteral = body.Literals[0];
        var secondLiteral = body.Literals[1];
        var thirdLiteral = body.Literals[2];

        if (firstLiteral.Literal.Terms[0] is VariableTerm firstVarTerm)
        {
            Assert.That(firstVarTerm.Name == "X");
        }
        
        if (secondLiteral.Literal.Terms[0] is VariableTerm secondVarTerm)
        {
            Assert.That(secondVarTerm.Name == "Y");
        }
        
        if (thirdLiteral.Literal.Terms[0] is VariableTerm thirdVarTerm)
        {
            Assert.That(thirdVarTerm.Name == "X");
        }
        
        if (thirdLiteral.Literal.Terms[1] is VariableTerm fourthVarTerm)
        {
            Assert.That(fourthVarTerm.Name == "Y");
        }
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