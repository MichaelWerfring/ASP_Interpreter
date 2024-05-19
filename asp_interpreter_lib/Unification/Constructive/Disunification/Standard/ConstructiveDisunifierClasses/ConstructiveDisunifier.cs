using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.Unification.StructureReducers;
using System.Collections.Immutable;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

/// <summary>
/// An instance-based disunification algorithm,
/// as to provide the algorithm with a context through its fields.
/// </summary>
public class ConstructiveDisunifier
{
    // functions
    private readonly StructureReducer _reducer = new();

    // input by constructor
    private readonly bool _doGroundednessCheck;
    private readonly bool _doDisunifyUnboundVariables;
    private readonly ISimpleTerm _left;
    private readonly ISimpleTerm _right;
    private readonly IImmutableDictionary<Variable, ProhibitedValuesBinding> _prohibitedValues;

    // disunifier mapping
    private readonly List<DisunificationResult> _disunifiers = [];

    // flags
    private bool _dontUnifyAnyway = false;
    private DisunificationException? _fatalError = null;

    public ConstructiveDisunifier
    (
        bool doGroundednessCheck, 
        bool doDisunifyUnboundVariables, 
        ISimpleTerm left, 
        ISimpleTerm right, 
        IImmutableDictionary<Variable, ProhibitedValuesBinding> mapping
    )
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        _doGroundednessCheck = doGroundednessCheck;
        _doDisunifyUnboundVariables = doDisunifyUnboundVariables;
        _left = left;
        _right = right;
        _prohibitedValues = mapping;
    }

    public IEither<DisunificationException, IEnumerable<DisunificationResult>> Disunify()
    {
        TryDisunify(_left, _right);

        // if we encountered a fatal error, such as two variables disunifying, or occurs check.
        if (_fatalError != null)
        {
            return new Left<DisunificationException, IEnumerable<DisunificationResult>>
                (_fatalError);
        }

        // if they wouldnt unify anyway
        if (_dontUnifyAnyway)
        {
            return new Right<DisunificationException, IEnumerable<DisunificationResult>>([]);
        }

        // if there is no way that they can disunify
        if (_disunifiers.Count == 0)
        {
            return new Left<DisunificationException, IEnumerable<DisunificationResult>>
                (new CannotDisunifyException($"Terms {_left} and {_right} cannot be disunified."));
        }

        // filter for values that are already prohibited anyways
        IEnumerable<DisunificationResult> filteredDisunifiers = _disunifiers.AsParallel().Where(disunifier =>
        {
            if (disunifier.IsPositive) { return true; };

            if (_prohibitedValues[disunifier.Variable].ProhibitedValues.Contains(disunifier.Term))
            {
                return false;
            }

            return true;
        });

        return new Right<DisunificationException, IEnumerable<DisunificationResult>>(filteredDisunifiers);
    }

    private void TryDisunify(ISimpleTerm left, ISimpleTerm right)
    {
        // check if mismatch encountered or error
        if (_dontUnifyAnyway || _fatalError != null)
        {
            return; 
        }

        // determine case
        if (left is Variable leftVar)
        {
            if (right is Variable rightVar)
            {
                LeftIsVarRightIsVar(leftVar, rightVar);
            }
            else if (right is IStructure rightStruct)
            {
                LeftIsVarRightIsStruct(leftVar, rightStruct);
            }
            else
            {
                throw new ArgumentException("The type hierarchy has been modifie so that not every term is either a variable or a structure!");
            }
        }
        else if (left is IStructure leftStruct)
        {
            if (right is Variable rightVar)
            {
                LeftIsStructRightIsVar(leftStruct, rightVar);
            }
            else if (right is IStructure rightStruct)
            {
                LeftIsStructRightIsStruct(leftStruct, rightStruct);
            }
            else
            {
                throw new ArgumentException("The type hierarchy has been modifie so that not every term is either a variable or a structure!");
            }
        }
        else
        {
            throw new ArgumentException("The type hierarchy has been modifie so that not every term is either a variable or a structure!");
        }
    }

    // cases
    private void LeftIsStructRightIsStruct(IStructure leftStruct, IStructure rightStruct)
    {
        IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> reductionMaybe = 
            _reducer.TryReduce(leftStruct, rightStruct);

        if (!reductionMaybe.HasValue)
        {
            _dontUnifyAnyway = true;
            return;
        }

        IEnumerable<(ISimpleTerm, ISimpleTerm)> reduction = reductionMaybe.GetValueOrThrow();

        foreach (var pair in reduction)
        {
            TryDisunify(pair.Item1, pair.Item2);
        }
    }

    private void LeftIsStructRightIsVar(IStructure left, Variable right)
    {
        LeftIsVarRightIsStruct(right, left);
    }

    private void LeftIsVarRightIsStruct(Variable left, IStructure right)
    {
        // do groundedness check if asked for
        if (_doGroundednessCheck && right.ExtractVariables().Any())
        {
            _fatalError = new NonGroundTermException
                ($"Cannot disunify variable and nonground term: {left} and {right}");
            return;
        }

        IEnumerable<DisunificationResult> negativesInvolvingLeft = _disunifiers
            .Where(disunifier => !disunifier.IsPositive && disunifier.Variable.IsEqualTo(left));

        // if any maps to another term already,
        // then it wont unify anyway.
        if (negativesInvolvingLeft.Any(disunifier => !disunifier.Term.IsEqualTo(right)))
        {
            _dontUnifyAnyway = true;
            return;
        }

        // if there is one with the same term already,
        // then dont add this one to avoid duplicates.
        if (negativesInvolvingLeft.Any(disunifier => disunifier.Term.IsEqualTo(right)))
        {
            return;
        }

        _disunifiers.Add(new DisunificationResult(left, right, false));
    }

    private void LeftIsVarRightIsVar(Variable left, Variable right)
    {
        if (!_doDisunifyUnboundVariables)
        {
            _fatalError = new VariableDisunificationException
                ($"Cannot disunify two variables: {left} and {right}");
            return;
        }

        var leftProhibitedValues = _prohibitedValues[left].ProhibitedValues;
        var rightProhibitedValues = _prohibitedValues[right].ProhibitedValues;

        var difference = leftProhibitedValues.Union(rightProhibitedValues)
                        .Except(leftProhibitedValues.Intersect(rightProhibitedValues));

        foreach (var term in difference)
        {
            if (_doGroundednessCheck && term.ExtractVariables().Any())
            {
                _fatalError = new NonGroundTermException
                    ($"Cannot disunify variable and nonground term: {left} and {right}");
                return;
            }

            if (leftProhibitedValues.Contains(term))
            {
                _disunifiers.Add(new DisunificationResult(right, term, true));
            }
            else
            {
                _disunifiers.Add(new DisunificationResult(left, term, true));
            }
        }
    }
}
