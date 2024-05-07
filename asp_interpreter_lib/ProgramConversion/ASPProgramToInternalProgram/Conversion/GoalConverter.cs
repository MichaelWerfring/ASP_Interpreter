﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using System.Collections.Immutable;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;

public class GoalConverter : TypeBaseVisitor<ISimpleTerm>
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
                                            children.ToImmutableList()
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

    public override IOption<ISimpleTerm> Visit(BinaryOperation binOp)
    {
        var leftMaybe = binOp.Left.Accept(_termConverter);
        if (!leftMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var rightMaybe = binOp.Right.Accept(_termConverter);
        if (!rightMaybe.HasValue) { return new None<ISimpleTerm>(); }

        var functor = _operatorConverter.Convert(binOp.BinaryOperator);

        var convertedStructure = new Structure(functor, [leftMaybe.GetValueOrThrow(), rightMaybe.GetValueOrThrow()]);

        return new Some<ISimpleTerm>(convertedStructure);       
    }
}
