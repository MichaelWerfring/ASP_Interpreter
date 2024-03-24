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
    
    //Disjunction in Rule Head
    //STMT without Rule Head
    //STMT without Rule Body (fact)
    //STMT with Rule Body and Head (rule)
    // -a
    // not a
    // not -a 
    // - not a => must be wrong

    [Test]
    public void HandlesClassicalNegationCorrectly()
    {
        var code = """
                   a(X) :- - b(X).
                   a?
                   """;
        
        var program = GetProgram(code);
        var nafLiteral = program.Statements[0].Body?.Literals[0]; 
        
        //First check inner part for classical negation
        //Then outer part for NAF
        Assert.That(nafLiteral?.Literal is { Identifier: "b", Negated: true } && 
            !nafLiteral.Negated);
    }
    
    [Test]
    public void HandlesNegationAsFailureCorrectly()
    {
        var code = """
                   a(X) :- not b(X).
                   a?
                   """;
        
        var program = GetProgram(code);
        var nafLiteral = program.Statements[0].Body?.Literals[0]; 
        
        //First check inner part for classical negation
        //Then outer part for NAF
        Assert.That(nafLiteral?.Literal is { Identifier: "b", Negated: false } && 
                    nafLiteral.Negated);
    }
    
    [Test]
    public void HandlesNafAndClassicalNegationCorrectly()
    {
        var code = """
                   a(X) :- not -b(X).
                   a?
                   """;
        
        var program = GetProgram(code);
        var nafLiteral = program.Statements[0].Body?.Literals[0]; 
        
        //First check inner part for classical negation
        //Then outer part for NAF
        Assert.That(nafLiteral?.Literal is { Identifier: "b", Negated: true } && 
                    nafLiteral.Negated);
    }
    
    [Test]
    public void ThrowsExceptionOnClassicalNegationAndNafCorrectly()
    {
        //This code cannot exist due to grammar specification
        var code = """
                   a(X) :- - not b(X).
                   a?
                   """;
        
        Assert.Throws<NullReferenceException>(() => GetProgram(code));
    }

    [Test]
    public void HandlesDisjunctionInRuleHeadCorrectly()
    {
        var code = """
                   a(X) | b(X) | c(X):- d(X).
                   a?
                   """;
        
        var program = GetProgram(code);
        
        var head = program.Statements[0].Head;
        var firstLiteral = head?.Literals[0];
        var secondLiteral = head?.Literals[1];
        var thirdLiteral = head?.Literals[2];
        
        Assert.That(
            head?.Literals.Count == 3 &&
            firstLiteral is {Negated:false, Identifier: "a"} &&
            secondLiteral is {Negated:false, Identifier: "b"} &&
            thirdLiteral is {Negated:false, Identifier: "c"});
    }
    
    [Test]
    public void HandlesStatementWithoutRuleHeadCorrectly()
    {
        var code = """
                   :- b.
                   b?
                   """;
        
        var program = GetProgram(code);
        var statement = program.Statements[0];
        
        Assert.That(
            statement.HasBody && 
            !statement.HasHead &&
            statement.Head == null &&
            statement.Body != null && 
            statement.Body.Literals[0] is {Negated: false, Literal.Identifier: "b"});
    }
    
    [Test]
    public void HandlesStatementWithoutRuleBodyCorrectly()
    {
        var code = """
                   a :- .
                   a?
                   """;
        
        var program = GetProgram(code);
        var statement = program.Statements[0];
        
        Assert.That(
            !statement.HasBody && 
            statement.HasHead &&
            statement.Head != null &&
            statement.Body == null && 
            statement.Head.Literals[0] is {Negated: false, Identifier: "a"});
    }
    
    [Test]
    public void HandlesStatementWithRuleBodyAndHeadCorrectly()
    {
        var code = """
                   a :- b.
                   a?
                   """;
        
        var program = GetProgram(code);
        var statement = program.Statements[0];
        
        Assert.That(
            statement.HasBody && 
            statement.HasHead &&
            statement.Head != null &&
            statement.Body != null && 
            statement.Head.Literals[0] is {Negated: false, Identifier: "a"} &&
            statement.Body.Literals[0] is {Negated: false, Literal.Identifier: "b"});
    }
    
    [Test]
    public void HandlesStatementWithRuleBodyAndNegatedHeadCorrectly()
    {
        var code = """
                   -a :- b.
                   a?
                   """;
        
        var program = GetProgram(code);
        var statement = program.Statements[0];
        
        Assert.That(
            statement.HasBody && 
            statement.HasHead &&
            statement.Head != null &&
            statement.Body != null && 
            statement.Head.Literals[0] is {Negated: true, Identifier: "a"} &&
            statement.Body.Literals[0] is {Negated: false, Literal.Identifier: "b"});
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