using System.Diagnostics;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using Antlr4.Runtime.Tree;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class ClassicalLiteralVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<ClassicalLiteral>>
{
    private IErrorLogger _errorLogger = errorLogger;

    private TermsVisitor _termVisitor = new TermsVisitor(errorLogger);
    
    public override IOption<ClassicalLiteral> VisitClassical_literal(ASPParser.Classical_literalContext context)
    {
        var negated = context.MINUS() !=  null;
        //? because ID can be null in an empty program
        var id = context.ID()?.GetText();

        if (string.IsNullOrEmpty(id))
        {
            _errorLogger.LogError($"Cannot parse the literals identifier!", context);
            return new None<ClassicalLiteral>();
        }

        var childTerms = context.terms();

        if (childTerms == null) return new Some<ClassicalLiteral>(new ClassicalLiteral(id, negated,[]));
        
        var terms = _termVisitor.VisitTerms(childTerms);
        
        if (!terms.HasValue)
        {
            _errorLogger.LogError("Cannot parse the terms contained the literal!", context);
            return new None<ClassicalLiteral>();
        }
        
        return new Some<ClassicalLiteral>(new ClassicalLiteral(id, negated,terms.GetValueOrThrow()));
    }
}