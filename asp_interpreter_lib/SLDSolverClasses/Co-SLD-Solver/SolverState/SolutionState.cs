using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util;
using System.Text;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.SolverState;

public class SolutionState
{
    public SolutionState
    (
        CallStack currentStack,
        CoinductiveHypothesisSet currentSet,
        VariableMapping currentMapping,
        int nextInternalIndex
    )
    {
        ArgumentNullException.ThrowIfNull(currentStack, nameof(currentStack));
        ArgumentNullException.ThrowIfNull(currentSet, nameof(currentSet));
        ArgumentNullException.ThrowIfNull(currentMapping, nameof(currentMapping));

        CurrentStack = currentStack;
        CurrentSet = currentSet;
        CurrentMapping = currentMapping;
        NextInternalVariableIndex = nextInternalIndex;
    }

    public CallStack CurrentStack { get; }

    public CoinductiveHypothesisSet CurrentSet { get; }

    public VariableMapping CurrentMapping { get; }

    public int NextInternalVariableIndex { get; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Current stack:");
        sb.AppendLine(CurrentStack.TermStack.ToList().ListToString());

        sb.AppendLine("Current set:");
        sb.AppendLine($"{{ {CurrentSet.Terms.ToList().ListToString()} }}");

        sb.AppendLine("Current mapping:");
        foreach(var pair in CurrentMapping.Mapping)
        {
            sb.AppendLine($"{pair.Key.ToString()} : {pair.Value.ToString()}");
        }

        sb.AppendLine("Current variable index:");
        sb.AppendLine(NextInternalVariableIndex.ToString());

        return sb.ToString();
    }
}
