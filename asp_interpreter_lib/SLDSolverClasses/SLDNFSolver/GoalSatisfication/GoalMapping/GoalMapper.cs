using asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Arithmetics;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Comparison;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Negation;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.SASP;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.Unification;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.Goals.ArithmeticEvaluation;
using asp_interpreter_lib.Unification.Robinson;

namespace asp_interpreter_lib.SLDSolverClasses.SLDNFSolver.GoalSatisfication.GoalMapping;

public class GoalMapper: ISimpleTermVisitor<IGoal>
{
    public IGoal GetGoal(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IGoal Visit(Variable variableTerm)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Structure basicTerm)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Division divide)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Multiplication multiply)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Addition plus)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Subtraction subtract)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(GreaterThan greaterThan)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(GreaterThanOrEqual greaterThanOrEqual)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(LessOrEqualThan lessOrEqualThan)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(LessThan lessThan)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(List list)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Parenthesis parenthesis)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Evaluation evaluation)
    {
        return new ArithmeticEvaluationGoal();
    }

    public IGoal Visit(Integer integer)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Nil nil)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(ClassicalNegation classicalNegation)
    {
        return new DatabaseUnificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(Naf naf)
    {
        return new NafGoal(new GoalResolver(new GoalMapper()));
    }

    public IGoal Visit(ForAll forAll)
    {
        throw new NotImplementedException();
    }

    public IGoal Visit(DisunificationStructure disunificationStructure)
    {
        return new DisunificationGoal(new RobinsonUnificationAlgorithm(false));
    }

    public IGoal Visit(UnificationStructure unificationStructure)
    {
        return new UnificationGoal(new RobinsonUnificationAlgorithm(false));
    }
}