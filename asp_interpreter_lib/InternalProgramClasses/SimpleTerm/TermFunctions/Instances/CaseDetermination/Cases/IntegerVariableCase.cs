using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

public class IntegerVariableCase : IBinaryTermCase
{
    public IntegerVariableCase(Integer left, Variable right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Left = left;
        Right = right;
    }

    public Integer Left { get; }

    public Variable Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IBinaryTermCaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
