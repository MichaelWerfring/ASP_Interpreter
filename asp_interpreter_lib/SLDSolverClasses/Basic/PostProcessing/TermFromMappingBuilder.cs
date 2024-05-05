using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Util;
using System.Collections.Immutable;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.PostProcessing;

internal class TermFromMappingBuilder : ISimpleTermArgsVisitor<ISimpleTerm, Dictionary<Variable, ISimpleTerm>>
{
    public ISimpleTerm BuildTerm(ISimpleTerm term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        ArgumentNullException.ThrowIfNull(term, nameof(term));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        return term.Accept(this, mapping);
    }

    public ISimpleTerm Visit(Variable term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        if (!mapping.ContainsKey(term)) { return term.Clone(); }

        return mapping[term].Accept(this, mapping);
    }

    public ISimpleTerm Visit(Structure term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        var newChildren = new ISimpleTerm[term.Children.Count()];

        for (int i = 0; i < term.Children.Count(); i++)
        {
            newChildren[i] = term.Children.ElementAt(i).Accept(this, mapping);
        }

        return new Structure(term.Functor.GetCopy(), newChildren.ToImmutableList());
    }

    public ISimpleTerm Visit(Integer term, Dictionary<Variable, ISimpleTerm> mapping)
    {
        return new Integer(term.Value);
    }
}
