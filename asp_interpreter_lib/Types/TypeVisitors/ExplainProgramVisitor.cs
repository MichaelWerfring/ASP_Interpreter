using System.Reflection;
using System.Text;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors
{
    public class ExplainProgramVisitor : TypeBaseVisitor<string>
    {
        private readonly AspProgram _program;

        public ExplainProgramVisitor(AspProgram program)
        {
            _program = program ?? throw new ArgumentNullException(nameof(program));
        }

        public override IOption<string> Visit(Statement statement)
        {
            if (!statement.HasHead)
            {
                return new Some<string>(string.Empty);
            }

            StringBuilder sb = new StringBuilder();
            var head = statement.Head.GetValueOrThrow();

            Explanation explaination;

            if (!_program.Explanations.TryGetValue(head.Identifier, out explaination))
            {
                //sb.Append(head.ToString() + " holds");
                return new Some<string>(string.Empty);
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

            if (!_program.Explanations.TryGetValue(literal.Identifier, out explaination))
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
                    //replace var
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