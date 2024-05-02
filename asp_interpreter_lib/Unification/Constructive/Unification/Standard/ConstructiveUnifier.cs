using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Unification.Basic.Robinson;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class ConstructiveUnifier
{
    // functions
    private SimpleTermCloner _cloner = new SimpleTermCloner();
    private SimpleTermComparer _comparer = new SimpleTermComparer();
    private SimpleTermContainsChecker _containsChecker = new SimpleTermContainsChecker();
    private VariableSubstituter _substituter = new VariableSubstituter();
    private ConstructiveVariableSubstitutor _maybeSubstitutor = new ConstructiveVariableSubstitutor();
    private StructureReducer _reducer = new StructureReducer();

    // input by constructor
    private bool _doOccursCheck;
    private ISimpleTerm _left;
    private ISimpleTerm _right;
    private VariableMapping _mapping;

    // indicates failure : assumed to be successful until proven otherwise
    private bool _hasSucceded;

    public ConstructiveUnifier(bool doOccursCheck, ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        _doOccursCheck = doOccursCheck;

        _left = target.Left;
        _right = target.Right;

        // copy the dictionary, construct variableMapping
        var copiedDict = new Dictionary<Variable, IVariableBinding>(new VariableComparer());
        foreach (var pair in target.Mapping)
        {
            ProhibitedValuesBinding copiedProhibitedValues = new ProhibitedValuesBinding
            (
                pair.Value.ProhibitedValues
                                    .Select(_cloner.Clone)
                                    .ToHashSet(new SimpleTermComparer())
            );

            copiedDict.Add(new Variable(pair.Key.Identifier.GetCopy()), copiedProhibitedValues);
        }
        _mapping = new VariableMapping(copiedDict);
        //

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
        if (!_hasSucceded) { return; }

        // substitute current terms if they are variables and they map to a term.
        ISimpleTerm currentLeft = left;
        ISimpleTerm currentRight = right;
        if (left is Variable leftVar)
        {
            currentLeft = _maybeSubstitutor.GetSubstitutionOrDefault(leftVar, _mapping);
        }
        if (right is Variable rightVar)
        {
            currentRight = _maybeSubstitutor.GetSubstitutionOrDefault(rightVar, _mapping);
        }
        // 

        // continue based on case of left.
        if (currentLeft is Variable variable)
        {
            TryUnifyVariableCase(variable, currentRight);
        }
        else if (currentLeft is IStructure structure)
        {
            TryUnifyStructureCase(structure, currentRight);
        }
        else
        {
            throw new ArgumentException
            (
                "The type hierarchy has been modified" +
                " so that not every term is either a variable or a structure!"
            );
        }
    }

    private void TryUnifyStructureCase(IStructure structure, ISimpleTerm other)
    {
        if (other is IStructure b) // both are structures
        {
            var reductionMaybe = _reducer.TryReduce(structure, b);
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
        else // left is structure and right variable
        {
            TryUnify(other, structure);
        }
    }

    private void TryUnifyVariableCase(Variable left, ISimpleTerm right)
    {
        // if both are variables and equal, do nothing
        if (_comparer.Equals(left, right))
        {
            return;
        }

        // do occurs check if asked for
        if (_doOccursCheck && _containsChecker.LeftContainsRight(right, left))
        {
            _hasSucceded = false;
            return;
        }

        // if right is a variable, then update bindings
        if (right is Variable rightVar)
        {
            UpdateBindings(left, rightVar);
        }

        // check if right is in prohibited value list of left: if yes, then fail.
        var prohibitedValuesOfLeft = (ProhibitedValuesBinding)_mapping.Mapping[left];
        if (prohibitedValuesOfLeft.ProhibitedValues.Contains(right, _comparer))
        {
            _hasSucceded = false;
            return;
        }

        // now do substitution composition
        _mapping = ApplySubstitutionComposition(_mapping, left, right);
    }

    private void UpdateBindings(Variable left, Variable right)
    {
        ProhibitedValuesBinding leftVals = (ProhibitedValuesBinding)_mapping.Mapping[left];
        ProhibitedValuesBinding rightVals = (ProhibitedValuesBinding)_mapping.Mapping[right];

        var union = leftVals.ProhibitedValues.Union(rightVals.ProhibitedValues, new SimpleTermComparer());

        _mapping.Mapping[right] = new ProhibitedValuesBinding(union.ToHashSet(new SimpleTermComparer()));
    }

    private VariableMapping ApplySubstitutionComposition(VariableMapping oldMapping, Variable var, ISimpleTerm term)
    {
        var dict = new Dictionary<Variable, ISimpleTerm>(new VariableComparer())
        {
            { var, term }
        };

        var newMapping = oldMapping.Mapping.Select(pair =>
        {
            if (pair.Value is TermBinding binding)
            {
                return (pair.Key, new TermBinding(_substituter.Substitute(binding.Term, dict)));
            }
            else
            {
                return (pair.Key, pair.Value);
            }
        })
        .ToDictionary(new VariableComparer());

        newMapping[var] = new TermBinding(term);

        return new VariableMapping(newMapping);
    }
}
