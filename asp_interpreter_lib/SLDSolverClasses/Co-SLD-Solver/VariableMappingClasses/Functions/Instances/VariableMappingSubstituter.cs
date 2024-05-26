// <copyright file="VariableMappingSubstituter.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

public class VariableMappingSubstituter : ISimpleTermArgsVisitor<ISimpleTerm, VariableMapping>
{
    /// <summary>
    /// Substitutes all variables in the term by their value in mapping, in case they have a termbinding.
    /// </summary>
    /// <param name="term"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public ISimpleTerm SubstituteTerm(ISimpleTerm term, VariableMapping mapping)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(mapping);
            
        return term.Accept(this, mapping);
    }

    public Structure SubstituteStructure(Structure term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        var newChildren = new ISimpleTerm[term.Children.Count];

        Parallel.For(0, newChildren.Length, index =>
        {
            newChildren[index] = term.Children.ElementAt(index).Accept(this, map);
        });

        return new Structure(term.Functor, newChildren);
    }

    public ISimpleTerm SubstituteVariable(Variable term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        if (!map.TryGetValue(term, out IVariableBinding? value))
        {
            return term;
        }

        var tbMaybe = VarMappingFunctions.ReturnTermbindingOrNone(value);

        if (!tbMaybe.HasValue)
        {
            return term;
        }

        return tbMaybe.GetValueOrThrow().Term;
    }

    public ISimpleTerm Visit(Variable term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return SubstituteVariable(term, map);
    }

    public ISimpleTerm Visit(Structure term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return SubstituteStructure(term, map);
    }

    public ISimpleTerm Visit(Integer term, VariableMapping map)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(map);

        return term;
    }
}
