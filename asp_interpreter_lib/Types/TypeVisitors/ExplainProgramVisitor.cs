//-----------------------------------------------------------------------
// <copyright file="ExplainProgramVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors
{
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System.Text;

    /// <summary>
    /// Represents the visitor to explain a program.
    /// </summary>
    public class ExplainProgramVisitor : TypeBaseVisitor<string>
    {
        private readonly AspProgram program;

        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplainProgramVisitor"/> class.
        /// </summary>
        /// <param name="program">The program to be explained.</param>
        /// <param name="logger">The logger for potentional errors.</param>
        /// <exception cref="ArgumentNullException">Is thrown if one of the parameters is null.</exception>
        public ExplainProgramVisitor(AspProgram program, ILogger logger)
        {
            this.program = program ?? throw new ArgumentNullException(nameof(program));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Explains a statement by generating text.
        /// </summary>
        /// <param name="statement">The statement to be explained.</param>
        /// <returns>The string representation of the statement is successful else none.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the statement is null.</exception>
        public override IOption<string> Visit(Statement statement)
        {
            ArgumentNullException.ThrowIfNull(statement);

            if (!statement.HasHead)
            {
                return new Some<string>(string.Empty);
            }

            StringBuilder sb = new StringBuilder();
            var head = statement.Head.GetValueOrThrow();

            Explanation explaination;

            if (!this.program.Explanations.TryGetValue((head.Identifier, head.Terms.Count), out explaination))
            {
                this.logger.LogInfo($"Unable to find Explanation for: {head.ToString()}");
                return new Some<string>(string.Empty);
            }

            if (explaination.Literal.Terms.Count != head.Terms.Count)
            {
                this.logger.LogError($"The actual Head of the statement: {head.ToString()}, does not " +
                    $"match the statement to be explained: {explaination.Literal.ToString()}");
                return new None<string>();
            }

            for (int i = 0; i < explaination.TextParts.Count; i++)
            {
                string item = explaination.TextParts[i];

                if (explaination.VariablesAt.Contains(i))
                {
                    var varAt = GetIndexOfVariable(item, explaination);
                    sb.Append(head.Terms[varAt].ToString());
                }
                else
                {
                    sb.Append(item);
                }
            }

            if (statement.HasBody)
            {
                sb.Append(" if ");
                sb.AppendLine();
                for (int i = 0; i < statement.Body.Count; i++)
                {
                    Goal goal = statement.Body[i];
                    if (i + 1 != statement.Body.Count)
                    {
                        sb.AppendLine("\t" + goal.Accept(this).GetValueOrThrow() + " and ");
                    }
                    else
                    {
                        sb.AppendLine("\t" + goal.Accept(this).GetValueOrThrow() + ".");
                    }
                }
            }
            else
            {
                sb.Append('.');
            }

            var t = sb.ToString();
            return new Some<string>(t);
        }

        /// <summary>
        /// Explains the literal by generating text.
        /// </summary>
        /// <param name="literal">The literal to be explained.</param>
        /// <returns>The string representation if successful else none.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the literal is null.</exception>
        public override IOption<string> Visit(Literal literal)
        {
            StringBuilder sb = new StringBuilder();

            Explanation explaination;

            if (literal.HasNafNegation)
            {
                sb.Append("there is no evidence that ");
            }

            if (literal.HasStrongNegation)
            {
                sb.Append("it is not the case that ");
            }

            if (!this.program.Explanations.TryGetValue((literal.Identifier, literal.Terms.Count), out explaination))
            {
                var copy = new Literal(literal.Identifier, false, false, literal.Terms);
                sb.Append(copy.ToString() + " holds");
                return new Some<string>(sb.ToString());
            }

            for (int i = 0; i < explaination.TextParts.Count; i++)
            {
                string item = explaination.TextParts[i];

                if (explaination.VariablesAt.Contains(i))
                {
                    // replace var
                    sb.Append(literal.Terms[GetIndexOfVariable(item, explaination)].ToString());
                }
                else
                {
                    sb.Append(item);
                }
            }

            var t = sb.ToString();
            return new Some<string>(t);
        }

        /// <summary>
        /// Explains the binary operation by generating text.
        /// </summary>
        /// <param name="binaryOperation">The operation to be explained.</param>
        /// <returns>The string representation if successful else none.</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the operation is null.</exception>
        public override IOption<string> Visit(BinaryOperation binaryOperation)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(binaryOperation.Left.ToString());
            BinopToTextConverter converter = new BinopToTextConverter();

            binaryOperation.BinaryOperator.Accept(converter).IfHasValue(v => sb.Append(v));

            sb.Append(binaryOperation.Right.ToString());

            return new Some<string>(sb.ToString());
        }

        private static int GetIndexOfVariable(string neededVariable, Explanation explanation)
        {
            var actualVariables = new List<string>();
            var variableVisitor = new TermToVariableConverter();

            foreach (var term in explanation.Literal.Terms)
            {
                var variable = term.Accept(variableVisitor);
                if (variable.HasValue)
                {
                    actualVariables.Add(variable.GetValueOrThrow().Identifier);
                }
            }

            return actualVariables.IndexOf(neededVariable);
        }
    }
}