using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace asp_interpreter_lib.Types;

public class Body
{
    private List<NafLiteral> _literals;

    //Does not support body consisting of bodies yet eg. grammar
    public Body(List<NafLiteral> literals)
    {
        Literals = literals;
    }

    //A body consists of any literals 
    public List<NafLiteral> Literals
    {
        get => _literals;
        private set => _literals = value ?? throw new ArgumentNullException(nameof(Literals));
    }
}