using System.Xml.Schema;
using Antlr4.Runtime;
using asp_interpreter_lib;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Visitors;
using NUnit.Framework.Internal;

namespace asp_interpreter_test;

public class VisitorTest
{
    private string _graphCode;
    
    private TestingLogger _logger = new(LogLevel.Error);
    
    private GoalToLiteralConverter _goalToLiteralConverter = new();
    
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

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\n")]
    [TestCase("\t")]
    [TestCase("\r")]
    [TestCase("\r\n")]
    public void HandlesEmptyProgramCorrectly(string code)
    {
        TestingLogger infoLogger = new TestingLogger(LogLevel.Info);
        var inputStream = new AntlrInputStream(code);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.program();
        var visitor = new ProgramVisitor(_logger);
        var program = AspExtensions.GetProgram(code, infoLogger);

        Assert.That(
            infoLogger.Errors.Count == 0 && 
            infoLogger.InfoMessages.Count > 0);
    }
    
    [Test]
    public void HandlesProgramWithoutQueryCorrectly()
    {
        var code = """
                   a :- b.
                   """;

        var program = AspExtensions.GetProgram(code, _logger);
        
        Assert.That(_logger.Errors.Count == 0 && !program.Query.HasValue);
    }
    
    [Test]
    public void HandlesProgramWithoutStatementsCorrectly()
    {
        var code = """
                   a?
                   """;

        _logger.LogInfo("Program has been specifed without any statement!");

        //Program without statements is valid but still logs a warning
        Assert.That(_logger.Errors.Count == 0);
    }
    
    [Test]
    public void HandlesMinimalProgramCorrectly()
    {
        var code = """
                   a.
                   a?
                   """;

        _logger.LogInfo("Program has been specifed without any statement!");

        //Program without statements is valid but still logs a warning
        Assert.That(_logger.Errors.Count == 0);
    }
    
    [Test]
    public void HandlesClassicalNegationCorrectly()
    {
        var code = """
                   a(X) :- - b(X).
                   a?
                   """;
        
        var program = AspExtensions.GetProgram(code, _logger);
        
        var literal = program.Statements[0].Body[0].Accept(_goalToLiteralConverter);
        Assert.That(_logger.Errors.Count == 0 && literal.HasValue && literal.GetValueOrThrow() is 
            { Identifier: "b",
                HasStrongNegation: true,
                HasNafNegation: false
            });
    }
    
    [Test]
    public void HandlesNegationAsFailureCorrectly()
    {
        var code = """
                   a(X) :- not b(X).
                   a?
                   """;
        
        var program = AspExtensions.GetProgram(code, _logger);
        var literal = program.Statements[0].Body[0].Accept(_goalToLiteralConverter);
        Assert.That(_logger.Errors.Count == 0 && literal.HasValue && literal.GetValueOrThrow() is 
        { Identifier: "b",
            HasStrongNegation: false,
            HasNafNegation: true
        });
    }
    
    [Test]
    public void HandlesNafAndClassicalNegationCorrectly()
    {
        var code = """
                   a(X) :- not -b(X).
                   a?
                   """;
        
        var program = AspExtensions.GetProgram(code, _logger);
        var literal = program.Statements[0].Body[0].Accept(_goalToLiteralConverter);
        Assert.That(_logger.Errors.Count == 0 && literal.HasValue && literal.GetValueOrThrow() is 
        { Identifier: "b",
            HasStrongNegation: true,
            HasNafNegation: true
        });
    }
    
    [Test]
    public void ThrowsExceptionOnClassicalNegationAndNafCorrectly()
    {
        //This code cannot exist due to grammar specification
        var code = """
                   a(X) :- - not b(X).
                   a?
                   """;
        
        Assert.Throws<ArgumentException>(() => AspExtensions.GetProgram(code, _logger));
        Assert.That(_logger.Errors.Count == 0);
    }
    
    [Test]
    public void HandlesStatementWithoutRuleHeadCorrectly()
    {
        var code = """
                   :- b.
                   b?
                   """;
        
        var program = AspExtensions.GetProgram(code, _logger);
        var statement = program.Statements[0];
        
        var literal = program.Statements[0].Body[0].Accept(_goalToLiteralConverter);
        Assert.That(
            _logger.Errors.Count == 0 &&
            literal.HasValue &&
            statement.HasBody && 
            !statement.HasHead&&
            literal.GetValueOrThrow() is 
        { Identifier: "b",
            HasStrongNegation: false,
            HasNafNegation: false
        });
    }
    
    [Test]
    public void HandlesStatementWithoutRuleBodyCorrectly()
    {
        var code = """
                   a.
                   a?
                   """;
        
        var program = AspExtensions.GetProgram(code, _logger);
        var statement = program.Statements[0];
        
        var literal = statement.Head.GetValueOrThrow().Accept(_goalToLiteralConverter);
        Assert.That(
            _logger.Errors.Count == 0 &&
            literal.HasValue &&
            !statement.HasBody && 
            statement.HasHead&&
            literal.GetValueOrThrow() is 
            { Identifier: "a",
                HasStrongNegation: false,
                HasNafNegation: false
            });
    }
    
