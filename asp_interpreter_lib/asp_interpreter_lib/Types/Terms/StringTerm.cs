namespace asp_interpreter_lib.Types.Terms;

public class StringTerm: Term
{
    public string Value { get; set; }

    public StringTerm(string value)
    {
        Value = value;
    }
}