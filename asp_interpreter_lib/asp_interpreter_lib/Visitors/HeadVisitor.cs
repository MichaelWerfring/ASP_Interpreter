using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using Antlr4.Runtime.Tree;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class HeadVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<Head>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<Head> VisitHead(ASPParser.HeadContext context)
    {
        var headLiteral = context.classical_literal().Accept(
            new ClassicalLiteralVisitor(_errorLogger));
        
        if (!headLiteral.HasValue)
        {
            _errorLogger.LogError("Cannot parse head literal!", context);
            return new None<Head>(); 
        }
        
        return new Some<Head>(new Head(headLiteral.GetValueOrThrow()));
    }
}