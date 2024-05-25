using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

public class IntegerStructureCase : IBinaryTermCase
{
    public IntegerStructureCase(Integer left, Structure right)
    {
        ArgumentNullException.ThrowIfNull(left, nameof(left));
        ArgumentNullException.ThrowIfNull(right, nameof(right));

        Left = left;
        Right = right;
    }

    public Integer Left { get; }

    public Structure Right { get; }

    public void Accept(IBinaryTermCaseVisitor visitor)
    {
        visitor.Visit(this);
    }

    public T Accept<T>(IBinaryTermCaseVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
