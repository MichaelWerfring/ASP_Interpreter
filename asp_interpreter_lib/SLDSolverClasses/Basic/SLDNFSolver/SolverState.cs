using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver
{
    public class SolverState
    {
        public SolverState
        (
            IEnumerable<ISimpleTerm> currentGoals,
            Dictionary<Variable, ISimpleTerm> currentSubstitution,
            int nextInternalVariable
        )
        {
            ArgumentNullException.ThrowIfNull(currentGoals);
            ArgumentNullException.ThrowIfNull(currentSubstitution);

            CurrentGoals = currentGoals;
            CurrentSubstitution = currentSubstitution;

            NextInternalVariable = nextInternalVariable;
        }

        public IEnumerable<ISimpleTerm> CurrentGoals { get; }

        public Dictionary<Variable, ISimpleTerm> CurrentSubstitution { get; }

        public int NextInternalVariable { get; }
    }
}