namespace asp_interpreter_lib.Types;

public class Query
{
    private ClassicalLiteral _classicalLiteral;

    public Query(ClassicalLiteral classicalLiteral)
    {
        ClassicalLiteral = classicalLiteral;
    }

    public ClassicalLiteral ClassicalLiteral
    {
        get => _classicalLiteral;
        private set => _classicalLiteral = value ?? throw new ArgumentNullException(nameof(ClassicalLiteral));
    }
}