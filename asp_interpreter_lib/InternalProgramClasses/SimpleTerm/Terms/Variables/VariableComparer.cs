using System.Diagnostics.CodeAnalysis;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

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
