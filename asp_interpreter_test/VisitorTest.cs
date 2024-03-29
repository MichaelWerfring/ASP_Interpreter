﻿using Antlr4.Runtime;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Visitors;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace asp_interpreter_test;

public class VisitorTest
{
    private string _graphCode;
    
    private MockErrorLogger _errorLogger = new();
    
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
        Assert.That(nafLiteral?.ClassicalLiteral is { Identifier: "b", Negated: true } && 
            !nafLiteral.IsNafNegated);
        Assert.That(_errorLogger.Errors.Count == 0);
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
        Assert.That(nafLiteral?.ClassicalLiteral is { Identifier: "b", Negated: false } && 
                    nafLiteral.IsNafNegated);
        Assert.That(_errorLogger.Errors.Count == 0);
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
        Assert.That(nafLiteral?.ClassicalLiteral is { Identifier: "b", Negated: true } && 
                    nafLiteral.IsNafNegated);
        Assert.That(_errorLogger.Errors.Count == 0);
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
        Assert.That(_errorLogger.Errors.Count == 0);
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
            statement.Body.Literals[0] is {IsNafNegated: false, ClassicalLiteral.Identifier: "b"});
        Assert.That(_errorLogger.Errors.Count == 0);
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
            statement.Head.Literal is {Negated: false, Identifier: "a"});
        Assert.That(_errorLogger.Errors.Count == 0);
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
            statement.Head.Literal is {Negated: false, Identifier: "a"} &&
            statement.Body.Literals[0] is {IsNafNegated: false, ClassicalLiteral.Identifier: "b"});
        Assert.That(_errorLogger.Errors.Count == 0);
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
            statement.Head.Literal is {Negated: true, Identifier: "a"} &&
            statement.Body.Literals[0] is {IsNafNegated: false, ClassicalLiteral.Identifier: "b"});
        Assert.That(_errorLogger.Errors.Count == 0);
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
            firstTerm.Identifier == "X" && secondTerm.Identifier == "Y");
        Assert.That(_errorLogger.Errors.Count == 0);
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
            firstTerm.Identifier == "X" && secondTerm.Identifier == "b");
        Assert.That(_errorLogger.Errors.Count == 0);
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
            firstLiteral.ClassicalLiteral.Identifier == "node" && !firstLiteral.ClassicalLiteral.Negated &&
            secondLiteral.ClassicalLiteral.Identifier == "node" && !secondLiteral.ClassicalLiteral.Negated &&
            thirdLiteral.IsNafNegated && thirdLiteral.ClassicalLiteral.Identifier == "edge");
        Assert.That(_errorLogger.Errors.Count == 0);
    }

    [Test]
    public void HandlesBodyTermsCorrectly()
    {
        var program = GetProgram(_graphCode);
        
        var body = program.Statements[6].Body;
        var firstLiteral = body.Literals[0];
        var secondLiteral = body.Literals[1];
        var thirdLiteral = body.Literals[2];

        if (firstLiteral.ClassicalLiteral.Terms[0] is VariableTerm firstVarTerm)
        {
            Assert.That(firstVarTerm.Identifier == "X");
        }
        
        if (secondLiteral.ClassicalLiteral.Terms[0] is VariableTerm secondVarTerm)
        {
            Assert.That(secondVarTerm.Identifier == "Y");
        }
        
        if (thirdLiteral.ClassicalLiteral.Terms[0] is VariableTerm thirdVarTerm)
        {
            Assert.That(thirdVarTerm.Identifier == "X");
        }
        
        if (thirdLiteral.ClassicalLiteral.Terms[1] is VariableTerm fourthVarTerm)
        {
            Assert.That(fourthVarTerm.Identifier == "Y");
        }
        
        Assert.That(_errorLogger.Errors.Count == 0);
    }
    
    // Helper method to get a program from a given code
    private AspProgram GetProgram(string code)
    {
        var inputStream = new AntlrInputStream(code);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.program();
        var visitor = new ProgramVisitor(_errorLogger);
        var program = visitor.VisitProgram(context); 
        
        return program.GetValueOrThrow();
    }
}