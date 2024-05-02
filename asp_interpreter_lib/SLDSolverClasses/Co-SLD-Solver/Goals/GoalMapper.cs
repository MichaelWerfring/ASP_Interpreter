using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals.GoalBuilders;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Goals;

public class GoalMapper : ISimpleTermArgsVisitor<IOption<ICoSLDGoal>, CoSldSolverState>
{
    private IDictionary<(string, int), IGoalBuilder> _mapping;

    public GoalMapper(IDictionary<(string, int), IGoalBuilder> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        _mapping = mapping;
    }

    public IOption<ICoSLDGoal> GetGoal(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<ICoSLDGoal> Visit(Variable variableTerm)
    {
        return new None<ICoSLDGoal>();
    }

    public IOption<ICoSLDGoal> Visit(Structure basicTerm)
    {
        IGoalBuilder? goalBuilder;
        _mapping.TryGetValue((basicTerm.Functor, basicTerm.Children.Count()), out goalBuilder);

        if(goalBuilder != null)
        {
            goalBuilder.BuildGoal()      
        }

    }

    public IOption<ICoSLDGoal> Visit(Integer integer)
    {
        return new None<ICoSLDGoal>();
    }

    public IOption<ICoSLDGoal> Visit(Variable variableTerm, CoSldSolverState arguments)
    {
        throw new NotImplementedException();
    }

    public IOption<ICoSLDGoal> Visit(Structure basicTerm, CoSldSolverState arguments)
    {
        throw new NotImplementedException();
    }

    public IOption<ICoSLDGoal> Visit(Integer integer, CoSldSolverState arguments)
    {
        throw new NotImplementedException();
    }
}
