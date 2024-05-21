using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

public class VariableVariableCase : IBinaryTermCase
{
    public VariableVariableCase(Variable left, Variable right)
    {
        Left = left;
        Right = right;
    }

    public Variable Left { get; }

    public Variable Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }
}