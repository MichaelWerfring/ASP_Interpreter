﻿using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class TypeBaseVisitor<T> 
{
    public virtual IOption<T> Visit(AspProgram _) => new None<T>();
    public virtual IOption<T> Visit(Query _) => new None<T>();
    
    public virtual IOption<T> Visit(Statement _) => new None<T>();
    
    public virtual IOption<T> Visit(Head _) => new None<T>();

    public virtual IOption<T> Visit(Body _) => new None<T>();
    
    public virtual IOption<T> Visit(NafLiteral _) => new None<T>();
    
    public virtual IOption<T> Visit(ClassicalLiteral _) => new None<T>();
    
    public virtual IOption<T> Visit(Plus _) => new None<T>();
    
    public virtual IOption<T> Visit(Minus _) => new None<T>();
    
    public virtual IOption<T> Visit(Multiply _) => new None<T>();
    
    public virtual IOption<T> Visit(Divide _) => new None<T>();
    
    public virtual IOption<T> Visit(BinaryOperation _) => new None<T>();
    
    public virtual IOption<T> Visit(Disunification _) => new None<T>();
    
    public virtual IOption<T> Visit(Equality _) => new None<T>();
    
    public virtual IOption<T> Visit(GreaterOrEqualThan _) => new None<T>();
    
    public virtual IOption<T> Visit(GreaterThan _) => new None<T>();
    
    public virtual IOption<T> Visit(LessOrEqualThan _) => new None<T>();
    
    public virtual IOption<T> Visit(LessThan _) => new None<T>();
    
    public virtual IOption<T> Visit(Is _) => new None<T>();
    
    public virtual IOption<T> Visit(AnonymusVariableTerm _) => new None<T>();
    
    public virtual IOption<T> Visit(VariableTerm _) => new None<T>();
    
    public virtual IOption<T> Visit(ArithmeticOperationTerm _) => new None<T>();
    
    public virtual IOption<T> Visit(BasicTerm _) => new None<T>();
    
    public virtual IOption<T> Visit(StringTerm _) => new None<T>();
    
    public virtual IOption<T> Visit(NumberTerm _) => new None<T>();
    
    public virtual IOption<T> Visit(NegatedTerm _) => new None<T>();
    
    public virtual IOption<T> Visit(ParenthesizedTerm _) => new None<T>();
}