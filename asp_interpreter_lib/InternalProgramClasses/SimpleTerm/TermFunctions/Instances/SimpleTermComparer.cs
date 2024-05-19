using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

/// <summary>
/// Compares terms, based on the SWI-Prolog "Standard Order of Terms":
/// Variables < Numbers < Compounds
/// Compounds : check arity, check functor, then check children from left to right
/// </summary>
public class SimpleTermComparer : IComparer<ISimpleTerm>, ISimpleTermArgsVisitor<int, ISimpleTerm>
{
    public int Compare(ISimpleTerm? x, ISimpleTerm? y)
    {
        if (x == null && y == null) { return 0; }

        if (x == null) { return -1; }

        if (y == null) { return 1; }

        return x.Accept(this, y);
    }

    public int Visit(Variable left, ISimpleTerm right)
    {
        if (right is Variable rightVar)
        {
            return left.Identifier.CompareTo(rightVar.Identifier);
        }
        else if (right is Integer)
        {
            return -1;
        }
        else if (right is Structure)
        {
            return -1;
        }
        else
        {
            throw new Exception("New types have been added to the type hierarchy!");
        }
    }

    public int Visit(Structure left, ISimpleTerm right)
    {
        if (right is Variable)
        {
            return 1;
        }
        else if (right is Integer)
        {
            return 1;
        }
        else if (right is Structure rightStructure)
        {
            return CompareStructures(left, rightStructure);
        }
        else
        {
            throw new Exception("New types have been added to the type hierarchy!");
        }
    }

    public int Visit(Integer left, ISimpleTerm right)
    {
        if (right is Variable)
        {
            return 1;
        }
        else if (right is Integer rightInteger)
        {
            return left.Value.CompareTo(rightInteger.Value);
        }
        else if (right is Structure)
        {
            return -1;
        }
        else
        {
            throw new Exception("New types have been added to the type hierarchy!");
        }
    }

    private int CompareStructures(Structure left, Structure right)
    {
        var childCountComparison = left.Children.Count.CompareTo(right.Children.Count);

        if (childCountComparison != 0) {return childCountComparison;}

        var functorComparions = left.Functor.CompareTo(right.Functor);

        if (functorComparions != 0) {  return functorComparions;}

        for(int i = 0; i < left.Children.Count; i++)
        {
            var currentChildrenComparison = left.Children.ElementAt(i).Accept(this, right.Children.ElementAt(i));

            if (currentChildrenComparison!= 0)
            {
                return currentChildrenComparison;
            }
        }

        return 0;
    }
}
