// <copyright file="ConstructiveUnifier.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Unification.Standard;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Collections.Immutable;

/// <summary>
/// An instance class to provide context for the unification algorithm.
/// </summary>
internal class ConstructiveUnifier : IBinaryTermCaseVisitor
{
    // input by constructor args
    private readonly bool doOccursCheck;
    private readonly ISimpleTerm left;
    private readonly ISimpleTerm right;

    // mutated during execution
    private readonly IDictionary<Variable, ProhibitedValuesBinding> prohibitedValues;
    private IDictionary<Variable, TermBinding> termBindings;
    private bool hasSucceded;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructiveUnifier"/> class.
    /// </summary>
    /// <param name="doOccursCheck">Whether to do an occurs check before unifying a variable and a term.</param>
    /// <param name="target">The constructive target that contains the two input terms and their prohibted values mapping.</param>
    public ConstructiveUnifier(bool doOccursCheck, ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        this.doOccursCheck = doOccursCheck;

        this.left = target.Left;
        this.right = target.Right;

        this.prohibitedValues = target.Mapping.ToDictionary(TermFuncs.GetSingletonVariableComparer());
        this.termBindings = new Dictionary<Variable, TermBinding>(TermFuncs.GetSingletonVariableComparer());

        this.hasSucceded = true;
    }

    /// <summary>
    /// Attempts to unify the target contained in this instance.
    /// </summary>
    /// <returns>A unifying mapping, or none.</returns>
    public IOption<VariableMapping> Unify()
    {
        this.TryUnify(this.left, this.right);

        if (!this.hasSucceded)
        {
            return new None<VariableMapping>();
        }

        return new Some<VariableMapping>(VarMappingFunctions.Merge(this.prohibitedValues, this.termBindings));
    }

    /// <summary>
    /// Visits a variable-variable case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(VariableVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveVariableVariableCase(binaryCase.Left, binaryCase.Right);
    }

    /// <summary>
    /// Visits a variable-structure case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(VariableStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveVariableStructureCase(binaryCase.Left, binaryCase.Right);
    }

    /// <summary>
    /// Visits a structure-structure case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(StructureStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveStructureStructureCase(binaryCase.Left, binaryCase.Right);
    }

    /// <summary>
    /// Visits a structure-variable case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(StructureVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveVariableStructureCase(binaryCase.Right, binaryCase.Left);
    }

    /// <summary>
    /// Visits a integer-integer case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(IntegerIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveStructureStructureCase(binaryCase.Left, binaryCase.Right);
    }

    /// <summary>
    /// Visits a integer-structure case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(IntegerStructureCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveStructureStructureCase(binaryCase.Left, binaryCase.Right);
    }

    /// <summary>
    /// Visits a integer-variable case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(IntegerVariableCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveVariableStructureCase(binaryCase.Right, binaryCase.Left);
    }

    /// <summary>
    /// Visits a structure-integer case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(StructureIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveStructureStructureCase(binaryCase.Left, binaryCase.Right);
    }

    /// <summary>
    /// Visits a variable-integer case. Delegates to the appropriate private method.
    /// </summary>
    /// <param name="binaryCase">The input case.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryCase"/> is null.</exception>
    public void Visit(VariableIntegerCase binaryCase)
    {
        ArgumentNullException.ThrowIfNull(binaryCase);

        this.ResolveVariableStructureCase(binaryCase.Left, binaryCase.Right);
    }

    private void TryUnify(ISimpleTerm left, ISimpleTerm right)
    {
        // stop if we have failed
        if (!this.hasSucceded)
        {
            return;
        }

        // get values of current terms if they are variables and they map to a term.
        ISimpleTerm currentLeft = this.TryGetSubstitution(left);
        ISimpleTerm currentRight = this.TryGetSubstitution(right);

        // determine case and resolve.
        IBinaryTermCase typeCase = TermFuncs.DetermineCase(currentLeft, currentRight);

        typeCase.Accept(this);
    }

    private void ResolveStructureStructureCase(IStructure left, IStructure right)
    {
        var reductionMaybe = TermFuncs.Reduce(left, right);
        if (!reductionMaybe.HasValue)
        {
            this.hasSucceded = false;
            return;
        }

        var reduction = reductionMaybe.GetValueOrThrow();

        foreach (var pair in reduction)
        {
            this.TryUnify(pair.LeftChild, pair.RightChild);
        }
    }

    private void ResolveVariableStructureCase(Variable left, IStructure right)
    {
        // do occurs check if asked for
        if (this.doOccursCheck && right.Contains(left))
        {
            this.hasSucceded = false;
            return;
        }

        // check if right is in prohibited value list of left: if yes, then fail.
        var prohibitedValuesOfLeft = this.prohibitedValues[left];
        if (prohibitedValuesOfLeft.ProhibitedValues.Contains(right))
        {
            this.hasSucceded = false;
            return;
        }

        // now do substitution composition
        this.ApplySubstitutionComposition(left, right);
    }

    private void ResolveVariableVariableCase(Variable left, Variable right)
    {
        // if both are equal, do nothing
        if (left.IsEqualTo(right))
        {
            return;
        }

        this.UpdateProhibitedValues(left, right);

        this.ApplySubstitutionComposition(left, right);
    }

    private void UpdateProhibitedValues(Variable left, Variable right)
    {
        var leftProhibs = this.prohibitedValues[left].ProhibitedValues;
        var rightProhibs = this.prohibitedValues[right].ProhibitedValues;

        var union = leftProhibs.Union(rightProhibs);

        this.prohibitedValues.Remove(left);

        this.prohibitedValues[right] = new ProhibitedValuesBinding(union);
    }

    private void ApplySubstitutionComposition(Variable var, ISimpleTerm term)
    {
        var dictForSubstitution = new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer())
        {
            { var, term },
        };

        var newPairs = new KeyValuePair<Variable, TermBinding>[this.termBindings.Count];

        Parallel.For(0, this.termBindings.Count, index =>
        {
            var currentPair = this.termBindings.ElementAt(index);

            newPairs[index] = new KeyValuePair<Variable, TermBinding>(
                currentPair.Key, new TermBinding(currentPair.Value.Term.Substitute(dictForSubstitution)));
        });

        this.termBindings = newPairs.ToDictionary(TermFuncs.GetSingletonVariableComparer());

        this.termBindings[var] = new TermBinding(term);
    }

    private ISimpleTerm TryGetSubstitution(ISimpleTerm term)
    {
        var variableMaybe = TermFuncs.ReturnVariableOrNone(term);

        if (!variableMaybe.HasValue)
        {
            return term;
        }

        if (!this.termBindings.TryGetValue(variableMaybe.GetValueOrThrow(), out TermBinding? value))
        {
            return term;
        }

        return value.Term;
    }
}