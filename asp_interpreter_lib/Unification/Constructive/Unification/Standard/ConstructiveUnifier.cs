using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using System.Collections.Immutable;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;
using asp_interpreter_lib.Unification.Constructive.CaseDetermination;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

/// <summary>
/// An instance class to provide context for the unification algorithm.
/// </summary>
internal class ConstructiveUnifier : IBinaryTermCaseVisitor
{
    // function providers
    private readonly ConstructiveVariableSubstitutor _maybeSubstitutor;
    private readonly SubstitutionApplier _subApplier;
    private readonly ProhibitedValuesUpdater _varUpdater;
    private readonly CaseDeterminer _caseDeterminer;

    // input by constructor args
    private readonly bool _doOccursCheck;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;

    // Changed during execution
    private VariableMapping _mapping;
    private bool _hasSucceded;

    /// <summary>
    /// Creates a new instance of the class. Should never be called directly, except by StandardConstructiveAlgorithm.
    /// </summary>
    public ConstructiveUnifier
    (
        bool doOccursCheck, 
        ConstructiveTarget target,
        ConstructiveVariableSubstitutor maybeSubstituter,
        SubstitutionApplier subApplier,
        ProhibitedValuesUpdater varUpdater,
        CaseDeterminer caseDeterminer
    )
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(maybeSubstituter);
        ArgumentNullException.ThrowIfNull (subApplier);
        ArgumentNullException.ThrowIfNull (varUpdater);
        ArgumentNullException.ThrowIfNull(caseDeterminer);

        _maybeSubstitutor = maybeSubstituter;
        _subApplier = subApplier;
        _varUpdater = varUpdater;
        _caseDeterminer = caseDeterminer;

        _doOccursCheck = doOccursCheck;
        _left = target.Left;
        _right = target.Right;

        _mapping = new VariableMapping(target.Mapping);

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
        // stop if we have failed
        if (!_hasSucceded)
        {
            return;
        }

        // get values of current terms if they are variables and they map to a term.
        ISimpleTerm currentLeft = _maybeSubstitutor.TryGetSubstitution(left, _mapping);
        ISimpleTerm currentRight = _maybeSubstitutor.TryGetSubstitution(right, _mapping);

        // determine case and resolve.
        _caseDeterminer.DetermineCase(currentLeft, currentRight).Accept(this);
    }

    public void Visit(VariableVariableCase currentCase)
    {
        ResolveVariableVariableCase(currentCase.Left, currentCase.Right);
    }

    public void Visit(VariableStructureCase currentCase)
    {
        ResolveVariableStructureCase(currentCase.Left, currentCase.Right);
    }

    public void Visit(StructureStructure currentCase)
    {
        ResolveStructureStructureCase(currentCase.Left, currentCase.Right);
    }

    public void Visit(StructureVariableCase unificationCase)
    {
        ResolveVariableStructureCase(unificationCase.Right, unificationCase.Left);
    }

    private void ResolveStructureStructureCase(IStructure left, IStructure right)
    {
        var reductionMaybe = TermFuncs.Reduce(left, right);
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

    private void ResolveVariableStructureCase(Variable left, IStructure right)
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
        _mapping = _subApplier.ApplySubstitutionComposition(_mapping, left, right);
    }

    private void ResolveVariableVariableCase(Variable left, Variable right)
    {
        // if both are equal, do nothing
        if (left.IsEqualTo(right))
        {
            return;
        }

        _mapping =_varUpdater.UpdateProhibitedValues(left, right, _mapping);

        _mapping = _subApplier.ApplySubstitutionComposition(_mapping, left, right);
    }
}