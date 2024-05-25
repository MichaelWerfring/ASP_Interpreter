using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class SimpleTermContainsChecker : ISimpleTermArgsVisitor<bool, ISimpleTerm>
{
    public bool LeftContainsRight(ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.Accept(this, other);
    }

    public bool Visit(Structure term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        var areEqual = term.IsEqualTo(other);

        if (areEqual)
        {
            return true;
        }

        if (term.Children.Any(child => child.Accept(this, other)))
        {
            return true;
        }

        return false;
    }

    public bool Visit(Variable term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.IsEqualTo(other);
    }

    public bool Visit(Integer term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.IsEqualTo(other);
    }
}
