using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.ClauseRenamer;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class VariableExtractor : ISimpleTermVisitor<IEnumerable<Variable>>
{
    public HashSet<Variable> GetVariableNames(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this).ToHashSet(new VariableComparer());
    }

    public IEnumerable<Variable> Visit(Variable term)
    {
        return new List<Variable>() { term };
    }

    public IEnumerable<Variable> Visit(Structure term)
    {
        IEnumerable<Variable> result = new List<Variable>();

        foreach (var child in term.Children)
        {
            var childVars = child.Accept(this);

            result = result.Concat(childVars);
        }

        return result;
    }

    public IEnumerable<Variable> Visit(Integer integer)
    {
        return [];
    }
}
