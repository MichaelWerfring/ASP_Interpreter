using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Visitors;

public class TermsVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<List<ITerm>>>
{

    private readonly ILogger _logger = logger ??
        throw new ArgumentNullException(nameof(logger), "The given argument must not be null!");

    private readonly TermVisitor _termVisitor = new(logger);
    
    public override IOption<List<ITerm>> VisitTerms(ASPParser.TermsContext context)
    {
        var multipleTerms = context.terms();
        var singleTerm = context.term().Accept(_termVisitor);

        if (!singleTerm.HasValue)
        {
            //This should never happen but if it does its a severe fail
            _logger.LogError("Cannot parse value of compound term!", context);
            return new None<List<ITerm>>();
        }
        
        //End of recursion
        if (multipleTerms == null)
        {
            return new Some<List<ITerm>>([singleTerm.GetValueOrThrow()]);
        }
        
        var terms = multipleTerms.Accept(this);
        
        if (!terms.HasValue)
        {
            _logger.LogError("Cannot parse value of compound term!", context);
            return new None<List<ITerm>>();
        }

        var list = terms.GetValueOrThrow();
        //To build the list in proper order
        list.Insert(0,singleTerm.GetValueOrThrow());
        
        return terms;
    }
}