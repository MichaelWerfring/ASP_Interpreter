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
    
   //public override IOption<ClassicalLiteral> VisitClassical_literal(ASPParser.Classical_literalContext context)
   //{
   //    var negated = context.MINUS() !=  null;
   //    //? because ID can be null in an empty program
   //    var id = context.ID()?.GetText();

   //    if (string.IsNullOrEmpty(id))
   //    {
   //        _errorLogger.LogError($"Cannot parse the literals identifier!", context);
   //        return new None<ClassicalLiteral>();
   //    }
   //            
   //    List<Term> terms = [];
   //    var termVisitor = new TermVisitor(_errorLogger);
   //    
   //    //var childTerms = context.terms()?.children;
   //    terms = GetChildTerms(context);
   //    List<IParseTree> childTerms = null;

   //    
   //    if (childTerms == null)
   //    {
   //        //Not an error just a literal without child terms
   //        return new Some<ClassicalLiteral>(new ClassicalLiteral(id, negated,terms));
   //    }
   //    
   //    foreach (var child in childTerms)
   //    {
   //        //This does not work recursively
   //        
   //        var term = child.Accept(termVisitor);

   //        //The children can sometimes be null therefore ignore them
   //        if (term == null) continue;
   //        
   //        if (!term.HasValue)
   //        {
   //            _errorLogger.LogError("Cannot parse the term contained the literal!", context);
   //            return new None<ClassicalLiteral>();
   //        }
   //        
   //        terms.Add(term.GetValueOrThrow());
   //    }
   //    
   //    return new Some<ClassicalLiteral>(new ClassicalLiteral(id, negated,terms));
   //}
}