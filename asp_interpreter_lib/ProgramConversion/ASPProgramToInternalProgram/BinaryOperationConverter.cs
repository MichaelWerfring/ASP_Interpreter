using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class BinaryOperationConverter : TypeBaseVisitor<ISimpleTerm>
{
    private TermConverter _converter;
    private BinaryOperation _operation;

    public BinaryOperationConverter(TermConverter termConverter, BinaryOperation binaryOperation)
    {
        ArgumentNullException.ThrowIfNull(termConverter);
        ArgumentNullException.ThrowIfNull(binaryOperation);

        _converter = termConverter;
        _operation = binaryOperation;
    }

    public ISimpleTerm Convert()
    {
        return _operation.BinaryOperator.Accept(this).GetValueOrThrow();
    }

    public override IOption<ISimpleTerm> Visit(Disunification _)
    {
        var left = _converter.Convert(_operation.Left);
        var right = _converter.Convert(_operation.Right);

        return new Some<ISimpleTerm>(new DisunificationStructure(left, right));
    }

    public override IOption<ISimpleTerm> Visit(Equality _)
    {
        var left = _converter.Convert(_operation.Left);
        var right = _converter.Convert(_operation.Right);

        return new Some<ISimpleTerm>(new UnificationStructure(left, right));
    }

    public override IOption<ISimpleTerm> Visit(GreaterOrEqualThan _)
    {
        var left = _converter.Convert(_operation.Left);
        var right = _converter.Convert(_operation.Right);

        return new Some<ISimpleTerm>(new GreaterThanOrEqual(left, right));
    }

    public override IOption<ISimpleTerm> Visit(Types.BinaryOperations.GreaterThan _)
    {
        var left = _converter.Convert(_operation.Left);
        var right = _converter.Convert(_operation.Right);

        return new Some<ISimpleTerm>(new InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison.GreaterThan(left, right));
    }

    public override IOption<ISimpleTerm> Visit(Types.BinaryOperations.LessOrEqualThan _)
    {
        var left = _converter.Convert(_operation.Left);
        var right = _converter.Convert(_operation.Right);

        return new Some<ISimpleTerm>(new InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison.LessOrEqualThan(left, right));
    }

    public override IOption<ISimpleTerm> Visit(Types.BinaryOperations.LessThan _)
    {
        var left = _converter.Convert(_operation.Left);
        var right = _converter.Convert(_operation.Right);

        return new Some<ISimpleTerm>(new InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison.LessThan(left, right));
    }
    public override IOption<ISimpleTerm> Visit(Is _)
    {
        var left = _converter.Convert(_operation.Left);
        var right = _converter.Convert(_operation.Right);

        return new Some<ISimpleTerm>(new Evaluation(left, right));
    }
}