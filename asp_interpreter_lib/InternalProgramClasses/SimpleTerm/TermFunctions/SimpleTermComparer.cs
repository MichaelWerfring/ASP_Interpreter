﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using System.Diagnostics.CodeAnalysis;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermComparer : ISimpleTermArgsVisitor<bool, ISimpleTerm>, IEqualityComparer<ISimpleTerm>
{
    private SimpleTermHasher _hasher = new SimpleTermHasher();

    public bool Equals(ISimpleTerm? x, ISimpleTerm? y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        return x.Accept(this, y);
    }

    public int GetHashCode([DisallowNull] ISimpleTerm obj)
    {
        return _hasher.Hash(obj);
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

    public bool Visit(Integer a, ISimpleTerm b)
    {
        Integer bInt;
        try
        {
            bInt = (Integer)b;
        }
        catch
        {
            return false;
        }

        return a.Value == bInt.Value;
    }
}
