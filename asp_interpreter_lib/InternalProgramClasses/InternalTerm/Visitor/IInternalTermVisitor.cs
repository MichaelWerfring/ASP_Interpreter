using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;

public interface IInternalTermVisitor
{
    public void Visit(Variable variableTerm);

    public void Visit(Structure basicTerm);

    public void Visit(Integer integer);
}

public interface IInternalTermVisitor<T>
{
    public T Visit(Variable variableTerm);

    public T Visit(Structure basicTerm);

    public T Visit(Integer integer);
}

public interface IInternalTermVisitor<TResult, TArgs>
{
    public TResult Visit(Variable variableTerm, TArgs arguments);

    public TResult Visit(Structure basicTerm, TArgs arguments);

    public TResult Visit(Integer integer, TArgs arguments);
}