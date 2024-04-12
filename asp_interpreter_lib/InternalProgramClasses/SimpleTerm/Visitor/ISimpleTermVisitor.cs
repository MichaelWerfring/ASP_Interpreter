using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;

public interface ISimpleTermVisitor
{
    public void Visit(Variable variableTerm);

    public void Visit(Structure basicTerm);
}

public interface ISimpleTermVisitor<T>
{
    public T Visit(Variable variableTerm);

    public T Visit(Structure basicTerm);
}

public interface ISimpleTermArgsVisitor<TArgs>
{
    public void Visit(Variable variableTerm, TArgs arguments);

    public void Visit(Structure basicTerm, TArgs arguments);
}

public interface ISimpleTermArgsVisitor<TResult, TArgs>
{
    public TResult Visit(Variable variableTerm, TArgs arguments);

    public TResult Visit(Structure basicTerm, TArgs arguments);
}