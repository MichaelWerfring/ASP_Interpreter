﻿using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class SimpleTermContainsChecker : ISimpleTermArgsVisitor<bool, ISimpleTerm>
{
    public bool LeftContainsRight(ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.Accept(this, other);
    }

    public bool Visit(Variable term, ISimpleTerm other)
    {
        return term.IsEqualTo(other);
    }

    public bool Visit(Structure term, ISimpleTerm other)
    {
        var areEqual = term.IsEqualTo(other);
        if (areEqual)
        {
            return true;
        }

        foreach (var child in term.Children)
        {
            bool containsEqualChild = child.Accept(this, other);

            if (containsEqualChild)
            {
                return true;
            }
        }

        return false;
    }

    public bool Visit(Integer term, ISimpleTerm other)
    {
        return term.IsEqualTo(other);
    }
}