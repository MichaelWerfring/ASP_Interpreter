namespace asp_interpreter_lib.Unification.Constructive.CaseDetermination.Cases;

public interface IBinaryTermCase
{
    public void Accept(IBinaryTermCaseVisitor visitor);
}