using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Postprocessing;

internal class SolutionPostprocessor
{
    private readonly VariableMappingPostprocessor _mappingPostprocessor;

    private readonly CHSPostProcessor _chsPostprocessor;

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

        var variablesInCHS = postprocessedCHS
            .Select(x => x.Term)
            .SelectMany(x => x.ExtractVariables())
            .Distinct(TermFuncs.GetSingletonVariableComparer());

        var nonInternals = solution.ResultMapping.Keys.Where(x => !x.Identifier.StartsWith('#'));

        var varsToKeep = variablesInCHS.Union(nonInternals, TermFuncs.GetSingletonVariableComparer());

        var postprocessedMapping = _mappingPostprocessor.Postprocess(solution.ResultMapping, varsToKeep);

        return new CoSLDSolution(postprocessedCHS.Select(x => x.Term), postprocessedMapping);
    }
}
