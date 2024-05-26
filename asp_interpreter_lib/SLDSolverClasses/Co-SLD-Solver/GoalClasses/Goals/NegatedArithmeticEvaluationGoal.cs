// <copyright file="NegatedArithmeticEvaluationGoal.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.GoalClasses.Goals;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.ArithmeticSolver;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions.Extensions;
using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Disunification;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Unification.Constructive.Target.Builder;
using Asp_interpreter_lib.Util.ErrorHandling;

internal class NegatedArithmeticEvaluationGoal : ICoSLDGoal, ISimpleTermArgsVisitor<IOption<GoalSolution>, int>
{
    private readonly ArithmeticEvaluator evaluator;
    private readonly ISimpleTerm left;
    private readonly ISimpleTerm right;
    private readonly SolutionState state;
    private readonly IConstructiveDisunificationAlgorithm algorithm;
    private readonly ILogger logger;

    public NegatedArithmeticEvaluationGoal(
        ArithmeticEvaluator evaluator,
        ISimpleTerm left,
        ISimpleTerm right,
        SolutionState state,
        IConstructiveDisunificationAlgorithm algorithm,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(evaluator);
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(algorithm);
        ArgumentNullException.ThrowIfNull(logger);

        this.evaluator = evaluator;
        this.left = left;
        this.right = right;
        this.state = state;
        this.algorithm = algorithm;
        this.logger = logger;
    }

    public IEnumerable<GoalSolution> TrySatisfy()
    {
        this.logger.LogInfo($"Attempting to solve negated arithmetic evaluation goal: {this.left}, {this.right}");
        this.logger.LogTrace($"Input state is: {this.state}");

        IOption<int> rightEvalMaybe = this.evaluator.Evaluate(this.right);

        int rightEval;
        try
        {
            rightEval = rightEvalMaybe.GetValueOrThrow();
        }
        catch
        {
            yield break;
        }

        var solutionMaybe = this.left.Accept(this, rightEval);
        if (!solutionMaybe.HasValue)
        {
            yield break;
        }

        this.logger.LogInfo($"Solved negated arithmetic evaluation goal: {this.left}, {this.right}");

        yield return solutionMaybe.GetValueOrThrow();
    }

    public IOption<GoalSolution> Visit(Variable var, int integer)
    {
        ArgumentNullException.ThrowIfNull(integer, nameof(integer));

        var targetEither = ConstructiveTargetBuilder.Build(var, new Integer(integer), this.state.Mapping);
        if (!targetEither.IsRight)
        {
            this.logger.LogError(targetEither.GetLeftOrThrow().Message);
            throw new ArgumentException(nameof(this.state));
        }

        ConstructiveTarget target = targetEither.GetRightOrThrow();

        var resultEither = this.algorithm.Disunify(target);
        if (!resultEither.IsRight)
        {
            return new None<GoalSolution>();
        }

        VariableMapping disunifyingMapping = resultEither.GetRightOrThrow().First();
        this.logger.LogTrace($"Disunifying mapping is {disunifyingMapping}");

        VariableMapping newMapping = this.state.Mapping.Update(resultEither.GetRightOrThrow().First()).GetValueOrThrow();
        this.logger.LogTrace($"New mapping is {newMapping}");

        return new Some<GoalSolution>(
            new GoalSolution(
                this.state.CHS,
                newMapping,
                this.state.Callstack,
                this.state.NextInternalVariableIndex));
    }

    public IOption<GoalSolution> Visit(Structure structure, int integer)
    {
        ArgumentNullException.ThrowIfNull(structure);

        return new None<GoalSolution>();
    }

    public IOption<GoalSolution> Visit(Integer integer, int rightEval)
    {
        ArgumentNullException.ThrowIfNull(integer, nameof(integer));

        if (integer.Value == rightEval)
        {
            return new None<GoalSolution>();
        }

        return new Some<GoalSolution>(
            new GoalSolution(
                this.state.CHS,
                this.state.Mapping,
                this.state.Callstack,
                this.state.NextInternalVariableIndex));
    }
}