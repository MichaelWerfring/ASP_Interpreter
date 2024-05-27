// <copyright file="ConstructiveDisunifier.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Util.ErrorHandling.Either;
using System.Collections.Immutable;

/// <summary>
/// An instance-based disunification algorithm,
/// to provide the algorithm with a context through its fields.
/// </summary>
public class ConstructiveDisunifier : IBinaryTermCaseVisitor
{
    // input by constructor
    private readonly bool doGroundednessCheck;
    private readonly bool doDisunifyUnboundVariables;

    private readonly ISimpleTerm left;
    private readonly ISimpleTerm right;

    private readonly IImmutableDictionary<Variable, ProhibitedValuesBinding> prohibitedValues;

    // mutated during execution.
    // disunifier mapping
    private readonly List<DisunificationResult> disunifiers;

    // flags
    private bool doesNotUnifyAnways;
    private DisunificationException? disunificationError;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConstructiveDisunifier"/> class.
    /// </summary>
    /// <param name="doGroundednessCheck">Whether to do a groundedness check in cases of variable-term.</param>
    /// <param name="doDisunifyUnboundVariables">Whether to disunify a variable-variable case.</param>
    /// <param name="target">The target to disunify.</param>
    /// <exception cref="ArgumentNullException">Thrown if target is null.</exception>
    public ConstructiveDisunifier(
        bool doGroundednessCheck,
        bool doDisunifyUnboundVariables,
        ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        this.doGroundednessCheck = doGroundednessCheck;
        this.doDisunifyUnboundVariables = doDisunifyUnboundVariables;
        this.left = target.Left;
        this.right = target.Right;
        this.prohibitedValues = target.Mapping;

        this.disunifiers =[];

        this.doesNotUnifyAnways = false;
        this.disunificationError = null;

        this.disunifiers =[];
    }

    /// <summary>
    /// Attempts to disunify the target contained in this instance.
    /// </summary>
    /// <returns>Either a disunification exception or an enumerable of disunification results.</returns>
    public IEither<DisunificationException, IEnumerable<DisunificationResult>> Disunify()
    {
        this.TryDisunify(this.left, this.right);

        // if we encountered a fatal error, such as two variables disunifying, or occurs check.
        if (this.disunificationError != null)
        {
            return new Left<DisunificationException, IEnumerable<DisunificationResult>>(this.disunificationError);
        }

        // if they wouldnt unify anyway
        if (this.doesNotUnifyAnways)
        {
            return new Right<DisunificationException, IEnumerable<DisunificationResult>>([]);
        }

        // if there is no way that they can disunify
        if (this.disunifiers.Count == 0)
        {
            return new Left<DisunificationException, IEnumerable<DisunificationResult>>(
                new CannotDisunifyException($"Terms {this.left} and {this.right} cannot be disunified."));
        }

        // filter for values that are already prohibited anyways
        IEnumerable<DisunificationResult> filteredDisunifiers = this.disunifiers.AsParallel().Where(disunifier =>
        {
            if (disunifier.IsInstantiation)
            {
                return true;
            }

            if (this.prohibitedValues[disunifier.Variable].ProhibitedValues.Contains(disunifier.Term))
            {
                return false;
            }

            return true;
        });

        return new Right<DisunificationException, IEnumerable<DisunificationResult>>(filteredDisunifiers);
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

    private void TryDisunify(ISimpleTerm left, ISimpleTerm right)
    {
        // check if mismatch encountered or error
        if (this.doesNotUnifyAnways || this.disunificationError != null)
        {
            return;
        }

        // determine case and continue based on case.
        IBinaryTermCase typeCase = TermFuncs.DetermineCase(left, right);

        typeCase.Accept(this);
    }

    private void ResolveStructureStructureCase(IStructure leftStruct, IStructure rightStruct)
    {
        IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> reductionMaybe = TermFuncs.Reduce(leftStruct, rightStruct);

        if (!reductionMaybe.HasValue)
        {
            this.doesNotUnifyAnways = true;
            return;
        }

        IEnumerable<(ISimpleTerm, ISimpleTerm)> reduction = reductionMaybe.GetValueOrThrow();

        foreach (var pair in reduction)
        {
            this.TryDisunify(pair.Item1, pair.Item2);
        }
    }

    private void ResolveVariableStructureCase(Variable left, IStructure right)
    {
        // do groundedness check if asked for
        if (this.doGroundednessCheck && right.ExtractVariables().Any())
        {
            this.disunificationError = new NonGroundTermException(
                $"Cannot disunify variable and nonground term: {left} and {right}");
            return;
        }

        IEnumerable<DisunificationResult> negativesInvolvingLeft = this.disunifiers
            .Where(disunifier => !disunifier.IsInstantiation && disunifier.Variable.IsEqualTo(left));

        // if any maps to another term already,
        // then it wont unify anyway.
        if (negativesInvolvingLeft.Any(disunifier => !disunifier.Term.IsEqualTo(right)))
        {
            this.doesNotUnifyAnways = true;
            return;
        }

        // if there is one with the same term already,
        // then dont add this one to avoid duplicates.
        if (negativesInvolvingLeft.Any(disunifier => disunifier.Term.IsEqualTo(right)))
        {
            return;
        }

        this.disunifiers.Add(new DisunificationResult(left, right, false));
    }

    private void ResolveVariableVariableCase(Variable left, Variable right)
    {
        if (!this.doDisunifyUnboundVariables)
        {
            this.disunificationError = new VariableDisunificationException(
                $"Cannot disunify two variables: {left} and {right}");
            return;
        }

        var leftProhibitedValues = this.prohibitedValues[left].ProhibitedValues;
        var rightProhibitedValues = this.prohibitedValues[right].ProhibitedValues;

        var difference = leftProhibitedValues.Union(rightProhibitedValues)
                        .Except(leftProhibitedValues.Intersect(rightProhibitedValues));

        foreach (var term in difference)
        {
            if (this.doGroundednessCheck && term.ExtractVariables().Any())
            {
                this.disunificationError = new NonGroundTermException(
                    $"Cannot disunify variable and nonground term: {left} and {right}");
                return;
            }

            if (leftProhibitedValues.Contains(term))
            {
                this.disunifiers.Add(new DisunificationResult(right, term, true));
            }
            else
            {
                this.disunifiers.Add(new DisunificationResult(left, term, true));
            }
        }
    }
}