namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

public interface IBinaryTermCase
{
    public void Accept(IBinaryTermCaseVisitor visitor);

    public T Accept<T>(IBinaryTermCaseVisitor<T> visitor);
}