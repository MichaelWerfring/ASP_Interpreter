namespace asp_interpreter_lib.Types.Terms;

public class VariableTerm: Term
{
    private string _identifier;

    public VariableTerm(string identifier)
    {
        Identifier = identifier;
    }

    public string Identifier
    {
        get => _identifier;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
            {
                throw new ArgumentException("Identifier cannot be null, empty or whitespace!", 
                    nameof(Identifier));
            }
            
            _identifier = value;
        }
    }
}