using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;

public interface IInternalTerm
{
    public void Accept(IInternalTermVisitor visitor);

    public T Accept<T>(IInternalTermVisitor<T> visitor);

    public TResult Accept<TResult, TArgs>(IInternalTermVisitor<TResult, TArgs> visitor, TArgs arguments);
}
