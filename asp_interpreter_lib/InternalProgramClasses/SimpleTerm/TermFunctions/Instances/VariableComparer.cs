using System.Diagnostics.CodeAnalysis;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class VariableComparer : IEqualityComparer<Variable>
{
    public bool Equals(Variable? x, Variable? y)
    {
        if (x == null && y == null) return true;

        if (x == null) return false;

        if (y == null) return false;

        return x.Identifier == y.Identifier;
    }

    public int GetHashCode([DisallowNull] Variable obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        return obj.Identifier.GetHashCode();
    }
}
