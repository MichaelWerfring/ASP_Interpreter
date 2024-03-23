namespace asp_interpreter_lib.Types.Terms;

public class VariableTerm: Term
{
    public VariableTerm(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}