﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;

public class List : IStructure
{
    public List(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Left = left;
        Right = right;
    }

    public ISimpleTerm Left { get; }
    public ISimpleTerm Right { get; }

    public void Accept(ISimpleTermVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(ISimpleTermVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(ISimpleTermArgsVisitor<TArgs> visitor, TArgs arguments)
    {
        visitor.Visit(this, arguments);
    }

    public TResult Accept<TResult, TArgs>(ISimpleTermArgsVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        return visitor.Visit(this, arguments);
    }

    public override string ToString()
    {
        return $"[{Left} | {Right}]";
    }
}