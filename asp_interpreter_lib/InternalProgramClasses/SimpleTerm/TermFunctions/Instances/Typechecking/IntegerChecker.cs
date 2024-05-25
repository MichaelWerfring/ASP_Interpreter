using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;

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
