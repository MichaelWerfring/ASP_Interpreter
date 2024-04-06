using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types;

public class Statement: IVisitableType
{
    //Empty rule per default

    public bool HasBody => Body.Literals.Count != 0;
    public bool HasHead => Head.HasValue;
    
    //Empty InternalHead and InternalBody per default
    public Head Head { get; private set; } = new();
    public Body Body { get; private set; } = new([]);

    public void AddHead(Head head)
    {
        ArgumentNullException.ThrowIfNull(head);
        if (HasHead)
        {
            throw new ArgumentException("A statement can only have one head");
        }
        Head = head;
    }
    
    public void AddBody(Body body)
    {
        ArgumentNullException.ThrowIfNull(body);
        if (HasBody)
        {
            throw new ArgumentException("A statement can only have one body");
        }
        Body = body;
    }

    public override string ToString()
    {
        if(HasBody)
        {
            return $"{Head.ToString()} :- {Body.ToString()}.";
        }
        else
        {
            return $"{Head.ToString()}.";
        }
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}