using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermContainsChecker : ISimpleTermArgsVisitor<bool, ISimpleTerm>
{
    private SimpleTermComparer _equivalenceChecker = new SimpleTermComparer();

    public bool LeftContainsRight(ISimpleTerm term, ISimpleTerm other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.Accept(this, other);
    }

    public bool Visit(Variable term, ISimpleTerm other)
    {
        return _equivalenceChecker.Visit(term, other);
    }

    public bool Visit(Structure term, ISimpleTerm other)
    {
        var areEqual = _equivalenceChecker.Visit(term, other);
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
        return _equivalenceChecker.Equals(term, other);
    }
}
