using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.BinaryOperations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using GreaterThan = asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison.GreaterThan;
using LessOrEqualThan = asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison.LessOrEqualThan;
using LessThan = asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison.LessThan;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermComparer : ISimpleTermArgsVisitor<bool, ISimpleTerm>, IEqualityComparer<ISimpleTerm>
{
    public bool Equals(ISimpleTerm? x, ISimpleTerm? y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        return x.Accept(this, y);
    }

    public int GetHashCode([DisallowNull] ISimpleTerm obj)
    {
        return obj.GetHashCode();
    }

    public bool Visit(Variable a, ISimpleTerm b)
    {
        Variable bVar;
        try
        {
            bVar = (Variable)b;
        }
        catch
        {
            return false;
        }

        return a.Identifier == bVar.Identifier;
    }

    public bool Visit(Structure a, ISimpleTerm b)
    {

        Structure bStruct;
        try
        {
            bStruct = (Structure)b;
        }
        catch
        {
            return false;
        }

        if (a.Functor != bStruct.Functor)
        {
            return false;
        }

        if (a.Children.Count() != bStruct.Children.Count())
        {
            return false;
        }

        for (int i = 0; i < a.Children.Count(); i++)
        {

            if (!Equals(a.Children.ElementAt(i), bStruct.Children.ElementAt(i)))
            {
                return false;
            }

        }

        return true;
    }

    public bool Visit(Division divide, ISimpleTerm arguments)
    {
        Division b;
        try
        {
            b = (Division)arguments;
        }
        catch
        {
            return false;
        }

        return divide.Left.Accept(this, b.Left) && divide.Right.Accept(this, b.Right);
    }

    public bool Visit(Multiplication multiply, ISimpleTerm arguments)
    {
        Multiplication b;
        try
        {
            b = (Multiplication)arguments;
        }
        catch
        {
            return false;
        }

        return multiply.Left.Accept(this, b.Left) && multiply.Right.Accept(this, b.Right);
    }

    public bool Visit(Addition plus, ISimpleTerm arguments)
    {
        Addition b;
        try
        {
            b = (Addition)arguments;
        }
        catch
        {
            return false;
        }

        return plus.Left.Accept(this, b.Left) && plus.Right.Accept(this, b.Right);
    }

    public bool Visit(Subtraction subtract, ISimpleTerm arguments)
    {
        Subtraction b;
        try
        {
            b = (Subtraction)arguments;
        }
        catch
        {
            return false;
        }

        return subtract.Left.Accept(this, b.Left) && subtract.Right.Accept(this, b.Right);
    }

    public bool Visit(GreaterThan greaterThan, ISimpleTerm arguments)
    {
        GreaterThan b;
        try
        {
            b = (GreaterThan)arguments;
        }
        catch
        {
            return false;
        }

        return greaterThan.Left.Accept(this, b.Left) && greaterThan.Right.Accept(this, b.Right);
    }

    public bool Visit(GreaterThanOrEqual greaterThanOrEqual, ISimpleTerm arguments)
    {
        GreaterThanOrEqual b;
        try
        {
            b = (GreaterThanOrEqual)arguments;
        }
        catch
        {
            return false;
        }
        return greaterThanOrEqual.Left.Accept(this, b.Left) && greaterThanOrEqual.Right.Accept(this, b.Right);
    }

    public bool Visit(LessOrEqualThan lessOrEqualThan, ISimpleTerm arguments)
    {
        LessOrEqualThan b;
        try
        {
            b = (LessOrEqualThan)arguments;
        }
        catch
        {
            return false;
        }

        return lessOrEqualThan.Left.Accept(this, b.Left) && lessOrEqualThan.Right.Accept(this, b.Right);
    }

    public bool Visit(LessThan lessThan, ISimpleTerm arguments)
    {
        LessThan b;
        try
        {
            b = (LessThan)arguments;
        }
        catch
        {
            return false;
        }

        return lessThan.Left.Accept(this, b.Left) && lessThan.Right.Accept(this, b.Right);
    }

    public bool Visit(List list, ISimpleTerm arguments)
    {
        List b;
        try
        {
            b = (List)arguments;
        }
        catch
        {
            return false;
        }

        return list.Left.Accept(this, b.Left) && list.Right.Accept(this, b.Right);
    }

    public bool Visit(Parenthesis parenthesis, ISimpleTerm arguments)
    {
        Parenthesis b;
        try
        {
            b = (Parenthesis)arguments;
        }
        catch
        {
            return false;
        }

        return parenthesis.Term.Accept(this, b.Term);
    }

    public bool Visit(Evaluation evaluation, ISimpleTerm arguments)
    {
        Evaluation b;
        try
        {
            b = (Evaluation)arguments;
        }
        catch
        {
            return false;
        }

        return evaluation.Left.Accept(this, b.Left) && evaluation.Right.Accept(this, b.Right);
    }

    public bool Visit(Integer integer, ISimpleTerm arguments)
    {
        Integer b;
        try
        {
            b = (Integer)arguments;
        }
        catch
        {
            return false;
        }

        return integer.Value == b.Value;
    }

    public bool Visit(Nil nil, ISimpleTerm arguments)
    {
        Nil b;
        try
        {
            b = (Nil)arguments;
        }
        catch
        {
            return false;
        }

        return true;
    }

    public bool Visit(ClassicalNegation classicalNegation, ISimpleTerm arguments)
    {
        ClassicalNegation b;
        try
        {
            b = (ClassicalNegation)arguments;
        }
        catch
        {
            return false;
        }

        return classicalNegation.Term.Accept(this, b.Term);
    }

    public bool Visit(Naf naf, ISimpleTerm arguments)
    {
        Naf b;
        try
        {
            b = (Naf)arguments;
        }
        catch
        {
            return false;
        }

        return naf.Term.Accept(this, b.Term);
    }

    public bool Visit(ForAll forAll, ISimpleTerm arguments)
    {
        ForAll b;
        try
        {
            b = (ForAll)arguments;
        }
        catch
        {
            return false;
        }

        return forAll.VarTerm.Accept(this, b.VarTerm) && forAll.Goal.Accept(this, b.Goal);
    }

    public bool Visit(DisunificationStructure disunificationStructure, ISimpleTerm arguments)
    {
        DisunificationStructure b;
        try
        {
            b = (DisunificationStructure)arguments;
        }
        catch
        {
            return false;
        }

        return disunificationStructure.Left.Accept(this, b.Left) && disunificationStructure.Right.Accept(this, b.Right);
    }

    public bool Visit(UnificationStructure unificationStructure, ISimpleTerm arguments)
    {
        UnificationStructure b;
        try
        {
            b = (UnificationStructure)arguments;
        }
        catch
        {
            return false;
        }

        return unificationStructure.Left.Accept(this, b.Left) && unificationStructure.Right.Accept(this, b.Right);
    }
}
