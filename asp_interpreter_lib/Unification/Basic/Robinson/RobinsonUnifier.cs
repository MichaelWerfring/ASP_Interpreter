using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.Unification.Basic.Robinson;

public class RobinsonUnifier
{
    private SimpleTermComparer _comparer = new SimpleTermComparer();
    private VariableSubstituter _substituter = new VariableSubstituter();
    private SimpleTermContainsChecker _containsChecker = new SimpleTermContainsChecker();
    private StructureReducer _reducer = new StructureReducer();

    private bool _doOccursCheck;

    private Dictionary<Variable, ISimpleTerm> _substitution;
    private bool _hasSucceded;

    public RobinsonUnifier(bool doOccursCheck)
    {
        _doOccursCheck = doOccursCheck;

        _substitution = new Dictionary<Variable, ISimpleTerm>(new VariableComparer());
        _hasSucceded = true;
    }

    public IOption<Dictionary<Variable, ISimpleTerm>> Unify(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        TryUnify(left, right);

        if (!_hasSucceded)
        {
            return new None<Dictionary<Variable, ISimpleTerm>>();
        }

        return new Some<Dictionary<Variable, ISimpleTerm>>(_substitution);
    }

    private void TryUnify(ISimpleTerm left, ISimpleTerm right)
    {
        if (!_hasSucceded) { return; }

        ISimpleTerm currentLeft = left;
        ISimpleTerm currentRight = right;
        if (left is Variable leftVar)
        {
            currentLeft = GetSubstitutionOrDefault(leftVar, _substitution);
        }
        if (right is Variable rightVar)
        {
            currentRight = GetSubstitutionOrDefault(rightVar, _substitution);
        }

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
                " so that not every term is either a variable or a structure!",
                nameof(currentLeft)
            );
        }
    }

    private void TryUnifyStructureCase(IStructure structure, ISimpleTerm other)
    {
        // if both are structures
        if (other is IStructure b)
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
        // left is structure and right variable
        else
        {
            TryUnify(other, structure);
        }
    }

    private void TryUnifyVariableCase(Variable left, ISimpleTerm right)
    {
        // if both are variables and are equal
        if (_comparer.Equals(left, right))
        {
            return;
        }
        // now we know that left is variable : do occurs check if necessary
        else if (_doOccursCheck && _containsChecker.LeftContainsRight(right, left))
        {
            _hasSucceded = false;
            return;
        }
        // now do substitution composition
        else
        {
            var newDict = new Dictionary<Variable, ISimpleTerm>(new VariableComparer())
            {
                { left, right }
            };
            _substitution = ApplySubstitutionComposition(_substitution, newDict);
        }
    }

    private Dictionary<Variable, ISimpleTerm> ApplySubstitutionComposition
    (
        Dictionary<Variable, ISimpleTerm> oldSubstitution,
        Dictionary<Variable, ISimpleTerm> substitution
    )
    {
        return oldSubstitution.Select(keyValuePair => (keyValuePair.Key, _substituter.Substitute(keyValuePair.Value, substitution)))
                              .ToDictionary(new VariableComparer())
                              .Union(substitution)
                              .ToDictionary(new VariableComparer());
    }

    private ISimpleTerm GetSubstitutionOrDefault
    (
        Variable variable,
        Dictionary<Variable, ISimpleTerm> substitution
    )
    {
        ISimpleTerm? potentialSub;

        substitution.TryGetValue(variable, out potentialSub);

        if (potentialSub == null)
        {
            return variable;
        }

        return potentialSub;
    }
}
