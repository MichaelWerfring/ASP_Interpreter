using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class StandardConstructiveUnificationAlgorithm : IConstructiveUnificationAlgorithm
{
    private bool _doOccursCheck;

    public StandardConstructiveUnificationAlgorithm(bool doOccursCheck)
    {
        _doOccursCheck = doOccursCheck;
    }

    /// <summary>
    /// Try to unify two terms, based on mapping of variables to prohibited value lists.
    /// Exceptions are thrown if mapping does not contain a value for each variable in left and right.
    /// </summary>
    public IOption<VariableMapping> Unify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        var constructiveUnifier = new ConstructiveUnifier(_doOccursCheck, target);

        return constructiveUnifier.Unify();
    }
}
