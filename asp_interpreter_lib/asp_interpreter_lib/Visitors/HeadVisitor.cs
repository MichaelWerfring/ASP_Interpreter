using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using Antlr4.Runtime.Tree;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;

namespace asp_interpreter_lib.Visitors;

public class HeadVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<Head>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override Head VisitHead(ASPParser.HeadContext context)
    {
        var headLiteral = context.classical_literal().Accept(new ClassicalLiteralVisitor(_errorLogger));
        
        return new Head(headLiteral);
    }
}