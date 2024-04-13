using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types.Terms;

public class RecursiveList : ListTerm
{
    private readonly ITerm _head;
    private readonly ITerm _tail;

    public RecursiveList(ITerm head, ITerm tail)
    {
        ArgumentNullException.ThrowIfNull(head);
        ArgumentNullException.ThrowIfNull(tail);
        
        _head = head;
        _tail = tail;
    }
    
    public ITerm Head => _head;
    
    public ITerm Tail => _tail;
    
    public override string ToString()
    {
        return $"[{_head.ToString()}| {_tail.ToString()}]";
    }
    
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}