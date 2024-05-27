//-----------------------------------------------------------------------
// <copyright file="TermCopyVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;

public class TermCopyVisitor : TypeBaseVisitor<ITerm>
{
    /// <inheritdoc/>
    public override IOption<ITerm> Visit(AnonymousVariableTerm term)
    {
        return new Some<ITerm>(new AnonymousVariableTerm());
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(VariableTerm term)
    {
        return new Some<ITerm>(new VariableTerm(term.Identifier.GetCopy()));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(ArithmeticOperationTerm term)
    {
        return new Some<ITerm>(new ArithmeticOperationTerm(term.Left, term.Operation, term.Right));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(BasicTerm term)
    {
        List<ITerm> children =[];

        term.Terms.ForEach(t =>
        {
            children.Add(t.Accept(this).GetValueOrThrow(string.Empty +
                "The child term cannot be parsed!"));
        });

        return new Some<ITerm>(new BasicTerm(term.Identifier.ToString(), children));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(StringTerm term)
    {
        return new Some<ITerm>(new StringTerm(term.Value.GetCopy()));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(NumberTerm term)
    {
        return new Some<ITerm>(new NumberTerm(term.Value));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(NegatedTerm term)
    {
        var innerTerm = term.Term.Accept(this);
        return new Some<ITerm>(new NegatedTerm(
            innerTerm.GetValueOrThrow("The inner term cannot be copied!")));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(ParenthesizedTerm term)
    {
        var innerTerm = term.Term.Accept(this);
        return new Some<ITerm>(new ParenthesizedTerm(
            innerTerm.GetValueOrThrow("The inner term cannot be copied!")));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(RecursiveList term)
    {
        ITerm head = term.Head.Accept(this).
            GetValueOrThrow("The left term cannot be copied!");

        ITerm tail = term.Tail.Accept(this).
            GetValueOrThrow("The right term cannot be copied!");

        return new Some<ITerm>(new RecursiveList(head, tail));
    }

    /// <inheritdoc/>
    public override IOption<ITerm> Visit(ConventionalList term)
    {
        List<ITerm> children =[];

        term.Terms.ForEach(t =>
        {
            children.Add(t.Accept(this).GetValueOrThrow(string.Empty +
                                                        "The child term cannot be parsed!"));
        });

        return new Some<ITerm>(new ConventionalList(children));
    }
}