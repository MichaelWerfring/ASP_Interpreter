using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util;
using System.Text;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

public class SolutionState
{
    public SolutionState
    (
        CallStack stack,
        CoinductiveHypothesisSet set,
        VariableMapping mapping,
        int nextInternalIndex
    )
    {
        ArgumentNullException.ThrowIfNull(stack, nameof(stack));
        ArgumentNullException.ThrowIfNull(set, nameof(set));
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

        Stack = stack;
        Set = set;
        Mapping = mapping;
        NextInternalVariableIndex = nextInternalIndex;
    }

    public CallStack Stack { get; }

    public CoinductiveHypothesisSet Set { get; }

    public VariableMapping Mapping { get; }

    public int NextInternalVariableIndex { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Current stack:");
        sb.AppendLine(Stack.TermStack.ToList().ListToString());

        sb.AppendLine("Current set:");
        sb.AppendLine($"{{ {Set.Entries.ToList().ListToString()} }}");

        sb.AppendLine("Current mapping:");
        foreach(var pair in Mapping.Mapping)
        {
            sb.AppendLine($"{pair.Key.ToString()} : {pair.Value.ToString()}");
        }

        sb.AppendLine("Current variable index:");
        sb.AppendLine(NextInternalVariableIndex.ToString());

        return sb.ToString();
    }
}
