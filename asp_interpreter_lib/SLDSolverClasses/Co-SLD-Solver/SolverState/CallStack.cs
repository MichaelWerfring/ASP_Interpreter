// <copyright file="CallStack.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.Util;
using System.Collections;
using System.Collections.Immutable;

/// <summary>
/// A class representing a callstack of goals.
/// </summary>
public class CallStack : IImmutableStack<Structure>
{
    private readonly IImmutableStack<Structure> terms;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallStack"/> class.
    /// </summary>
    public CallStack()
    {
        this.terms = ImmutableStack.Create<Structure>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CallStack"/> class.
    /// </summary>
    /// <param name="termStack">An immutable stack of terms.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="termStack"/> is null.</exception>
    public CallStack(IImmutableStack<Structure> termStack)
    {
        ArgumentNullException.ThrowIfNull(termStack, nameof(termStack));

        this.terms = termStack;
    }

    /// <summary>
    /// Gets a value indicating whether the callstack is empty.
    /// </summary>
    public bool IsEmpty => this.terms.IsEmpty;

    /// <summary>
    /// Gets a value indicating whether the callstack is empty.
    /// </summary>
    bool IImmutableStack<Structure>.IsEmpty => this.IsEmpty;

    /// <summary>
    /// Clears the callstack.
    /// </summary>
    /// <returns>A cleared callstack.</returns>
    public CallStack Clear()
    {
        return new CallStack(this.terms.Clear());
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator for the callstack entries.</returns>
    public IEnumerator<Structure> GetEnumerator()
    {
        return this.terms.GetEnumerator();
    }

    /// <summary>
    /// Peeks the top of the stack.
    /// </summary>
    /// <returns>The element on the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown if stack is empty.</exception>
    public Structure Peek()
    {
        return this.terms.Peek();
    }

    /// <summary>
    /// Pops an element from the stack.
    /// </summary>
    /// <returns>A new callstack with the element popped, or the same callstack if it is already empty.</returns>
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

    /// <summary>
    /// Pushes a new entry to the top of the stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    /// <returns>A new callstack with the value on the top of the stack.</returns>
    /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
    public CallStack Push(Structure value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new CallStack(this.terms.Push(value));
    }

    /// <summary>
    /// Clears the callstack.
    /// </summary>
    /// <returns>A cleared callstack.</returns>
    IImmutableStack<Structure> IImmutableStack<Structure>.Clear()
    {
        return this.Clear();
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator for the callstack entries.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator for the callstack entries.</returns>
    IEnumerator<Structure> IEnumerable<Structure>.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    /// <summary>
    /// Peeks the top of the stack.
    /// </summary>
    /// <returns>The element on the top of the stack.</returns>
    /// <exception cref="InvalidOperationException">Thrown if stack is empty.</exception>
    Structure IImmutableStack<Structure>.Peek()
    {
        return this.Peek();
    }

    /// <summary>
    /// Pops an element from the stack.
    /// </summary>
    /// <returns>A new callstack with the element popped, or the same callstack if it is already empty.</returns>
    IImmutableStack<Structure> IImmutableStack<Structure>.Pop()
    {
        return this.Pop();
    }

    /// <summary>
    /// Pushes a new entry to the top of the stack.
    /// </summary>
    /// <param name="value">The value to push.</param>
    /// <returns>A new callstack with the value on the top of the stack.</returns>
    /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
    IImmutableStack<Structure> IImmutableStack<Structure>.Push(Structure value)
    {
        return this.Push(value);
    }

    /// <summary>
    /// Converts the callstack to a string representation.
    /// </summary>
    /// <returns>A string representation.</returns>
    public override string ToString()
    {
        return !this.terms.IsEmpty ? $"{{{this.terms.ToList().ListToString()}}}" : "Empty Callstack";
    }
}