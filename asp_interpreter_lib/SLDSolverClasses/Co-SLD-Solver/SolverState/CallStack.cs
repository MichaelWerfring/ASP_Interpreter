using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util;
using System.Collections;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

public class CallStack : IImmutableStack<ISimpleTerm>
{
    private readonly IImmutableStack<ISimpleTerm> _terms;

    public CallStack()
    {
        _terms = [];
    }

    public CallStack(IImmutableStack<ISimpleTerm> termStack)
    {
        ArgumentNullException.ThrowIfNull(termStack, nameof(termStack));

        _terms = termStack;
    }

    public bool IsEmpty => _terms.IsEmpty;

    public CallStack Clear()
    {
        return new CallStack(_terms.Clear());
    }

    public IEnumerator<ISimpleTerm> GetEnumerator()
    {
        return _terms.GetEnumerator();
    }

    public ISimpleTerm Peek()
    {
        return _terms.Peek();
    }

    public CallStack Pop()
    {
        if (_terms.IsEmpty)
        {
            return this;
        }
        else
        {
            return new CallStack(_terms.Pop());
        }       
    }

    public CallStack Push(ISimpleTerm value)
    {
        return new CallStack(_terms.Push(value));
    }

    bool IImmutableStack<ISimpleTerm>.IsEmpty => IsEmpty;

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

    public override string ToString()
    {
        return _terms.ToList().ListToString();
    }
}
