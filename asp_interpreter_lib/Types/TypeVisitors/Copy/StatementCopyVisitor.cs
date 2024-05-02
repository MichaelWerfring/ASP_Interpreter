using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors.Copy;

public class StatementCopyVisitor : TypeBaseVisitor<Statement>
{
    private readonly BinaryOperationVisitor _binaryOperationVisitor;
    private readonly LiteralCopyVisitor _literalCopyVisitor;

    public StatementCopyVisitor()
    {
        var termCopyVisitor = new TermCopyVisitor();
        _literalCopyVisitor = new LiteralCopyVisitor(termCopyVisitor);
        _binaryOperationVisitor = new BinaryOperationVisitor(termCopyVisitor);
    }
    
    public override IOption<Statement> Visit(Statement statement)
    {
        Statement copy = new();

        if (statement.HasHead)
        {
            statement.Head.GetValueOrThrow().Accept(_literalCopyVisitor).IfHasValue(
                head => copy.AddHead(head));
        }
        
        List<Goal> body = [];
        foreach (var goal in statement.Body)
        {
            goal.Accept(_literalCopyVisitor).IfHasValue(
                literal => body.Add(literal));
            goal.Accept(_binaryOperationVisitor).IfHasValue(
                binaryOperation => body.Add(binaryOperation));
        }

        if (body.Count != statement.Body.Count)
        {
            throw new InvalidOperationException("The body of the statement could not be copied!");
        }
        
        copy.AddBody(body);
        
        return new Some<Statement>(copy);
    }
}