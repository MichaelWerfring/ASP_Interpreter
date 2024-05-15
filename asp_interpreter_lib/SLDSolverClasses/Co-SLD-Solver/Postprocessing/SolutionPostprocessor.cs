using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

internal class SolutionPostprocessor
{
    private VariableMappingPostprocessor _mappingPostprocessor;

    private CHSPostProcessor _chsPostprocessor;

    public SolutionPostprocessor(VariableMappingPostprocessor mappingProcessor, CHSPostProcessor chsProcessor)
    {
        ArgumentNullException.ThrowIfNull(mappingProcessor, nameof(mappingProcessor));
        ArgumentNullException.ThrowIfNull(chsProcessor, nameof(chsProcessor));

        _mappingPostprocessor = mappingProcessor;
        _chsPostprocessor = chsProcessor;
    }

    public CoSLDSolution Postprocess(GoalSolution solution)
    {
        ArgumentNullException.ThrowIfNull(solution, nameof(solution));

        CoinductiveHypothesisSet postprocessedCHS = _chsPostprocessor.Postprocess(solution.ResultSet);

        VariableMapping postprocessedMapping = _mappingPostprocessor.Postprocess(solution.ResultMapping);

        return new CoSLDSolution(postprocessedCHS.Select(x => x.Term), postprocessedMapping);
    }
}
