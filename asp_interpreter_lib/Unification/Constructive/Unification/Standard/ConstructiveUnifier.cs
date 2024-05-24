using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using System.Collections.Immutable;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Antlr4.Runtime.Misc;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

/// <summary>
/// An instance class to provide context for the unification algorithm.
/// </summary>
internal class ConstructiveUnifier : IBinaryTermCaseVisitor
{
    // input by constructor args
    private readonly bool _doOccursCheck;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;

    // mutated during execution
    private IDictionary<Variable, ProhibitedValuesBinding> _prohibitedValues;
    private IDictionary<Variable, TermBinding> _termBindings;
    private bool _hasSucceded;

    /// <summary>
    /// Creates a new instance of the class. Should never be called directly, except by StandardConstructiveAlgorithm.
    /// </summary>
    public ConstructiveUnifier(bool doOccursCheck, ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        _doOccursCheck = doOccursCheck;

        _left = target.Left;
        _right = target.Right;

        _prohibitedValues = target.Mapping.ToDictionary(TermFuncs.GetSingletonVariableComparer());
        _termBindings = new Dictionary<Variable, TermBinding> (TermFuncs.GetSingletonVariableComparer());

        _hasSucceded = true;
    }

    public IOption<VariableMapping> Unify()
    {
        TryUnify(_left, _right);

        if (!_hasSucceded)
        {
            return new None<VariableMapping>();
        }
        
        return new Some<VariableMapping>(VarMappingFunctions.Merge(_prohibitedValues, _termBindings));
    }

    // cases
    public void Visit(VariableVariableCase currentCase)
    {
        ArgumentNullException.ThrowIfNull(currentCase);

        ResolveVariableVariableCase(currentCase.Left, currentCase.Right);
    }

    public void Visit(VariableStructureCase currentCase)
    {
        ArgumentNullException.ThrowIfNull(currentCase);

        ResolveVariableStructureCase(currentCase.Left, currentCase.Right);
    }

    public void Visit(StructureStructureCase currentCase)
    {
        ArgumentNullException.ThrowIfNull(currentCase);

        ResolveStructureStructureCase(currentCase.Left, currentCase.Right);
    }

    public void Visit(StructureVariableCase currentCase)
    {
        ArgumentNullException.ThrowIfNull(currentCase);

        ResolveVariableStructureCase(currentCase.Right, currentCase.Left);
    }

    public void Visit(IntegerIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        ResolveStructureStructureCase(binaryCase.Left, binaryCase.Right);
    }

    public void Visit(IntegerStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        ResolveStructureStructureCase(binaryCase.Left, binaryCase.Right);
    }

    public void Visit(IntegerVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        ResolveVariableStructureCase(binaryCase.Right, binaryCase.Left);
    }

    public void Visit(StructureIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        ResolveStructureStructureCase(binaryCase.Left, binaryCase.Right);
    }

    public void Visit(VariableIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        ResolveVariableStructureCase(binaryCase.Left, binaryCase.Right);
    }

    private void TryUnify(ISimpleTerm left, ISimpleTerm right)
    {
        // stop if we have failed
        if (!_hasSucceded)
        {
            return;
        }

        // get values of current terms if they are variables and they map to a term.
        ISimpleTerm currentLeft = TryGetSubstitution(left);
        ISimpleTerm currentRight = TryGetSubstitution(right);

        // determine case and resolve.
        IBinaryTermCase typeCase = TermFuncs.DetermineCase(currentLeft, currentRight);

        typeCase.Accept(this);
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
        var prohibitedValuesOfLeft = _prohibitedValues[left];
        if (prohibitedValuesOfLeft.ProhibitedValues.Contains(right))
        {
            _hasSucceded = false;
            return;
        }

        // now do substitution composition
        ApplySubstitutionComposition(left, right);
    }

    private void ResolveVariableVariableCase(Variable left, Variable right)
    {
        // if both are equal, do nothing
        if (left.IsEqualTo(right))
        {
            return;
        }

        UpdateProhibitedValues(left, right);

        ApplySubstitutionComposition(left, right);
    }

    private void UpdateProhibitedValues(Variable left, Variable right)
    {
        var leftProhibs = _prohibitedValues[left].ProhibitedValues;
        var rightProhibs = _prohibitedValues[right].ProhibitedValues;

        var union = leftProhibs.Union(rightProhibs);

        _prohibitedValues.Remove(left);

        _prohibitedValues[right] = new ProhibitedValuesBinding(union);
    }

    private void ApplySubstitutionComposition(Variable var, ISimpleTerm term)
    {
        var dictForSubstitution = new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer())
        {
            { var, term }
        };

        var newPairs = new KeyValuePair<Variable, TermBinding>[_termBindings.Count];

        Parallel.For(0, _termBindings.Count, index =>
        {
            var currentPair =_termBindings.ElementAt(index);

            newPairs[index] = new KeyValuePair<Variable, TermBinding>
                (currentPair.Key, new TermBinding(currentPair.Value.Term.Substitute(dictForSubstitution)));
        });

        _termBindings = newPairs.ToDictionary(TermFuncs.GetSingletonVariableComparer());

        _termBindings[var] = new TermBinding(term);
    }

    private ISimpleTerm TryGetSubstitution(ISimpleTerm term)
    {
        var variableMaybe = TermFuncs.ReturnVariableOrNone(term);

        if (!variableMaybe.HasValue)
        {
            return term;
        }

        if (!_termBindings.TryGetValue(variableMaybe.GetValueOrThrow(), out TermBinding? value))
        {
            return term;
        }

        return value.Term;
    }
}