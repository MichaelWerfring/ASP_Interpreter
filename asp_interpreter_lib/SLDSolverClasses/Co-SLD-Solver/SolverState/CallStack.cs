using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using System.Collections;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CallStack : IImmutableStack<ISimpleTerm>
{
    public CallStack()
    {
        TermStack = [];
    }

    public CallStack(IImmutableStack<ISimpleTerm> termStack)
    {
        ArgumentNullException.ThrowIfNull(termStack, nameof(termStack));

        TermStack = termStack;
    }

    public IImmutableStack<ISimpleTerm> TermStack { get; }

    public bool IsEmpty => TermStack.IsEmpty;

    bool IImmutableStack<ISimpleTerm>.IsEmpty => throw new NotImplementedException();

    public CallStack Clear()
    {
        return new CallStack(TermStack.Clear());
    }

    public IEnumerator<ISimpleTerm> GetEnumerator()
    {
        return TermStack.GetEnumerator();
    }

    public ISimpleTerm Peek()
    {
        return TermStack.Peek();
    }

    public CallStack Pop()
    {
        if (TermStack.IsEmpty)
        {
            return this;
        }
        else
        {
            return new CallStack(TermStack.Pop());
        }       
    }

    public CallStack Push(ISimpleTerm value)
    {
        return new CallStack(TermStack.Push(value));
    }

    IImmutableStack<ISimpleTerm> IImmutableStack<ISimpleTerm>.Clear()
    {
        return Clear();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<ISimpleTerm> IEnumerable<ISimpleTerm>.GetEnumerator()
    {
        return GetEnumerator();
    }

    ISimpleTerm IImmutableStack<ISimpleTerm>.Peek()
    {
        return Peek();
    }

    IImmutableStack<ISimpleTerm> IImmutableStack<ISimpleTerm>.Pop()
    {
        return Pop();
    }

    IImmutableStack<ISimpleTerm> IImmutableStack<ISimpleTerm>.Push(ISimpleTerm value)
    {
        return Push(value);
    }
}
