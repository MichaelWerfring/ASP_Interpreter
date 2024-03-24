namespace asp_interpreter_lib.Types;

public class Statement(Head? head, Body? body)
{
    public bool HasHead => Head != null;
    
    public bool HasBody => Body != null;
    
    public Head? Head { get; } = head;
    public Body? Body { get; } = body;
}