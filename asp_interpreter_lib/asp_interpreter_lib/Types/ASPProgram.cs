namespace asp_interpreter_lib.Types;

public class AspProgram
{
    public AspProgram(List<Statement> statments, Query query)
    {
        Statements = statments;
        Query = query;
    }

    public List<Statement> Statements { get; private set; }
    
    public Query Query { get; private set; }
}