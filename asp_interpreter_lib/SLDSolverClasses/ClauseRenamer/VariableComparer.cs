using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SLDSolverClasses.VariableRenaming
{
    public class VariableComparer : IEqualityComparer<Variable>
    {
        public bool Equals(Variable? x, Variable? y)
        {
            ArgumentNullException.ThrowIfNull(x, nameof(x));
            ArgumentNullException.ThrowIfNull(y, nameof(y));

            return x.Identifier == y.Identifier;
        }

        public int GetHashCode([DisallowNull] Variable obj)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));

            return obj.Identifier.GetHashCode();
        }
    }
}
