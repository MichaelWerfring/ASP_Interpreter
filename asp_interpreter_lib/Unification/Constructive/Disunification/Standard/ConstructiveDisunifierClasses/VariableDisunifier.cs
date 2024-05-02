using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.Constructive.Disunification.Standard.ConstructiveDisunifierClasses;

public class VariableDisunifier
{
    public VariableDisunifier(Variable variable, ISimpleTerm term, bool isPositive)
    {
        ArgumentNullException.ThrowIfNull(variable, nameof(variable));
        ArgumentNullException.ThrowIfNull(term, nameof(term));

        Variable = variable;
        Term = term;
        IsPositive = isPositive;
    }

    public Variable Variable { get; }

    public ISimpleTerm Term { get; }

    public bool IsPositive { get; }
}
