using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class StatementCopyVisitor : TypeBaseVisitor<Statement>
{
    private readonly TermCopyVisitor _termCopyVisitor = new();
    
    public override IOption<Statement> Visit(Statement statement)
    {
        Statement copy = new();

        if (statement.HasBody)
        {
            copy.AddBody(CopyBody(statement.Body));    
        }

        if (statement.HasHead)
        {
            copy.AddHead(CopyHead(statement.Head));
        }

        return new Some<Statement>(copy);
    }

    private Head CopyHead(Head head)
    {
        if (head.Literal == null)
        {
            //just return empty head
            return new Head();
        }
        
        return new Head(CopyClassicalLiteral(head.Literal));
    }

    private Body CopyBody(Body body)
    {
        List<NafLiteral> newLiterals = [];

        foreach (var literal in body.Literals)
        {
            if (literal.IsBinaryOperation)
            {
                newLiterals.Add(new NafLiteral(CopyBinOp(literal.BinaryOperation)));
                continue;
            }
            
            newLiterals.Add(new NafLiteral(CopyClassicalLiteral(literal.ClassicalLiteral),
                                                literal.IsNafNegated));
        }

        return new Body(newLiterals);
    }

    private BinaryOperation CopyBinOp(BinaryOperation binaryOperation)
    {
        var leftCopy = binaryOperation.Left.Accept(_termCopyVisitor).GetValueOrThrow(
            "The given left term cannot be read!");
        var rightCopy = binaryOperation.Right.Accept(_termCopyVisitor).GetValueOrThrow(
            "The given right term cannot be read!");

        return new BinaryOperation(leftCopy, binaryOperation.BinaryOperator, rightCopy);
    }
    
    private ClassicalLiteral CopyClassicalLiteral(ClassicalLiteral literal)
    {
        string newId = literal.Identifier.GetCopy();
        List<ITerm> newTerms = [];
        bool neg = literal.Negated;
        
        foreach (var term in literal.Terms)
        {
            var newTerm = term.Accept(_termCopyVisitor);
            newTerm.IfHasValue(t => newTerms.Add(t));
        }

        return new ClassicalLiteral(newId, neg, newTerms);
    }
}