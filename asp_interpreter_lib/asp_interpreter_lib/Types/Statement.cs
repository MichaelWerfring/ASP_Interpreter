namespace asp_interpreter_lib.Types;

public class Statement
{
    //Empty rule per default

    public bool HasBody => Body.Literals.Count != 0;
    public bool HasHead => Head.HasValue;
    
    //Empty Head and Body per default
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
}