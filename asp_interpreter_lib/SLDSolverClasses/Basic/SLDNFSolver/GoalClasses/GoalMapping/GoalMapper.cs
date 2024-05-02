using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.Goals.Unification;
using asp_interpreter_lib.Unification.Basic.Robinson;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver.GoalClasses.GoalMapping;

public class GoalMapper : ISimpleTermVisitor<IOption<IGoal>>
{
    private IDictionary<(string, int), IGoal> _mapping;

    public GoalMapper(IDictionary<(string, int), IGoal> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);

        _mapping = mapping;
    }

    public IOption<IGoal> GetGoal(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<IGoal> Visit(Variable variableTerm)
    {
        return new None<IGoal>();
    }

    public IOption<IGoal> Visit(Structure basicTerm)
    {
        IGoal? goal;
        _mapping.TryGetValue((basicTerm.Functor, basicTerm.Children.Count()), out goal);
        if (goal == null)
        {
            return new Some<IGoal>(new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false)));
        }

        return new Some<IGoal>(goal);
    }

    public IOption<IGoal> Visit(Integer integer)
    {
        return new None<IGoal>();
    }
}