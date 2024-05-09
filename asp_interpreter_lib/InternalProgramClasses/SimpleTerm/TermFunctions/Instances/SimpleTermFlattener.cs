using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class SimpleTermFlattener : ISimpleTermVisitor<IEnumerable<ISimpleTerm>>
{
    public IEnumerable<ISimpleTerm> ToList(ISimpleTerm term)
    {
        foreach (var t in term.Accept(this))
        {
            yield return t;
        }
    }

    public IEnumerable<ISimpleTerm> Visit(Variable variableTerm)
    {
       yield return variableTerm;
    }

    public IEnumerable<ISimpleTerm> Visit(Structure basicTerm)
    {
        yield return basicTerm;

        foreach (var childTerm in basicTerm.Children)
        {
            foreach(var term in childTerm.Accept(this))
            {
                yield return term;
            }
        }
    }

    public IEnumerable<ISimpleTerm> Visit(Integer integer)
    {
        yield return integer;
    }
}
