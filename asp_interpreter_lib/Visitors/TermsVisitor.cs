﻿using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Visitors;

public class TermsVisitor(IErrorLogger errorLogger) : ASPBaseVisitor<IOption<List<Term>>>
{

    private IErrorLogger _errorLogger = errorLogger;
    
    private TermVisitor _termVisitor = new TermVisitor(errorLogger);
    
    public override IOption<List<Term>> VisitTerms(ASPParser.TermsContext context)
    {
        var multipleTerms = context.terms();
        var singleTerm = context.term().Accept(_termVisitor);

        if (!singleTerm.HasValue)
        {
            //This should never happen but if it does its a severe fail
            _errorLogger.LogError("Cannot parse value of compound term!", context);
            return new None<List<Term>>();
        }
        
        //End of recursion
        if (multipleTerms == null)
        {
            return new Some<List<Term>>([singleTerm.GetValueOrThrow()]);
        }
        
        var terms = multipleTerms.Accept(this);
        
        if (!terms.HasValue)
        {
            _errorLogger.LogError("Cannot parse value of compound term!", context);
            return new None<List<Term>>();
        }

        var list = terms.GetValueOrThrow();
        //To build the list in proper order
        list.Insert(0,singleTerm.GetValueOrThrow());
        
        return terms;
    }
}