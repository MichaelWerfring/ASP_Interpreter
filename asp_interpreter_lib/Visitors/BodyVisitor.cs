using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class BodyVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Body>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<Body> VisitBody(ASPParser.BodyContext context)
    {
        var children = context.children;
        List<NafLiteral> literals = [];
        var literalVisitor = new NafLiteralVisitor(_errorLogger);
        
        foreach (var c in children)
        {
            var literal = c.Accept(literalVisitor);

            //The children can sometimes be null therefore ignore them
            if (literal == null) continue;
            
            if (!literal.HasValue)
            {
                _errorLogger.LogError("Failed to parse literal in body!", context);
                return new None<Body>();
            }
            
            literals.Add(literal.GetValueOrThrow());
        }

        if (literals.Count == 0)
        {
            _errorLogger.LogError("A body must contain at least one literal", context);
            return new None<Body>();
        }
        
        return new Some<Body>(new Body(literals));
    }
}