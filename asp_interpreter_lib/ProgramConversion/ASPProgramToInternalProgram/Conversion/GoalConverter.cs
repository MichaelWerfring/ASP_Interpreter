using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.FunctorNaming;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class GoalConverter : TypeBaseVisitor<Structure>
{
    private readonly FunctorTableRecord _functorTable;

    private readonly TermConverter _termConverter;

    private readonly OperatorConverter _operatorConverter;

    public GoalConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _functorTable = functorTable;
        _termConverter = new TermConverter(functorTable);
        _operatorConverter = new OperatorConverter(functorTable);
    }

    public IOption<Structure> Convert(Goal goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        return goal.Accept(this);
    }

    public override IOption<Structure> Visit(Forall goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        var leftMaybe = goal.VariableTerm.Accept(_termConverter);
        if(!leftMaybe.HasValue)
        {
            return new None<Structure>();
        }

        var rightMaybe = goal.Goal.Accept(this);
        if (!rightMaybe.HasValue)
        {
            return new None<Structure>();
        }

        var convertedStruct = new Structure
            (_functorTable.Forall, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<Structure>(convertedStruct);
    }

    public override IOption<Structure> Visit(Literal goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        var children = goal.Terms.Select(_termConverter.Convert).ToArray();

        var convertedTerm = new Structure(goal.Identifier.GetCopy(),children);

        if (goal.HasStrongNegation)
        {
            convertedTerm = new Structure(_functorTable.ClassicalNegation, [convertedTerm]);
        }

        if (goal.HasNafNegation)
        {
            convertedTerm = new Structure(_functorTable.NegationAsFailure, [convertedTerm]);
        }

        return new Some<Structure>(convertedTerm);
    }

    public override IOption<Structure> Visit(BinaryOperation goal)
    {
        ArgumentNullException.ThrowIfNull(goal);

        var leftMaybe = goal.Left.Accept(_termConverter);

        var rightMaybe = goal.Right.Accept(_termConverter);

        var functor = _operatorConverter.Convert(goal.BinaryOperator);

        var convertedStructure = new Structure
            (functor, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<Structure>(convertedStructure);       
    }
}
