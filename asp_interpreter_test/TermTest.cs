//-----------------------------------------------------------------------
// <copyright file="TermTest.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_test;
using Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;

public class TermTest
{
    private readonly TermToNumberConverter visitor = new TermToNumberConverter();

    [Test]
    public void TermToNumberConverterFailsOnBasicTerm()
    {
        ITerm term = new BasicTerm("a",[]);
        var result = term.Accept(this.visitor);

        Assert.That(!result.HasValue);
    }

    [Test]
    public void TermToNumberConverterFailsOnAnonymousVariableTerm()
    {
        ITerm term = new AnonymousVariableTerm();
        var result = term.Accept(this.visitor);

        Assert.That(!result.HasValue);
    }

    [Test]
    public void TermToNumberConverterFailsOnVariableTerm()
    {
        ITerm term = new VariableTerm("a");

        Assert.That(!term.Accept(this.visitor).HasValue);
    }

    [Test]
    public void TermToNumberConverterSucceedsOnPlus()
    {
        // ITerm term = new ArithmeticOperationTerm(new Addition(new NumberTerm(1), new NumberTerm(1)));
        ITerm term = new ArithmeticOperationTerm(new NumberTerm(1), new Plus(), new NumberTerm(1));
        var result = term.Accept(this.visitor);

        Assert.That(result.HasValue && result.GetValueOrThrow() == 2);
    }

    [Test]
    public void TermToNumberConverterSucceedsOnMinus()
    {
        ITerm term = new ArithmeticOperationTerm(new NumberTerm(1), new Minus(), new NumberTerm(1));
        var result = term.Accept(this.visitor);

        Assert.That(result.HasValue && result.GetValueOrThrow() == 0);
    }

    [Test]
    public void TermToNumberConverterSucceedsOnTimes()
    {
        ITerm term = new ArithmeticOperationTerm(new NumberTerm(2), new Multiply(), new NumberTerm(2));
        var result = term.Accept(this.visitor);

        Assert.That(result.HasValue && result.GetValueOrThrow() == 4);
    }

    [Test]
    public void TermToNumberConverterSucceedsOnDivide()
    {
        ITerm term = new ArithmeticOperationTerm(new NumberTerm(2), new Divide(), new NumberTerm(2));
        var result = term.Accept(this.visitor);

        Assert.That(result.HasValue && result.GetValueOrThrow() == 1);
    }

    [Test]
    public void TermToNumberConverterSuccessOnParenthesizedTerm()
    {
        ITerm term = new ParenthesizedTerm(new NumberTerm(1));
        var result = term.Accept(this.visitor);

        Assert.That(result.HasValue && result.GetValueOrThrow() == 1);
    }

    [Test]
    public void TermToNumberConverterFailsOnStringTerm()
    {
        ITerm term = new StringTerm("a");
        var result = term.Accept(this.visitor);

        Assert.That(!result.HasValue);
    }

    [Test]
    public void TermToNumberConverterSucceedsOnNegatedTerm()
    {
        ITerm term = new NegatedTerm(new NumberTerm(1));
        var result = term.Accept(this.visitor);

        Assert.That(result.HasValue && result.GetValueOrThrow() == -1);
    }

    [Test]
    public void TermToNumberConverterSucceedsOnConventionalNumberTerm()
    {
        ITerm term = new NumberTerm(1);
        var result = term.Accept(this.visitor);

        Assert.That(result.HasValue && result.GetValueOrThrow() == 1);
    }
}