// <copyright file="TermConverter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for converting terms.
/// </summary>
public class TermConverter : TypeBaseVisitor<ISimpleTerm>
{
    private readonly FunctorTableRecord functorTable;

    private readonly OperatorConverter operatorConverter;

    private readonly NegatedTermConverter negatedTermConverter;

    private int nextAnonymousVariableIndex = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="TermConverter"/> class.
    /// </summary>
    /// <param name="functorTable">The table of functors to use for conversion.</param>
    /// <exception cref="ArgumentNullException">Thrown if functorTable is null.</exception>
    public TermConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable, nameof(functorTable));

        this.functorTable = functorTable;
        this.operatorConverter = new OperatorConverter(functorTable);
        this.negatedTermConverter = new NegatedTermConverter(this, functorTable);
    }

    /// <summary>
    /// Converts a term.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public ISimpleTerm Convert(ITerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this).GetValueOrThrow();
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(AnonymousVariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var variable = new Variable($"{this.functorTable.AnonymousVariable}{this.nextAnonymousVariableIndex}");
        this.nextAnonymousVariableIndex += 1;

        return new Some<ISimpleTerm>(variable);
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var leftMaybe = term.Left.Accept(this);

        var rightMaybe = term.Right.Accept(this);

        var functor = this.operatorConverter.Convert(term.Operation);

        var children = new ISimpleTerm[] { leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow() };

        var structure = new Structure(functor, children);

        return new Some<ISimpleTerm>(structure);
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var newChildrenMaybes = term.Terms.Select((term) => term.Accept(this));
        if (newChildrenMaybes.Any((maybe) => !maybe.HasValue))
        {
            return new None<ISimpleTerm>();
        }

        var newChildren = newChildrenMaybes.Select(x => x.GetValueOrThrow()).ToArray();

        return new Some<ISimpleTerm>(new Structure(term.Identifier, newChildren));
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(ConventionalList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(this.ConvertConventionalList(term.Terms));
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedTermMaybe = this.negatedTermConverter.Convert(term);

        return convertedTermMaybe;
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Integer(term.Value));
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var inner = term.Term.Accept(this);
        try
        {
            return new Some<ISimpleTerm>(new Structure(this.functorTable.Parenthesis, [inner.GetValueOrThrow()]));
        }
        catch
        {
            return new None<ISimpleTerm>();
        }
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(RecursiveList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedList = this.ConvertRecursiveList(term);

        return new Some<ISimpleTerm>(convertedList);
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(this.ConvertString(term.Value));
    }

    /// <summary>
    /// Visits a term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<ISimpleTerm>(new Variable(term.Identifier));
    }

    private Structure ConvertString(string str)
    {
        if (str.Equals(string.Empty))
        {
            return new Structure(this.functorTable.Nil, []);
        }

        var head = new Structure(str.First().ToString(), []);
        var tail = this.ConvertString(new string(str.Skip(1).ToArray()));

        return new Structure(this.functorTable.List, [head, tail]);
    }

    private Structure ConvertRecursiveList(RecursiveList term)
    {
        var leftMaybe = term.Head.Accept(this);

        var rightMaybe = term.Tail.Accept(this);

        return new Structure(this.functorTable.List, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);
    }

    private Structure ConvertConventionalList(IEnumerable<ITerm> terms)
    {
        if (!terms.Any())
        {
            return new Structure(this.functorTable.Nil, []);
        }

        var headMaybe = terms.ElementAt(0).Accept(this);

        return new Structure(this.functorTable.List, [headMaybe.GetValueOrThrow(), this.ConvertConventionalList(terms.Skip(1))]);
    }
}