﻿namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

public interface ISimpleTermVisitor
{
    void Visit(Variable variableTerm);
    void Visit(Structure basicTerm);
    void Visit(Integer integer);
}

public interface ISimpleTermVisitor<T>
{
    T Visit(Variable variableTerm);
    T Visit(Structure basicTerm);
    T Visit(Integer integer);
}

public interface ISimpleTermArgsVisitor<TArgs>
{
    void Visit(Variable variableTerm, TArgs arguments);
    void Visit(Structure basicTerm, TArgs arguments);
    void Visit(Integer integer, TArgs arguments);
}

public interface ISimpleTermArgsVisitor<TResult, TArgs>
{
    TResult Visit(Variable variableTerm, TArgs arguments);
    TResult Visit(Structure basicTerm, TArgs arguments);
    TResult Visit(Integer integer, TArgs arguments);
}