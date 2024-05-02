using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CallStack
{
    public CallStack(IImmutableStack<ISimpleTerm> termStack)
    {
        ArgumentNullException.ThrowIfNull(termStack, nameof(termStack));

        TermStack = termStack;
    }

    public IImmutableStack<ISimpleTerm> TermStack { get; }
}
