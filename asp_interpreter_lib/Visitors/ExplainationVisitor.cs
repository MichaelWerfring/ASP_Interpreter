using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Explaination;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Util.ErrorHandling;
namespace asp_interpreter_lib.Visitors;

public class ExplanationVisitor(ILogger logger) : ASPParserBaseVisitor<IOption<Explanation>>
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public override IOption<Explanation> VisitExplaination(ASPParser.ExplainationContext context)
    {
        var literal = context.literal();

        foreach (var c in context.children)
        {
            Console.WriteLine(c);
        }

        return new None<Explanation>();
    }
}