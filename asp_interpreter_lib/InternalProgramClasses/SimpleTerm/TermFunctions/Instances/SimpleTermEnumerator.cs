using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class SimpleTermEnumerator : ISimpleTermVisitor<IEnumerable<ISimpleTerm>>
{
    public IEnumerable<ISimpleTerm> Enumerate(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        foreach (var t in term.Accept(this))
        {
            yield return t;
        }
    }

    public IEnumerable<ISimpleTerm> Visit(Variable variable)
    {
        ArgumentNullException.ThrowIfNull(variable);

        yield return variable;
    }

    public IEnumerable<ISimpleTerm> Visit(Structure structure)
    {
        ArgumentNullException.ThrowIfNull(structure);

        yield return structure;

        foreach (var childTerm in structure.Children)
        {
            foreach(var term in childTerm.Accept(this))
            {
                yield return term;
            }
        }
    }

    public IEnumerable<ISimpleTerm> Visit(Integer integer)
    {
        ArgumentNullException.ThrowIfNull(integer);

        yield return integer;
    }
}
