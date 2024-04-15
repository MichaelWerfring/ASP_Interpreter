using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

public interface ISimpleTermVisitor
{
    void Visit(Variable variableTerm);
    void Visit(Structure basicTerm);
    void Visit(Division divide);
    void Visit(Multiplication multiply);
    void Visit(Addition plus);
    void Visit(Subtraction subtract);
    void Visit(GreaterThan greaterThan);
    void Visit(GreaterThanOrEqual greaterThanOrEqual);
    void Visit(LessOrEqualThan lessOrEqualThan);
    void Visit(LessThan lessThan);
    void Visit(List list);
    void Visit(Parenthesis parenthesis);
    void Visit(Evaluation evaluation);
    void Visit(Integer integer);
    void Visit(Nil nil);
    void Visit(ClassicalNegation classicalNegation);
    void Visit(Naf naf);
    void Visit(ForAll forAll);
    void Visit(DisunificationStructure disunificationStructure);
    void Visit(UnificationStructure unificationStructure);
}

public interface ISimpleTermVisitor<T>
{
    T Visit(Variable variableTerm);
    T Visit(Structure basicTerm);
    T Visit(Division divide);
    T Visit(Multiplication multiply);
    T Visit(Addition plus);
    T Visit(Subtraction subtract);
    T Visit(GreaterThan greaterThan);
    T Visit(GreaterThanOrEqual greaterThanOrEqual);
    T Visit(LessOrEqualThan lessOrEqualThan);
    T Visit(LessThan lessThan);
    T Visit(List list);
    T Visit(Parenthesis parenthesis);
    T Visit(Evaluation evaluation);
    T Visit(Integer integer);
    T Visit(Nil nil);
    T Visit(ClassicalNegation classicalNegation);
    T Visit(Naf naf);
    T Visit(ForAll forAll);
    T Visit(DisunificationStructure disunificationStructure);
    T Visit(UnificationStructure unificationStructure);
}

public interface ISimpleTermArgsVisitor<TArgs>
{
    void Visit(Variable variableTerm, TArgs arguments);
    void Visit(Structure basicTerm, TArgs arguments);
    void Visit(Division divide, TArgs arguments);
    void Visit(Multiplication multiply, TArgs arguments);
    void Visit(Addition plus, TArgs arguments);
    void Visit(Subtraction subtract, TArgs arguments);
    void Visit(GreaterThan greaterThan, TArgs arguments);
    void Visit(GreaterThanOrEqual greaterThanOrEqual, TArgs arguments);
    void Visit(LessOrEqualThan lessOrEqualThan, TArgs arguments);
    void Visit(LessThan lessThan, TArgs arguments);
    void Visit(List list, TArgs arguments);
    void Visit(Parenthesis parenthesis, TArgs arguments);
    void Visit(Evaluation evaluation, TArgs arguments);
    void Visit(Integer integer, TArgs arguments);
    void Visit(Nil nil, TArgs arguments);
    void Visit(ClassicalNegation classicalNegation, TArgs arguments);
    void Visit(Naf naf, TArgs arguments);
    void Visit(ForAll forAll, TArgs arguments);
    void Visit(DisunificationStructure disunificationStructure, TArgs arguments);
    void Visit(UnificationStructure unificationStructure, TArgs arguments);
}

public interface ISimpleTermArgsVisitor<TResult, TArgs>
{
    TResult Visit(Variable variableTerm, TArgs arguments);
    TResult Visit(Structure basicTerm, TArgs arguments);
    TResult Visit(Division divide, TArgs arguments);
    TResult Visit(Multiplication multiply, TArgs arguments);
    TResult Visit(Addition plus, TArgs arguments);
    TResult Visit(Subtraction subtract, TArgs arguments);
    TResult Visit(GreaterThan greaterThan, TArgs arguments);
    TResult Visit(GreaterThanOrEqual greaterThanOrEqual, TArgs arguments);
    TResult Visit(LessOrEqualThan lessOrEqualThan, TArgs arguments);
    TResult Visit(LessThan lessThan, TArgs arguments);
    TResult Visit(List list, TArgs arguments);
    TResult Visit(Parenthesis parenthesis, TArgs arguments);
    TResult Visit(Evaluation evaluation, TArgs arguments);
    TResult Visit(Integer integer, TArgs arguments);
    TResult Visit(Nil nil, TArgs arguments);
    TResult Visit(ClassicalNegation classicalNegation, TArgs arguments);
    TResult Visit(Naf naf, TArgs arguments);
    TResult Visit(ForAll forAll, TArgs arguments);
    TResult Visit(DisunificationStructure disunificationStructure, TArgs arguments);
    TResult Visit(UnificationStructure unificationStructure, TArgs arguments);
}