
using asp_interpreter_lib.SimplifiedTerm.Visitor;

namespace asp_interpreter_lib.SimplifiedTerm;

public interface ISimplifiedTerm
{
    public IEnumerable<ISimplifiedTerm> Children { get; }

    public void Accept(ISimplifiedTermVisitor visitor);

    public T Accept<T>(ISimplifiedTermVisitor<T> visitor);
}
