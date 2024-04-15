using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class NafLiteralConverter : TypeBaseVisitor<ISimpleTerm>
{
    private ClassicalLiteralConverter _classicalLiteralConverter = new ClassicalLiteralConverter();
    private TermConverter _termConverter = new TermConverter();

    public ISimpleTerm Convert(NafLiteral nafLiteral)
    {
        ArgumentNullException.ThrowIfNull(nafLiteral);

        var result = nafLiteral.Accept(this);
        if(!result.HasValue)
        {
            throw new ArgumentException("Could not convert literal");
        }

        return result.GetValueOrThrow();
    }

    public override IOption<ISimpleTerm> Visit(NafLiteral nafLiteral)
    {
        ArgumentNullException.ThrowIfNull(nafLiteral);

        ISimpleTerm term;
        if (nafLiteral.IsClassicalLiteral)
        {
            term = _classicalLiteralConverter.Convert(nafLiteral.ClassicalLiteral);
        }
        else
        {

            var converter = new BinaryOperationConverter(_termConverter, nafLiteral.BinaryOperation);
            term = converter.Convert();
        }

        if (nafLiteral.IsNafNegated)
        {
            term = new Naf(term);
        }

        return new Some<ISimpleTerm>(term);
    }

    public override IOption<ISimpleTerm> Visit(Forall forall)
    {
        ArgumentNullException.ThrowIfNull(forall);

        var convertedVariable = _termConverter.Convert(forall.VariableTerm);

        var convertedGoalMaybe = forall.Goal.Accept(this);
        if (!convertedGoalMaybe.HasValue) { return new None<ISimpleTerm>(); }

        return new Some<ISimpleTerm>
        (
            new ForAll(convertedVariable, convertedGoalMaybe.GetValueOrThrow())
        );
    }
}

