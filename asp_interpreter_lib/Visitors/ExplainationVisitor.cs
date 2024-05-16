using Antlr4.Runtime.Tree;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Util.ErrorHandling;
namespace asp_interpreter_lib.Visitors;

public class ExplanationVisitor : ASPParserBaseVisitor<IOption<asp_interpreter_lib.Types.Explanation>>
{
    private readonly ILogger _logger;
    private readonly ASPParserBaseVisitor<IOption<Literal>> _literalVisitor;
    private readonly ExplanationVariableVisitor _variableVisitor;
    private readonly ExplanationTextVisitor _textVisitor;

    public ExplanationVisitor(ILogger logger,
                              ASPParserBaseVisitor<IOption<Literal>> literalVisitor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _literalVisitor = literalVisitor ?? throw new ArgumentNullException(nameof(literalVisitor));
        _variableVisitor = new ExplanationVariableVisitor();
        _textVisitor = new ExplanationTextVisitor();
    }

    public override IOption<Explanation> VisitExplaination(ASPParser.ExplainationContext context)
    {
        var literal = context.literal().Accept(_literalVisitor);
        if (literal == null || !literal.HasValue)
        {
            _logger.LogError("The specified literal cannot be parsed!", context);
            return new None<Explanation>();
        }

        List<string> textParts = [];
        HashSet<int> variablesAt = [];

        for (int i = 0; i < context.children.Count; i++)
        {
            IParseTree? child = context.children[i];
            var text = child.Accept(_textVisitor);
            if (!string.IsNullOrWhiteSpace(text))
            {
                textParts.Add(text);
                continue;
            }

            var variable = child.Accept(_variableVisitor);
            if (variable != null)
            {
                textParts.Add(variable);
                int index = textParts.FindLastIndex(v => variable == v);
                variablesAt.Add(index);
            }
        }

        return new Some<Explanation>(new Explanation(textParts, variablesAt, literal.GetValueOrThrow()));
    }
}