// <copyright file="CallStack.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Util;
using System.Collections;
using System.Collections.Immutable;

public class CallStack : IImmutableStack<Structure>
{
    private readonly IImmutableStack<Structure> _terms;

    public CallStack()
    {
        _terms = [];
    }

    public CallStack(IImmutableStack<Structure> termStack)
    {
        ArgumentNullException.ThrowIfNull(termStack, nameof(termStack));

        _terms = termStack;
    }

    public bool IsEmpty => _terms.IsEmpty;

    public CallStack Clear()
    {
        return new CallStack(_terms.Clear());
    }

    public IEnumerator<Structure> GetEnumerator()
    {
        return _terms.GetEnumerator();
    }

    public Structure Peek()
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

    public CallStack Push(Structure value)
    {
        return new CallStack(_terms.Push(value));
    }

    bool IImmutableStack<Structure>.IsEmpty => IsEmpty;

    IImmutableStack<Structure> IImmutableStack<Structure>.Clear()
    {
        return Clear();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<Structure> IEnumerable<Structure>.GetEnumerator()
    {
        return GetEnumerator();
    }

    Structure IImmutableStack<Structure>.Peek()
    {
        return Peek();
    }

    IImmutableStack<Structure> IImmutableStack<Structure>.Pop()
    {
        return Pop();
    }

    IImmutableStack<Structure> IImmutableStack<Structure>.Push(Structure value)
    {
        return Push(value);
    }

    public override string ToString()
    {
        return !_terms.IsEmpty ? $"{{{_terms.ToList().ListToString()}}}" : "Empty Callstack";
    }
}
