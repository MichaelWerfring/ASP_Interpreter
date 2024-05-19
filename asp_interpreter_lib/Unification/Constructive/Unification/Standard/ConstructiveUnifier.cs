using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.Unification.StructureReducers;
using System.Collections.Immutable;
using Antlr4.Runtime.Misc;
using System;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class ConstructiveUnifier
{
    private readonly ConstructiveVariableSubstitutor _maybeSubstitutor = new();
    private readonly StructureReducer _reducer = new();

    // input by constructor args
    private readonly bool _doOccursCheck;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;

    // Changed during execution
    private VariableMapping _mapping;
    private bool _hasSucceded;

    public ConstructiveUnifier(bool doOccursCheck, ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        _doOccursCheck = doOccursCheck;
        _left = target.Left;
        _right = target.Right;
        _mapping = target.Mapping;

        _hasSucceded = true;
    }

    public IOption<VariableMapping> Unify()
    {
        TryUnify(_left, _right);

        if (!_hasSucceded)
        {
            return new None<VariableMapping>();
        }

        return new Some<VariableMapping>(_mapping);
    }

    private void TryUnify(ISimpleTerm left, ISimpleTerm right)
    {
        if (!_hasSucceded)
        {
            return;
        }

        // get values of current terms if they are variables and they map to a term.
        ISimpleTerm currentLeft = _maybeSubstitutor.TryGetSubstitution(left, _mapping);
        ISimpleTerm currentRight = _maybeSubstitutor.TryGetSubstitution(right, _mapping);

        // determine case
        if (currentLeft is Variable leftVariable)
        {
            if (currentRight is Variable rightVariable)
            {
                LeftIsVarRightIsVar(leftVariable, rightVariable);
            }
            else if (currentRight is IStructure rightStructure)
            {
                LeftIsVarRightIsStruct(leftVariable, rightStructure);
            }
            else
            {
                throw new ArgumentException("The type hierarchy has been modified so that not every term is either a variable or a structure!");
            }
        }
        else if (currentLeft is IStructure leftStructure)
        {
            if (currentRight is Variable rightVariable)
            {
                LeftIsStructRightIsVar(leftStructure, rightVariable);
            }
            else if (currentRight is IStructure rightStructure)
            {
                LeftIsStructRightIsStruct(leftStructure, rightStructure);
            }
            else
            {
                throw new ArgumentException("The type hierarchy has been modified so that not every term is either a variable or a structure!");
            }
        }
        else
        {
            throw new ArgumentException("The type hierarchy has been modified so that not every term is either a variable or a structure!");
        }
    }

    // cases
    private void LeftIsStructRightIsStruct(IStructure left, IStructure right)
    {
        var reductionMaybe = _reducer.TryReduce(left, right);
        if (!reductionMaybe.HasValue)
        {
            _hasSucceded = false;
            return;
        }

        var reduction = reductionMaybe.GetValueOrThrow();

        foreach (var pair in reduction)
        {
            TryUnify(pair.Item1, pair.Item2);
        }
    }

    private void LeftIsStructRightIsVar(IStructure left, Variable right)
    {
        LeftIsVarRightIsStruct(right, left);
    }

    private void LeftIsVarRightIsStruct(Variable left, IStructure right)
    {
        // do occurs check if asked for
        if (_doOccursCheck && right.Contains(left))
        {
            _hasSucceded = false;
            return;
        }

        // check if right is in prohibited value list of left: if yes, then fail.
        var prohibitedValuesOfLeft = (ProhibitedValuesBinding)_mapping[left];
        if (prohibitedValuesOfLeft.ProhibitedValues.Contains(right))
        {
            _hasSucceded = false;
            return;
        }

        // now do substitution composition
        _mapping = ApplySubstitutionComposition(_mapping, left, right);
    }

    private void LeftIsVarRightIsVar(Variable left, Variable right)
    {
        // if both are equal, do nothing
        if (left.IsEqualTo(right))
        {
            return;
        }

        UpdateProhibitedValues(left, right);

        _mapping = ApplySubstitutionComposition(_mapping, left, right);
    }

    // updating
    private void UpdateProhibitedValues(Variable left, Variable right)
    {
        ProhibitedValuesBinding leftVals = (ProhibitedValuesBinding)_mapping[left];
        ProhibitedValuesBinding rightVals = (ProhibitedValuesBinding)_mapping[right];

        ImmutableSortedSet<ISimpleTerm> union = leftVals.ProhibitedValues
                                                    .Union(rightVals.ProhibitedValues)
                                                    .ToImmutableSortedSet(new SimpleTermComparer());

        _mapping = _mapping.SetItem
        (
            right,
            new ProhibitedValuesBinding(union.ToImmutableSortedSet(new SimpleTermComparer()))
        );
    }

    private VariableMapping ApplySubstitutionComposition(VariableMapping oldMapping, Variable var, ISimpleTerm term)
    {
        var dictForSubstitution = new Dictionary<Variable, ISimpleTerm>(new VariableComparer())
        {
            { var, term }
        };

        var newMap = oldMapping;

        foreach (var pair in oldMapping)
        {
            if (pair.Value is TermBinding binding)
            {
                newMap = newMap.SetItem(pair.Key, new TermBinding(binding.Term.Substitute(dictForSubstitution)));
            }
        }
        newMap = newMap.SetItem(var, new TermBinding(term));

        return newMap;
    }
}
