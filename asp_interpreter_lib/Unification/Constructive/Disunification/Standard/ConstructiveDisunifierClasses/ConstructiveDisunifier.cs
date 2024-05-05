using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util.ErrorHandling.Either;
using asp_interpreter_lib.Unification.Constructive.Target;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.Unification.StructureReducers;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

public class ConstructiveDisunifier
{
    // functions
    private readonly StructureReducer _reducer = new StructureReducer();

    // input by constructor
    private readonly bool _doGroundednessCheck;
    private readonly bool _doDisunifyUnboundVariables;
    private readonly ConstructiveTarget _target;

    // modified during execution :
    //  flags
    private bool _mismatch;
    private DisunificationException? _fatalError;

    //  disunifier mapping
    private List<DisunificationResult> _disunifiers;

    public ConstructiveDisunifier(bool doGroundednessCheck, bool doDisunifyUnboundVariables, ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        _doGroundednessCheck = doGroundednessCheck;
        _doDisunifyUnboundVariables = doDisunifyUnboundVariables;
        _target = target;

        // set flags 
        _mismatch = false;
        _fatalError = null;

        //initialize mapping
        _disunifiers = new List<DisunificationResult>();
    }

    public IEither<DisunificationException, IEnumerable<DisunificationResult>> Disunify()
    {
        TryDisunify(_target.Left, _target.Right);

        if (_fatalError != null)
        {
            return new Left<DisunificationException, IEnumerable<DisunificationResult>>
                (_fatalError);
        }

        if (_mismatch)
        {
            return new Right<DisunificationException, IEnumerable<DisunificationResult>>
                (new List<DisunificationResult>());
        }

        if (_disunifiers.Count() == 0)
        {
            return new Left<DisunificationException, IEnumerable<DisunificationResult>>
                (new CannotDisunifyException($"Terms {_target.Left} and {_target.Right} cannot be disunified."));
        }

        _disunifiers = _disunifiers.Where(disunifier =>
        {
            if (disunifier.IsPositive) { return true; };

            if (_target.Mapping[disunifier.Variable].ProhibitedValues.Contains(disunifier.Term))
            {
                return false;
            }

            return true;
        }).ToList();

        return new Right<DisunificationException, IEnumerable<DisunificationResult>>(_disunifiers);
    }

    private void TryDisunify(ISimpleTerm left, ISimpleTerm right)
    {
        if (_mismatch || _fatalError != null) { return; }

        // continue based on case of left.sag 
        if (left is Variable variable)
        {
            TryUnifyVariableCase(variable, right);
        }
        else if (left is IStructure structure)
        {
            TryUnifyStructureCase(structure, right);
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
                _mismatch = true;
                return;
            }

            var reduction = reductionMaybe.GetValueOrThrow();

            foreach (var pair in reduction)
            {
                TryDisunify(pair.Item1, pair.Item2);
            }
        }
        else // left is structure and right variable
        {
            TryDisunify(other, structure);
        }
    }

    private void TryUnifyVariableCase(Variable left, ISimpleTerm right)
    {
        if (!_doDisunifyUnboundVariables && right is Variable)
        {
            _fatalError = new VariableDisunificationException
                ($"Cannot disunify two variables: {left} and {right}");
            return;
        }

        if (right is Variable rightVar)
        {
            DisunifyVariables(left, rightVar);
            return;
        }

        // do groundedness check if asked for
        if (_doGroundednessCheck && right.ToList().Any(x => x is Variable))
        {
            _fatalError = new NonGroundTermException
                ($"Cannot disunify variable and nonground term: {left} and {right}");
            return;
        }

        // if the variable already maps to a different term, then we have a mismatch.
        if (_disunifiers.Any
        (
            map => !map.IsPositive
            &&
            map.Variable.IsEqualTo(left)
            && 
            !map.Term.IsEqualTo(right)) 
        )
        {
            _mismatch = true;
            return;
        }

        _disunifiers.Add(new DisunificationResult(left, right, false));
    }

    private void DisunifyVariables(Variable left, Variable right)
    {
        var comparer = new SimpleTermEqualityComparer();

        var leftProhibitedValues = _target.Mapping[left].ProhibitedValues;
        var rightProhibitedValues = _target.Mapping[right].ProhibitedValues;

        var union = leftProhibitedValues.Union(rightProhibitedValues, comparer);
        var intersect = leftProhibitedValues.Intersect(rightProhibitedValues, comparer);
        var difference = union.Except(intersect, comparer);

        foreach (var term in difference)
        {
            if (_doGroundednessCheck && term.ToList().Any(x => x is Variable))
            {
                _fatalError = new NonGroundTermException
                    ($"Cannot disunify variable and nonground term: {left} and {right}");
                return;
            }

            if (leftProhibitedValues.Contains(term, comparer))
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
