// <copyright file="IVariableBindingVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;

public interface IVariableBindingVisitor
{
    void Visit(ProhibitedValuesBinding binding);

    void Visit(TermBinding binding);
}

public interface IVariableBindingVisitor<T>
{
    T Visit(ProhibitedValuesBinding binding);

    T Visit(TermBinding binding);
}

public interface IVariableBindingArgumentVisitor<TArgs>
{
    void Visit(ProhibitedValuesBinding binding, TArgs args);

    void Visit(TermBinding binding, TArgs args);
}

public interface IVariableBindingArgumentVisitor<TResult, TArgs>
{
    TResult Visit(ProhibitedValuesBinding binding, TArgs args);

    TResult Visit(TermBinding binding, TArgs args);
}