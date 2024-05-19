using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Extensions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
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

        CoinductiveHypothesisSet postprocessedCHS = solution.ResultSet;

        var variablesInCHS = postprocessedCHS
            .Select(x => x.Term)
            .SelectMany(x => x.ExtractVariables())
            .Distinct(new VariableComparer());

        var nonInternals = solution.ResultMapping.Keys.Where(x => !x.Identifier.StartsWith('#'));

        var varsToKeep = variablesInCHS.Union(nonInternals, new VariableComparer());

        var postprocessedMapping = _mappingPostprocessor.Postprocess(solution.ResultMapping, varsToKeep);

        return new CoSLDSolution(postprocessedCHS.Select(x => x.Term), postprocessedMapping);
    }
}
