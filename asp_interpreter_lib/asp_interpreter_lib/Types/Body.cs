namespace asp_interpreter_lib.Types;

public class Body
{
    
    //Does not support body consisting of bodies yet eg. grammar
    public Body(List<NafLiteral> literals)
    {
        Literals = literals;
    }

    //A body consists of any literals 
    public List<NafLiteral> Literals { get; private set; }
}