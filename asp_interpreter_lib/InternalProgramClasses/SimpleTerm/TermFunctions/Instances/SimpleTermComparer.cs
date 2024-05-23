using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

/// <summary>
/// Compares terms, based on the SWI-Prolog "Standard Order of Terms":
/// Variables < Numbers < Compounds
/// Compounds : check arity, check functor, then check children from left to right
/// </summary>
public class SimpleTermComparer : IComparer<ISimpleTerm>, IBinaryTermCaseVisitor<int>
{
    public int Compare(ISimpleTerm? x, ISimpleTerm? y)
    {
        if (x == null && y == null) { return 0; }

        if (x == null) { return -1; }

        if (y == null) { return 1; }

        return TermFuncs.DetermineCase(x, y).Accept(this);
    }
    public int Visit(IntegerIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return binaryCase.Left.Value.CompareTo(binaryCase.Right.Value);
    }

    public int Visit(IntegerStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }

    public int Visit(IntegerVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    public int Visit(StructureIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    public int Visit(StructureStructureCase binaryCase)
    {
        var childCountComparison = binaryCase.Left.Children.Count.CompareTo(binaryCase.Right.Children.Count);

        if (childCountComparison != 0) { return childCountComparison; }
        var functorComparions = binaryCase.Left.Functor.CompareTo(binaryCase.Right.Functor);

        if (functorComparions != 0) { return functorComparions; }

        for (int i = 0; i < binaryCase.Left.Children.Count; i++)
        {
            var currentChildrenComparison = TermFuncs.DetermineCase
                (binaryCase.Left.Children.ElementAt(i), binaryCase.Right.Children.ElementAt(i))
                .Accept(this);

            if (currentChildrenComparison != 0)
            {
                return currentChildrenComparison;
            }
        }

        return 0;
    }

    public int Visit(StructureVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return 1;
    }

    public int Visit(VariableIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }

    public int Visit(VariableVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return binaryCase.Left.Identifier.CompareTo(binaryCase.Right.Identifier);
    }

    public int Visit(VariableStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        return -1;
    }
}