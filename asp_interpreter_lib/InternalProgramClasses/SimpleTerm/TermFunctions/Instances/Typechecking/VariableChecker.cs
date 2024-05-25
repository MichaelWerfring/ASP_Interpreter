using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;

internal class VariableChecker : ISimpleTermVisitor<IOption<Variable>>
{
    public IOption<Variable> ReturnVariableOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<Variable> Visit(Variable term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<Variable>(term);
    }

    public IOption<Variable> Visit(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Variable>();
    }

    public IOption<Variable> Visit(Integer term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Variable>();
    }
}
