// <copyright file="OperatorConverter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

using Asp_interpreter_lib.FunctorNaming;
using Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A class for converting an operator to its string representation.
/// </summary>
public class OperatorConverter : TypeBaseVisitor<string>
{
    private readonly FunctorTableRecord functorTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorConverter"/> class.
    /// </summary>
    /// <param name="functorTable">The functor table to use for conversion.</param>
    /// <exception cref="ArgumentNullException">Thrown if functorTable is null.</exception>
    public OperatorConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable, nameof(functorTable));

        this.functorTable = functorTable;
    }

    /// <summary>
    /// Converts an arithmetic operation.
    /// </summary>
    /// <param name="arithmeticOperation">The operation to convert.</param>
    /// <returns>A string rrepresentation of the operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if arithmeticOperation is null.</exception>
    public string Convert(ArithmeticOperation arithmeticOperation)
    {
        ArgumentNullException.ThrowIfNull(arithmeticOperation);

        return arithmeticOperation.Accept(this).GetValueOrThrow();
    }

    /// <summary>
    /// Converts a binary operator.
    /// </summary>
    /// <param name="binaryOperator">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if binaryOperator is null.</exception>
    public string Convert(BinaryOperator binaryOperator)
    {
        ArgumentNullException.ThrowIfNull(binaryOperator);

        return binaryOperator.Accept(this).GetValueOrThrow();
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Divide op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.Division);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Plus op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.Addition);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Minus op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.Subtraction);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Multiply op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.Multiplication);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Disunification op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.Disunification);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Equality op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.Unification);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(GreaterOrEqualThan op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.GreaterOrEqualThan);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(GreaterThan op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.GreaterThan);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Is op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.ArithmeticEvaluation);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(IsNot op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.ArithmeticEvaluationNegated);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(LessOrEqualThan op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.LessOrEqualThan);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(LessThan op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.LessThan);
    }

    /// <summary>
    /// Converts an operator.
    /// </summary>
    /// <param name="op">The operator to convert.</param>
    /// <returns>A string rrepresentation of the operator.</returns>
    /// <exception cref="ArgumentNullException">Thrown if op is null.</exception>
    public override IOption<string> Visit(Power op)
    {
        ArgumentNullException.ThrowIfNull(op);

        return new Some<string>(this.functorTable.Power);
    }
}