//-----------------------------------------------------------------------
// <copyright file="VisitorTest.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_test;
using Antlr4.Runtime;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Visitors;
using NUnit.Framework.Internal;

public class VisitorTest
{
    private string graphCode;

    private readonly TestingLogger logger = new(LogLevels.Error);

    private readonly GoalToLiteralConverter goalToLiteralConverter = new();

    [SetUp]
    public void Setup()
    {
        this.graphCode = """
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
        var inputStream = new AntlrInputStream(code);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.program();
        var infoLogger = new TestingLogger(LogLevels.Info);
        var visitor = new ProgramVisitor(infoLogger);

        _ = visitor.VisitProgram(context);
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

        var program = AspExtensions.GetProgram(code, this.logger);

        Assert.That(this.logger.Errors.Count == 0 && !program.Query.HasValue);
    }

    [Test]
    public void HandlesProgramWithoutStatementsCorrectly()
    {
        Assert.That(logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesMinimalProgramCorrectly()
    {
        var code = """
                   a.
                   ?- a.
                   """;
        Assert.That(logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesClassicalNegationCorrectly()
    {
        var code = """
                   a(X) :- - b(X).
                   ?- a(Y).
                   """;

        var program = AspExtensions.GetProgram(code, this.logger);

        Assert.That(program.Statements.Count == 1);
        Assert.That(program.Statements[0].Body.Count == 1);
        Assert.That(AspExtensions.CompareGoal(program.Statements[0].Body[0], false, true, "b",["X"]));
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesNegationAsFailureCorrectly()
    {
        var code = """
                   a(X) :- not b(X).
                   ?- a(X).
                   """;

        var program = AspExtensions.GetProgram(code, this.logger);

        Assert.That(program.Statements.Count == 1);
        Assert.That(program.Statements[0].Body.Count == 1);
        Assert.That(AspExtensions.CompareGoal(program.Statements[0].Body[0], true, false, "b",["X"]));
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesNafAndClassicalNegationCorrectly()
    {
        var code = """
                   a(X) :- not -b(X).
                   ?- a(X).
                   """;

        var program = AspExtensions.GetProgram(code, this.logger);

        Assert.That(program.Statements.Count == 1);
        Assert.That(program.Statements[0].Body.Count == 1);
        Assert.That(AspExtensions.CompareGoal(program.Statements[0].Body[0], true, true, "b",["X"]));
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesMultipleLiteralsInQueryCorrectly()
    {
        // This code cannot exist due to grammar specification
        var code = """
                   a(X) :- c(X).
                   a(X) :- b(X).
                   b(1).
                   c(3).
                   ?- a(X), Y = 1 ,b(Y).
                   """;
        var program = AspExtensions.GetProgram(code, this.logger);

        Assert.That(program.Query.HasValue && program.Query.GetValueOrThrow().Goals.Count == 3);
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesStatementWithoutRuleHeadCorrectly()
    {
        var code = """
                   :- b.
                   ?- a(X).
                   """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var statement = program.Statements[0];

        Assert.That(statement.Body.Count == 1);
        Assert.That(AspExtensions.CompareGoal(statement.Body[0], false, false, "b",[]));
        Assert.That(!statement.Head.HasValue);
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesStatementWithoutRuleBodyCorrectly()
    {
        var code = """
                   a.
                   ?- a.
                   """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var statement = program.Statements[0];

        Assert.That(statement.Head.HasValue);
        Assert.That(AspExtensions.CompareGoal(statement.Head.GetValueOrThrow(), false, false, "a",[]));
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesStatementWithRuleBodyAndHeadCorrectly()
    {
        var code = """
                   a :- b.
                   ?- a.
                   """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var statement = program.Statements[0];

        var body = program.Statements[0].Body[0].Accept(this.goalToLiteralConverter);

        Assert.That(body.HasValue);
        Assert.That(AspExtensions.CompareGoal(body.GetValueOrThrow(), false, false, "b",[]));
        Assert.That(statement.Head.HasValue);
        Assert.That(AspExtensions.CompareGoal(statement.Head.GetValueOrThrow(), false, false, "a",[]));
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesStatementWithRuleBodyAndNegatedHeadCorrectly()
    {
        var code = """
                   -a :- b.
                   ?- -a.
                   """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var statement = program.Statements[0];
        var body = program.Statements[0].Body[0].Accept(this.goalToLiteralConverter);

        Assert.That(body.HasValue);
        Assert.That(AspExtensions.CompareGoal(body.GetValueOrThrow(), false, false, "b",[]));
        Assert.That(statement.Head.HasValue);
        Assert.That(AspExtensions.CompareGoal(statement.Head.GetValueOrThrow(), false, true, "a",[]));
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesTermsInHeadCorrectly()
    {
        AspProgram program = AspExtensions.GetProgram(this.graphCode, this.logger);

        var headLiteral = program.Statements[6].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(headLiteral, false, false, "separate",["X", "Y"]));
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesBodyLiteralsCorrectly()
    {
        AspProgram program = AspExtensions.GetProgram(this.graphCode, this.logger);

        var body = program.Statements[6].Body;
        var firstLiteral = body[0].Accept(this.goalToLiteralConverter).GetValueOrThrow();
        var secondLiteral = body[1].Accept(this.goalToLiteralConverter).GetValueOrThrow();
        var thirdLiteral = body[2].Accept(this.goalToLiteralConverter).GetValueOrThrow();

        Assert.That(
            body.Count == 3 &&
            firstLiteral.Identifier == "node" && !firstLiteral.HasNafNegation &&
            secondLiteral.Identifier == "node" && !secondLiteral.HasNafNegation &&
            thirdLiteral.HasNafNegation && thirdLiteral.Identifier == "edge");
        Assert.That(this.logger.Errors.Count == 0);
    }

    [Test]
    public void HandlesBodyTermsCorrectly()
    {
        var program = AspExtensions.GetProgram(this.graphCode, this.logger);

        var body = program.Statements[6].Body;

        Assert.That(body.Count == 3);
        Assert.That(AspExtensions.CompareGoal(body[0], false, false, "node",["X"]));
        Assert.That(AspExtensions.CompareGoal(body[1], false, false, "node",["Y"]));
        Assert.That(AspExtensions.CompareGoal(body[2], true, false, "edge",["X", "Y"]));
        Assert.That(this.logger.Errors.Count == 0);
    }
}