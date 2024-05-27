//-----------------------------------------------------------------------
// <copyright file="ExplainationVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Visitors;
using Antlr4.Runtime.Tree;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class ExplanationVisitor : ASPParserBaseVisitor<IOption<Asp_interpreter_lib.Types.Explanation>>
{
    private readonly ILogger logger;
    private readonly ASPParserBaseVisitor<IOption<Literal>> literalVisitor;
    private readonly ExplanationVariableVisitor variableVisitor;
    private readonly ExplanationTextVisitor textVisitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExplanationVisitor"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="literalVisitor"></param>
    public ExplanationVisitor(
        ILogger logger,
                              ASPParserBaseVisitor<IOption<Literal>> literalVisitor)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.literalVisitor = literalVisitor ?? throw new ArgumentNullException(nameof(literalVisitor));
        this.variableVisitor = new ExplanationVariableVisitor(this.logger);
        this.textVisitor = new ExplanationTextVisitor(this.logger);
    }

    public override IOption<Explanation> VisitExplanation(ASPParser.ExplanationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var optionLiteral = context.literal().Accept(this.literalVisitor);
        if (optionLiteral == null || !optionLiteral.HasValue)
        {
            this.logger.LogError("The specified literal cannot be parsed!", context);
            return new None<Explanation>();
        }

        var literal = optionLiteral.GetValueOrThrow();
        var variablesInLiteral = literal.Accept(new VariableFinder()).GetValueOrThrow();

        if (literal.Terms.Count != variablesInLiteral.Count)
        {
            this.logger.LogError("Not all terms in the head of the explanation are variables!", context);
            return new None<Explanation>();
        }

        List<string> textParts = new List<string>();
        HashSet<int> variablesAt = new HashSet<int>();

        for (int i = 0; i < context.children.Count; i++)
        {
            IParseTree? child = context.children[i];
            var text = child.Accept(this.textVisitor);
            if (!string.IsNullOrWhiteSpace(text))
            {
                textParts.Add(text);
                continue;
            }

            var variable = child.Accept(this.variableVisitor);
            if (variable != null)
            {
                // see if variable is contained in literal
                if (!variablesInLiteral.Any(v => v.Identifier == variable))
                {
                    this.logger.LogError($"The variable {variable} is not in the head of the explanation: {literal.ToString()}!", context);
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