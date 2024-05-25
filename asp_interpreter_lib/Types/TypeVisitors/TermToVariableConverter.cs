using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_interpreter_lib.Types.TypeVisitors
{
    internal class TermToVariableConverter : TypeBaseVisitor<VariableTerm>
    {
        public override IOption<VariableTerm> Visit(VariableTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            return new Some<VariableTerm>(term);
        }
    }
}
