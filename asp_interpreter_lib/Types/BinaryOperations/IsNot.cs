using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_interpreter_lib.Types.BinaryOperations
{
    public class IsNot : BinaryOperator, IVisitableType
    {
        public override string ToString()
        {
            return "is not";
        }

        public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }
    }
}
