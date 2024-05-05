using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System.Collections.Immutable;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class VariableExtractor : ISimpleTermVisitor<IEnumerable<Variable>>
{
    public IImmutableSet<Variable> GetVariables(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this).ToImmutableHashSet(new VariableComparer());
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
