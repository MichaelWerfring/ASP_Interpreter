namespace asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

public class NonGroundTermException : DisunificationException
{
    public NonGroundTermException(string message) : base(message)
    {
    }
}
