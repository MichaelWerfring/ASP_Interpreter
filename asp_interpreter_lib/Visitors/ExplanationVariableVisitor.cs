using Antlr4.Runtime.Misc;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Visitors
{
    internal class ExplanationVariableVisitor : ASPParserBaseVisitor<string>
    {
        public override string VisitExp_var([NotNull] ASPParser.Exp_varContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            return context.EXP_VAR().GetText();
        }
    }
}
