namespace asp_interpreter_lib.Types;

public class Statement
{
    public Statement(Head? head, Body? body)
    {
        Head = head;
        Body = body;
    }
    
    public bool HasHead => Head != null;
    
    public bool HasBody => Body != null;
    
    public Head? Head { get; private set; }
    public Body? Body { get; private set; }
}