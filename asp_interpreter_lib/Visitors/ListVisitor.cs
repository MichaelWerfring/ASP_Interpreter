using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class ListVisitor(IErrorLogger errorLogger): ASPBaseVisitor<IOption<ListTerm>>
{
    private IErrorLogger _errorLogger = errorLogger;
    
    public override IOption<ListTerm> VisitConventionalList(ASPParser.ConventionalListContext context)
    {
        var innerList = context.terms();

        if (innerList == null)
        {
            return new Some<ListTerm>(new ConventionalList([]));
        }
        
        var terms = innerList.Accept(new TermsVisitor(_errorLogger));
        
        if(!terms.HasValue)
        {
            _errorLogger.LogError("Cannot parse list terms!", context);
            return new None<ListTerm>();
        }
        
        return new Some<ListTerm>(new ConventionalList(terms.GetValueOrThrow()));
    }

    public override IOption<ListTerm> VisitRecursiveList(ASPParser.RecursiveListContext context)
    {
        var head = context.term(0).Accept(new TermVisitor(_errorLogger));
        var tail = context.term(1).Accept(new TermVisitor(_errorLogger));

        if (!head.HasValue)
        {
            _errorLogger.LogError("Cannot parse head term!", context);
            return new None<ListTerm>();
        }
        
        if (!tail.HasValue)
        {
            _errorLogger.LogError("Cannot parse tail term!", context);
            return new None<ListTerm>();
        }
        
        return new Some<ListTerm>(new RecursiveList(head.GetValueOrThrow(), tail.GetValueOrThrow()));
    }
}