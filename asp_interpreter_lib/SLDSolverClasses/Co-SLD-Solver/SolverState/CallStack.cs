using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CallStack
{
    public CallStack(IImmutableStack<ISimpleTerm> termStack)
    {
        ArgumentNullException.ThrowIfNull(termStack, nameof(termStack));

        TermStack = termStack;
    }

    public IImmutableStack<ISimpleTerm> TermStack { get; }

    public override string ToString()
    {
        return $"{{ {TermStack.ToList().ToList()} }}";
    }
}
