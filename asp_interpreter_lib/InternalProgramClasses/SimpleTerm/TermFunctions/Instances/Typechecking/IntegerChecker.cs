using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;

internal class IntegerChecker : ISimpleTermVisitor<IOption<Integer>>
{
    public IOption<Integer> ReturnIntegerOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<Integer> Visit(Variable term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Integer>();
    }

    public IOption<Integer> Visit(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Integer>();
    }

    public IOption<Integer> Visit(Integer term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<Integer>(term);
    }
}
