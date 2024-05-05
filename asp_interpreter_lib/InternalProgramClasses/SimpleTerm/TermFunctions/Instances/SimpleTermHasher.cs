using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class SimpleTermHasher : ISimpleTermVisitor<string>
{
    public int Hash(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        return term.Accept(this).Length.GetHashCode();
    }

    public string Visit(Variable variableTerm)
    {
        return $"V:{variableTerm.Identifier}";
    }

    public string Visit(Structure basicTerm)
    {
        return $"S:{basicTerm.Functor}:C:{basicTerm.Children.Select(x => x.Accept(this))}";
    }

    public string Visit(Integer integer)
    {
        return $"I:{integer.Value}";
    }
}
