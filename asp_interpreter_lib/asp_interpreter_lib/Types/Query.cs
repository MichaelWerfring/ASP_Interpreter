namespace asp_interpreter_lib.Types;

public class Query
{
    public Query(ClassicalLiteral classicalLiteral)
    {
        ClassicalLiteral = classicalLiteral;
    }

    public ClassicalLiteral ClassicalLiteral { get; private set; }
}