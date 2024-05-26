// <copyright file="NegatedTermConverter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.FunctorNaming;

/// <summary>
/// A class for converting a negated term.
/// </summary>
public class NegatedTermConverter : TypeBaseVisitor<ISimpleTerm>
{
    private readonly TermConverter converter;
    private readonly FunctorTableRecord functorTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="NegatedTermConverter"/> class.
    /// </summary>
    /// <param name="converter">A term converter to convert the inner term of a negated term.</param>
    /// <param name="functorTable">The functor table to use for conversion.</param>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. converter is null.
    /// .. functorTable is null.</exception>
    public NegatedTermConverter(TermConverter converter, FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(converter, nameof(converter));
        ArgumentNullException.ThrowIfNull(functorTable, nameof(functorTable));

        this.converter = converter;
        this.functorTable = functorTable;
    }

    /// <summary>
    /// Converts a negated term.
    /// </summary>
    /// <param name="negatedTerm">The term to convert.</param>
    /// <returns>The converted term, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if negatedTerm is null.</exception>
    public IOption<ISimpleTerm> Convert(NegatedTerm negatedTerm)
    {
        ArgumentNullException.ThrowIfNull(negatedTerm);

        return negatedTerm.Term.Accept(this);
    }

    /// <summary>
    /// Visits an inner term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if negatedTerm is null.</exception>
    public override IOption<ISimpleTerm> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedStructure = this.converter.Convert(term);

        return new Some<ISimpleTerm>(new Structure(this.functorTable.ClassicalNegation, [convertedStructure]));
    }

    /// <summary>
    /// Visits an inner term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if negatedTerm is null.</exception>
    public override IOption<ISimpleTerm> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var convertedTerm = this.converter.Convert(term.Term);

        return new Some<ISimpleTerm>(convertedTerm);
    }

    /// <summary>
    /// Visits an inner term to convert it.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The converted term, or none in case of failure.</returns>
    /// <exception cref="ArgumentNullException">Thrown if negatedTerm is null.</exception>
    public override IOption<ISimpleTerm> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var negatedInteger = new Integer(term.Value * -1);

        return new Some<ISimpleTerm>(negatedInteger);
    }

    // unconvertible terms : these should not be inside a negated term

    /// <summary>
    /// Visits an inner term to convert it. Failure case.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>Always none. </returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(AnonymousVariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    /// <summary>
    /// Visits an inner term to convert it. Failure case.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>Always none. </returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    /// <summary>
    /// Visits an inner term to convert it. Failure case.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>Always none. </returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(ConventionalList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    /// <summary>
    /// Visits an inner term to convert it. Failure case.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>Always none. </returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    /// <summary>
    /// Visits an inner term to convert it. Failure case.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>Always none. </returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(RecursiveList term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    /// <summary>
    /// Visits an inner term to convert it. Failure case.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>Always none. </returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }

    /// <summary>
    /// Visits an inner term to convert it. Failure case.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>Always none. </returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public override IOption<ISimpleTerm> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<ISimpleTerm>();
    }
}