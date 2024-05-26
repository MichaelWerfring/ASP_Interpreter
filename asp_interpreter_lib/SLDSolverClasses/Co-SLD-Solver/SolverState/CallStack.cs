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
    private readonly IImmutableStack<Structure> terms;

    public CallStack()
    {
        this.terms = [];
    }

    public CallStack(IImmutableStack<Structure> termStack)
    {
        ArgumentNullException.ThrowIfNull(termStack, nameof(termStack));

        this.terms = termStack;
    }

    public bool IsEmpty => this.terms.IsEmpty;

    public CallStack Clear()
    {
        return new CallStack(this.terms.Clear());
    }

    public IEnumerator<Structure> GetEnumerator()
    {
        return this.terms.GetEnumerator();
    }

    public Structure Peek()
    {
        return this.terms.Peek();
    }

    public CallStack Pop()
    {
        if (this.terms.IsEmpty)
        {
            return this;
        }
        else
        {
            return new CallStack(this.terms.Pop());
        }
    }

    public CallStack Push(Structure value)
    {
        return new CallStack(this.terms.Push(value));
    }

    bool IImmutableStack<Structure>.IsEmpty => this.IsEmpty;

    IImmutableStack<Structure> IImmutableStack<Structure>.Clear()
    {
        return this.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    IEnumerator<Structure> IEnumerable<Structure>.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    Structure IImmutableStack<Structure>.Peek()
    {
        return this.Peek();
    }

    IImmutableStack<Structure> IImmutableStack<Structure>.Pop()
    {
        return this.Pop();
    }

    IImmutableStack<Structure> IImmutableStack<Structure>.Push(Structure value)
    {
        return this.Push(value);
    }

    public override string ToString()
    {
        return !this.terms.IsEmpty ? $"{{{this.terms.ToList().ListToString()}}}" : "Empty Callstack";
    }
}