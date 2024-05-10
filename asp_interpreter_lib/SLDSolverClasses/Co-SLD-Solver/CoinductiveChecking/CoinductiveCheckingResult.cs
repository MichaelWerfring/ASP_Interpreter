using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.CoinductivChecking.CoinductivityChecking;

public enum SuccessType { DeterministicSuccess, NonDeterministicSuccess, NoMatch };

public class CoinductiveCheckingResult
{
    public CoinductiveCheckingResult(Structure constrainedTarget, VariableMapping constrainedMapping, SuccessType successType) 
    {
        ArgumentNullException.ThrowIfNull(constrainedTarget, nameof(constrainedTarget));
        ArgumentNullException.ThrowIfNull(constrainedMapping, nameof(constrainedMapping));

        ConstrainedTarget = constrainedTarget;
        ConstrainedMapping = constrainedMapping;
        SuccessType = successType;
    }

    public Structure ConstrainedTarget { get; }

    public VariableMapping ConstrainedMapping { get; }

    public SuccessType SuccessType { get; }
}