    [Test]
    public void HandlesStatementWithRuleBodyAndHeadCorrectly()
    {
        var code = """
                   a :- b.
                   a?
                   """;
        
        var program = AspExtensions.GetProgram(code, _logger);
        var statement = program.Statements[0];
        
        var body = program.Statements[0].Body[0].Accept(_goalToLiteralConverter);
        Assert.That(
            _logger.Errors.Count == 0 &&
            body.HasValue &&
            statement.Head.HasValue && 
            statement.Head.GetValueOrThrow() is
                {
                    Identifier: "a",
                    HasStrongNegation: false,
                    HasNafNegation: false
                } &&
            body.GetValueOrThrow() is 
            { 
                Identifier: "b",
                HasStrongNegation: false,
                HasNafNegation: false
            });
    }
    
    [Test]
    public void HandlesStatementWithRuleBodyAndNegatedHeadCorrectly()
    {
        var code = """
                   -a :- b.
                   a?
                   """;
        
        var program = AspExtensions.GetProgram(code, _logger);
        var statement = program.Statements[0];
        
        var body = program.Statements[0].Body[0].Accept(_goalToLiteralConverter);
        Assert.That(
            _logger.Errors.Count == 0 &&
            body.HasValue &&
            statement.Head.HasValue && 
            statement.Head.GetValueOrThrow() is
            {
                Identifier: "a",
                HasStrongNegation: true,
                HasNafNegation: false
            } &&
            body.GetValueOrThrow() is 
            { 
                Identifier: "b",
                HasStrongNegation: false,
                HasNafNegation: false
            });
    }
    
    [Test]
    public void HandlesTermsInHeadCorrectly()
    {
        AspProgram program = AspExtensions.GetProgram(_graphCode, _logger);

        var headLiteral = program.Statements[6].Head.GetValueOrThrow(); 
        var firstTerm = headLiteral.Terms[0] as VariableTerm;
        var secondTerm = headLiteral.Terms[1] as VariableTerm;

        Assert.That(
            headLiteral is
                { Identifier: "separate", HasStrongNegation: false, HasNafNegation: false, Terms.Count: 2 } &&
            firstTerm?.Identifier == "X" && secondTerm?.Identifier == "Y");
        Assert.That(_logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesLiteralInQueryCorrectly()
    {
        AspProgram program = AspExtensions.GetProgram(_graphCode, _logger);

        var queryLiteral = program.Query.GetValueOrThrow().Literal;
        var firstTerm = queryLiteral.Terms[0] as VariableTerm;
        var secondTerm = queryLiteral.Terms[1] as BasicTerm;

        Assert.That(
            queryLiteral is { Identifier: "edge", HasStrongNegation: false, HasNafNegation:false, Terms.Count: 2 } &&
            firstTerm.Identifier == "X" && secondTerm.Identifier == "b");
        Assert.That(_logger.Errors.Count == 0);
    }
    
    [Test]
    public void HandlesBodyLiteralsCorrectly()
    {
        AspProgram program = AspExtensions.GetProgram(_graphCode, _logger);

        var body = program.Statements[6].Body;
        var firstLiteral = body[0].Accept(_goalToLiteralConverter).GetValueOrThrow();
        var secondLiteral = body[1].Accept(_goalToLiteralConverter).GetValueOrThrow();
        var thirdLiteral = body[2].Accept(_goalToLiteralConverter).GetValueOrThrow();

        Assert.That(
            body.Count == 3 &&
            firstLiteral.Identifier == "node" && !firstLiteral.HasNafNegation &&
            secondLiteral.Identifier == "node" && !secondLiteral.HasNafNegation &&
            thirdLiteral.HasNafNegation && thirdLiteral.Identifier == "edge");
        Assert.That(_logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesBodyTermsCorrectly()
    {
        var program = AspExtensions.GetProgram(_graphCode, _logger);
        
        var body = program.Statements[6].Body;
        var firstLiteral = body[0].Accept(_goalToLiteralConverter).GetValueOrThrow();
        var secondLiteral = body[1].Accept(_goalToLiteralConverter).GetValueOrThrow();
        var thirdLiteral = body[2].Accept(_goalToLiteralConverter).GetValueOrThrow();

        if (firstLiteral.Terms[0] is VariableTerm firstVarTerm)
        {
            Assert.That(firstVarTerm.Identifier == "X");
        }
        
        if (secondLiteral.Terms[0] is VariableTerm secondVarTerm)
        {
            Assert.That(secondVarTerm.Identifier == "Y");
        }
        
        if (thirdLiteral.Terms[0] is VariableTerm thirdVarTerm)
        {
            Assert.That(thirdVarTerm.Identifier == "X");
        }
        
        if (thirdLiteral.Terms[1] is VariableTerm fourthVarTerm)
        {
            Assert.That(fourthVarTerm.Identifier == "Y");
        }
        
        Assert.That(_logger.Errors.Count == 0);
    }
}