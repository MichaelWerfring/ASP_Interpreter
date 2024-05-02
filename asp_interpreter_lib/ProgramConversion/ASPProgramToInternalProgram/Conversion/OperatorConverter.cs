using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class OperatorConverter : TypeBaseVisitor<string>
{
    private FunctorTableRecord _functorTable;

    public OperatorConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable, nameof(functorTable));

        _functorTable = functorTable;
    }

    public string Convert(ArithmeticOperation arithmeticOperation)
    {
        ArgumentNullException.ThrowIfNull(arithmeticOperation);

        return arithmeticOperation.Accept(this).GetValueOrThrow();
    }

    public string Convert(BinaryOperator binaryOperator)
    {
        ArgumentNullException.ThrowIfNull(binaryOperator);

        return binaryOperator.Accept(this).GetValueOrThrow();
    }

    public override IOption<string> Visit(Divide op)
    {
        return new Some<string>(_functorTable.Division);
    }

    public override IOption<string> Visit(Plus op)
    {
        return new Some<string>(_functorTable.Addition);
    }

    public override IOption<string> Visit(Minus op)
    {
        return new Some<string>(_functorTable.Subtraction);
    }

    public override IOption<string> Visit(Multiply op)
    {
        return new Some<string>(_functorTable.Multiplication);
    }

    public override IOption<string> Visit(Disunification op)
    {
        return new Some<string>(_functorTable.Disunification);
    }

    public override IOption<string> Visit(Equality op)
    {
        return new Some<string>(_functorTable.Unification);
    }

    public override IOption<string> Visit(GreaterOrEqualThan op)
    {
        return new Some<string>(_functorTable.GreaterOrEqualThan);
    }

    public override IOption<string> Visit(GreaterThan op)
    {
        return new Some<string>(_functorTable.GreaterThan);
    }

    public override IOption<string> Visit(Is op)
    {
        return new Some<string>(_functorTable.ArithmeticEvaluation);
    }

    public override IOption<string> Visit(LessOrEqualThan op)
    {
        return new Some<string>(_functorTable.LessOrEqualThan);
    }

    public override IOption<string> Visit(LessThan op)
    {
        return new Some<string>(_functorTable.LessThan);
    }

    public override IOption<string> Visit(Power op)
    {
        return new Some<string>(_functorTable.Power);
    }
}
