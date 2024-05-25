using Antlr4.Runtime.Tree;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;
namespace Asp_interpreter_lib.Visitors;

public class ExplanationVisitor : ASPParserBaseVisitor<IOption<Asp_interpreter_lib.Types.Explanation>>
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

    public override IOption<Explanation> VisitExplanation(ASPParser.ExplanationContext context)
    {
        var optionLiteral = context.literal().Accept(_literalVisitor);
        if (optionLiteral == null || !optionLiteral.HasValue)
        {
            _logger.LogError("The specified literal cannot be parsed!", context);
            return new None<Explanation>();
        }

        var literal = optionLiteral.GetValueOrThrow();
        var variablesInLiteral = literal.Accept(new VariableFinder()).GetValueOrThrow();

        if (literal.Terms.Count != variablesInLiteral.Count)
        {
            _logger.LogError("Not all terms in the head of the explanation are variables!",context);
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
                //see if variable is contained in literal
                if (!variablesInLiteral.Any(v => v.Identifier == variable))
                {
                    _logger.LogError($"The variable {variable} is not in the head of the explanation: {literal.ToString()}!", context);
                    return new None<Explanation>(); 
                }

                textParts.Add(variable);
                int index = textParts.FindLastIndex(v => variable == v);
                variablesAt.Add(index);
            }
        }

        return new Some<Explanation>(new Explanation(textParts, variablesAt, literal));
    }
}