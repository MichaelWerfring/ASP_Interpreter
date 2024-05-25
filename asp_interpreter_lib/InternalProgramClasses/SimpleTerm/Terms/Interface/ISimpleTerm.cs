namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

public interface ISimpleTerm
{
    public void Accept(ISimpleTermVisitor visitor);

    public T Accept<T>(ISimpleTermVisitor<T> visitor);

    public void Accept<TArgs>(ISimpleTermArgsVisitor<TArgs> visitor, TArgs arguments);

    public TResult Accept<TResult, TArgs>(ISimpleTermArgsVisitor<TResult, TArgs> visitor, TArgs arguments);

    public abstract string ToString();

}
