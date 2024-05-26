// <copyright file="ForallGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class ForallGoal : ICoSLDGoal, IVariableBindingArgumentVisitor<IEnumerable<GoalSolution>, GoalSolution>
{
    private readonly GoalSolver solver;
    private readonly Variable variable;
    private readonly Structure goalTerm;
    private readonly SolutionState inputState;
    private readonly ILogger logger;
    private readonly int maxSolutionCount;

    private int alreadyreturnedSolutionCount;

    public ForallGoal(
        GoalSolver solver,
        Variable variable,
        Structure goalTerm,
        SolutionState state,
        ILogger logger,
        int maxSolutionCount)
    {
        ArgumentNullException.ThrowIfNull(solver, nameof(solver));
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(goalTerm, nameof(goalTerm));
        ArgumentNullException.ThrowIfNull(state, nameof(state));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        this.solver = solver;
        this.variable = variable;
        this.goalTerm = goalTerm;
        this.inputState = state;
        this.logger = logger;

        this.alreadyreturnedSolutionCount = 0;
        this.maxSolutionCount = maxSolutionCount;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        this.logger.LogInfo($"Attempting to solve forall goal with var {this.variable}, goal {this.goalTerm}");

        var initialState = new CoSldSolverState([this.goalTerm], this.inputState);

        foreach (GoalSolution initialForallSolution in this.solver.SolveGoals(initialState))
        {
            // get binding. transitive resolving is necessary because through unification,
            // you could have something like X -> Y -> \={1,2}
            IOption<IVariableBinding> mappingForForallVariableMaybe = initialForallSolution.ResultMapping.Resolve(this.variable, true);

            if (!mappingForForallVariableMaybe.HasValue)
            {
                yield return initialForallSolution;
                yield break;
            }

            IVariableBinding mappingForForallVariable = mappingForForallVariableMaybe.GetValueOrThrow();

            // visit the variable binding type, enumerate solutions (if any).
            IEnumerable<GoalSolution> solutions = mappingForForallVariable.Accept(this, initialForallSolution);

            foreach (var solution in solutions)
            {
                if (this.alreadyreturnedSolutionCount >= this.maxSolutionCount)
                {
                    yield break;
                }

                yield return solution;

                this.alreadyreturnedSolutionCount += 1;
            }
        }
    }

    public IEnumerable<GoalSolution> Visit(ProhibitedValuesBinding binding, GoalSolution initialSolution)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(initialSolution);

        // if no prohibited values(unconstrained), then succeed.
        if (binding.ProhibitedValues.Count == 0)
        {
            yield return initialSolution;
            yield break;
        }

        // update goal with whatever we have found out about its vars during initial forall execution.
        var updatedGoal = initialSolution.ResultMapping.ApplySubstitution(this.goalTerm);

        // get the "new version" of the variable:
        // During the initial forall execution, variable might have been renamed during unification.
        var updatedVarMapping = initialSolution.ResultMapping.Resolve(this.variable, false).GetValueOrThrow();
        var updatedVar = TermFuncs.ReturnVariableOrNone
            (VarMappingFunctions.ReturnTermbindingOrNone(updatedVarMapping).GetValueOrThrow().Term).GetValueOrThrow();

        // construct new goals where variable in goalTerm is substituted by each prohibited value of variable.
        var constraintSubstitutedGoals = binding.ProhibitedValues
        .Select(prohibitedTerm => updatedGoal.Substitute(
            new Dictionary<Variable, ISimpleTerm>(TermFuncs.GetSingletonVariableComparer())
            {
                    { updatedVar, prohibitedTerm },
            }));

        // construct new solver state
        var initialSolvingState = new CoSldSolverState(
            constraintSubstitutedGoals,
            new SolutionState(
                initialSolution.Stack,
                initialSolution.ResultSet,
                initialSolution.ResultMapping
,
                initialSolution.NextInternalVariable));

        IEnumerable<GoalSolution> solutions = this.solver.SolveGoals(initialSolvingState);

        foreach (var solution in solutions)
        {
            var newSolution = new GoalSolution(
                solution.ResultSet,
                solution.ResultMapping.SetItem(this.variable, new ProhibitedValuesBinding()),
                solution.Stack,
                solution.NextInternalVariable);

            yield return newSolution;
        }
    }

    public IEnumerable<GoalSolution> Visit(TermBinding binding, GoalSolution initialSolution)
    {
        ArgumentNullException.ThrowIfNull(binding);
        ArgumentNullException.ThrowIfNull(initialSolution);

        yield break;
    }
}