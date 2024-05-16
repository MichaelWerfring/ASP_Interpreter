using Antlr4.Runtime.Misc;
using asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Visitors
{
    internal class ExplanationTextVisitor : ASPParserBaseVisitor<string>
    {
        public override string VisitExp_text([NotNull] ASPParser.Exp_textContext context)
        {
            string text = context.GetText();
            return text;
        }
    }
}
