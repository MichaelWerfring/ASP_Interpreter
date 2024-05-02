using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.Unification.Basic.Robinson;
using asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.Util.ErrorHandling.Either;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

public class ConstructiveDisunifier
{
    // functions
    private StructureReducer _reducer = new StructureReducer();
    private SimpleTermFlattener _flattener = new SimpleTermFlattener();
    private SimpleTermComparer _comparer = new SimpleTermComparer();

    // input by constructor
    private bool _doGroundednessCheck;
    private bool _doDisunifyUnboundVariables;
    private ConstructiveTarget _target;

    // modified during execution :

    //  flags
    private bool _mismatch;
    private DisunificationException? _fatalError;

    //  disunifier mapping
    private List<VariableDisunifier> _disunifiers;


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
        _disunifiers = new List<VariableDisunifier>();
    }

    public IEither<DisunificationException, IEnumerable<VariableDisunifier>> Disunify()
    {
        TryDisunify(_target.Left, _target.Right);

        if (_fatalError != null)
        {
            return new Left<DisunificationException, IEnumerable<VariableDisunifier>>
                (_fatalError);
        }

        if (_mismatch)
        {
            return new Right<DisunificationException, IEnumerable<VariableDisunifier>>
                (new List<VariableDisunifier>());
        }

        if (_disunifiers.Count() == 0)
        {
            return new Left<DisunificationException, IEnumerable<VariableDisunifier>>
                (new CannotDisunifyException($"Terms {_target.Left.ToString()} and {_target.Right.ToString()} cannot be disunified."));
        }

        return new Right<DisunificationException, IEnumerable<VariableDisunifier>>(_disunifiers);
    }

    private void TryDisunify(ISimpleTerm left, ISimpleTerm right)
    {
        if (_mismatch || _fatalError != null) { return; }

        // continue based on case of left.
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
                ($"Cannot disunify two variables: {left.ToString()} and {right.ToString()}");
            return;
        }

        if (right is Variable rightVar)
        {
            DisunifyVariables(left, rightVar);
            return;
        }

        // do groundedness check if asked for
        if (_doGroundednessCheck && _flattener.FlattenToList(right).Any(x => x is Variable))
        {
            _fatalError = new NonGroundTermException
                ($"Cannot disunify variable and nonground term: {left.ToString()} and {right.ToString()}");
            return;
        }

        // if the variable already maps to a different value, then we have a mismatch.
        if (_disunifiers.Any
        (
            map => !map.IsPositive
            &&
            _comparer.Equals(map.Variable, left)
            && 
            !_comparer.Equals(map.Term, right)) 
        )
        {
            _mismatch = true;
            return;
        }

        _disunifiers.Add(new VariableDisunifier(left, right, false));
    }

    private void DisunifyVariables(Variable left, Variable right)
    {
        var leftProhibitedValues = _target.Mapping[left].ProhibitedValues;
        var rightProhibitedValues = _target.Mapping[right].ProhibitedValues;

        var union = leftProhibitedValues.Union(rightProhibitedValues, _comparer);
        var intersect = leftProhibitedValues.Intersect(rightProhibitedValues, _comparer);
        var difference = union.Except(intersect, _comparer);

        foreach (var value in difference)
        {
            if (_doGroundednessCheck && _flattener.FlattenToList(value).Any(x => x is Variable))
            {
                _fatalError = new NonGroundTermException
                    ($"Cannot disunify variable and nonground term: {left.ToString()} and {right.ToString()}");
                return;
            }

            if (leftProhibitedValues.Contains(value, _comparer))
            {
                _disunifiers.Add(new VariableDisunifier(right, value, true));
            }
            else
            {
                _disunifiers.Add(new VariableDisunifier(left, value, true));
            }
        }
    }
}
