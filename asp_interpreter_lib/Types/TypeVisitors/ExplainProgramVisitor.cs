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

        public static string Explain(AspProgram program)
        {
            StringBuilder sb = new();
            foreach (var statement in program.Statements)
            {
                
            }
            
            return sb.ToString();
        }

        public override IOption<string> Visit(Statement statement)
        {
            if (!statement.HasHead)
            {
                new Some<string>("");
            }

            StringBuilder sb = new StringBuilder();
            var head = statement.Head.GetValueOrThrow();

            Explanation explaination;

            if (!_program.Explanations.TryGetValue(head.Identifier, out explaination))
            {
                sb.Append(head.ToString() + " holds");
                return new Some<string>(sb.ToString());
            }


            for (int i = 0; i < explaination.TextParts.Count; i++)
            {
                string item = explaination.TextParts[i];

                if (explaination.VariablesAt.Contains(i))
                {
                    //replace var

                    sb.Append(head.Terms[this.GetIndexOfVariable(item, explaination)].ToString());
                }
                else
                {
                    sb.Append(' ');
                    sb.Append(item);
                }
            }

            if (statement.HasBody)
            {
                sb.Append(" if ");
                sb.AppendLine();
                foreach (var goal in statement.Body)
                {
                    sb.AppendLine("   " + goal.Accept(this).GetValueOrThrow());
                }
            }

            var t = sb.ToString();
            return new Some<string>(t);

        }

        public override IOption<string> Visit(Literal literal)
        {
            StringBuilder sb = new StringBuilder();

            Explanation explaination;

            if (!_program.Explanations.TryGetValue(literal.Identifier, out explaination))
            {
                return new Some<string>("");
            }

            if (literal.HasNafNegation)
            {
                sb.Append("there is no evidence that ");
            }

            if (literal.HasStrongNegation)
            {
                sb.Append("there is no case that ");
            }

            for (int i = 0; i < explaination.TextParts.Count; i++)
            {
                string item = explaination.TextParts[i];

                if (explaination.VariablesAt.Contains(i))
                {
                    //replace var
                    sb.Append(literal.Terms[this.GetIndexOfVariable(item, explaination)].ToString());
                }
                else
                {
                    sb.Append(' ');
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

        private int GetIndexOfVariable(string neededVariable, Explanation explanation)
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