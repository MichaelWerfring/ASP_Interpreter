namespace asp_interpreter_lib.Types;

public class Body
{
    public Body(List<NafLiteral> literals)
    {
        Literals = literals;
    }

    //A body consists of any literals 
    public List<NafLiteral> Literals { get; private set; }
    
    
}