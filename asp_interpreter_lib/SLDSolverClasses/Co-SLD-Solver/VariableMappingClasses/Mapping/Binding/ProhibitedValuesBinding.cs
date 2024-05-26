﻿// <copyright file="ProhibitedValuesBinding.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.Util;
using System.Collections.Immutable;

public class ProhibitedValuesBinding : IVariableBinding
{
    public ProhibitedValuesBinding()
    {
        this.ProhibitedValues = ImmutableSortedSet.Create<ISimpleTerm>(TermFuncs.GetSingletonTermComparer());
    }

    public ProhibitedValuesBinding(ImmutableSortedSet<ISimpleTerm> prohibitedValuesSet)
    {
        ArgumentNullException.ThrowIfNull(prohibitedValuesSet, nameof(prohibitedValuesSet));
        if (prohibitedValuesSet.KeyComparer is not SimpleTermComparer)
        {
            throw new ArgumentException("Must contain correct comparer.");
        }

        this.ProhibitedValues = prohibitedValuesSet;
    }

    public ImmutableSortedSet<ISimpleTerm> ProhibitedValues { get; }

    public void Accept(IVariableBindingVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IVariableBindingVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }

    public void Accept<TArgs>(IVariableBindingArgumentVisitor<TArgs> visitor, TArgs arguments)
    {
        visitor.Visit(this, arguments);
    }

    public TResult Accept<TResult, TArgs>(IVariableBindingArgumentVisitor<TResult, TArgs> visitor, TArgs arguments)
    {
        return visitor.Visit(this, arguments);
    }

    public override string ToString()
    {
        return $"ProhibitedValues:{{{this.ProhibitedValues.ToList().ListToString()}}}";
    }
}