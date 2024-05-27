//-----------------------------------------------------------------------
// <copyright file="TermVisitorTest.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_test;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;

public class TermVisitorTest
{
    private readonly ILogger errorLogger = new ConsoleLogger(LogLevel.Error);

    [Test]
    public void ParseVariableTerm()
    {
        string code = "a(X). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["X"]));
    }

    [Test]
    public void ParseStringTerm()
    {
        string code = "a(\"hallo\"). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["\"hallo\""]));
    }

    [Test]
    public void ParseBasicTerm()
    {
        string code = "a(b, c). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["b", "c"]));
    }

    [Test]
    public void ParseNegatedTerm()
    {
        string code = "a(-1). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);
        var converter = new TermToNumberConverter();

        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[0];
        var content = term?.Accept(converter);

        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == -1);
    }

    [Test]
    public void ParseBasicTermWithInnerTerms()
    {
        string code = "a(b, c(d, e)). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["b", "c(d, e)"]));
    }

    [Test]
    public void ParseParenthesizedTerm()
    {
        string code = "a((b)). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["b"]));
    }

    [Test]
    public void ParseParenthesizedTermWithMultipleInnerTerms()
    {
        string code = "a(b,(c(d, e, f, g))). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["b", "c(d, e, f, g)"]));
    }

    [Test]
    public void ParseAnonymusVariableTerm()
    {
        string code = "a(_). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["_"]));
    }

    [Test]
    public void ParseAnonymusVariableTermWithSeveralArguments()
    {
        string code = "a(b, _). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["b", "_"]));
    }

    [Test]
    public void ParseAnonymusVariableTermWithInnerTerms()
    {
        string code = "a(b, c(d, _)). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);

        var literal = program.Statements[0].Head.GetValueOrThrow();

        Assert.That(AspExtensions.CompareGoal(literal, false, false, "a",["b", "c(d, _)"]));
    }

    [Test]
    public void ParseNumberTerm()
    {
        string code = "a(1). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);
        var converter = new TermToNumberConverter();

        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[0];
        var content = term?.Accept(converter);

        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == 1);
    }

    [Test]
    public void ParseNumberTermWithSeveralArguments()
    {
        string code = "a(1, 2, 3, 4, 5). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);
        var converter = new TermToNumberConverter();

        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[1];
        var content = term?.Accept(converter);

        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == 2);
    }

    [Test]
    public void ParseNumberTermWithInnerTerms()
    {
        string code = "a(1, 2, 3, 7). a?";
        var program = AspExtensions.GetProgram(code, this.errorLogger);
        var converter = new TermToNumberConverter();

        var literal = program.Statements[0].Head.GetValueOrThrow();
        var term = literal?.Terms[2];
        var content = term?.Accept(converter);

        Assert.That(content != null &&
            content.HasValue && content.GetValueOrThrow() == 3);
    }
}