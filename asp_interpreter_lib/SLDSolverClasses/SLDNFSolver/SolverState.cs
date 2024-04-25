using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver
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