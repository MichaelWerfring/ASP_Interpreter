using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Mapping;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class GoalConverter : TypeBaseVisitor<ISimpleTerm>
{
    private FunctorTableRecord _functorTable;

    private TermConverter _termConverter;

    private OperatorConverter _operatorConverter;

    public GoalConverter(FunctorTableRecord functorTable)
    {
        ArgumentNullException.ThrowIfNull(functorTable);

        _functorTable = functorTable;
        _termConverter = new TermConverter(functorTable);
        _operatorConverter = new OperatorConverter(functorTable);
    }

    public IOption<ISimpleTerm> Convert(Goal goal)
    {
        ArgumentNullException.ThrowIfNull(goal, nameof(goal));

        return goal.Accept(this);
    }

    public override IOption<ISimpleTerm> Visit(Forall goal)
    {
        var leftMaybe = goal.VariableTerm.Accept(_termConverter);
        if(!leftMaybe.HasValue)
        {
            return new None<ISimpleTerm>();
        }

        var rightMaybe = goal.Goal.Accept(this);
        if (!rightMaybe.HasValue)
        {
            return new None<ISimpleTerm>();
        }

        var convertedStruct = new Structure(_functorTable.Forall, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<ISimpleTerm>(convertedStruct);
    }

    public override IOption<ISimpleTerm> Visit(Literal goal)
    {
        var children = goal.Terms.Select(_termConverter.Convert);

        var convertedTerm = new Structure(
                                            goal.Identifier.GetCopy(),
                                            children.ToList()
                                         );

        if (goal.HasStrongNegation)
        {
            convertedTerm = new Structure(_functorTable.ClassicalNegation, [convertedTerm]);
        }

        if (goal.HasNafNegation)
        {
            convertedTerm = new Structure(_functorTable.NegationAsFailure, [convertedTerm]);
        }

        return new Some<ISimpleTerm>(convertedTerm);
    }

    public override IOption<ISimpleTerm> Visit(BinaryOperation goal)
    {
        var leftMaybe = goal.Left.Accept(_termConverter);
        if (!leftMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var rightMaybe = goal.Right.Accept(_termConverter);
        if (!rightMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var functor = _operatorConverter.Convert(goal.BinaryOperator);

        var convertedStructure = new Structure(functor, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<ISimpleTerm>(convertedStructure);       
    }
}
