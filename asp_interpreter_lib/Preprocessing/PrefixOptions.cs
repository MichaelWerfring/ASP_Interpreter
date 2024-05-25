namespace Asp_interpreter_lib.Preprocessing;

public class PrefixOptions(
    string forallPrefix,
    string emptyHeadPrefix,
    string checkPrefix,
    string dualPrefix,
    string variablePrefix)
{
    public string ForallPrefix { get; } = forallPrefix;

    public string EmptyHeadPrefix { get; } = emptyHeadPrefix;

    public string CheckPrefix { get; } = checkPrefix;

    public string DualPrefix { get; } = dualPrefix;

    public string VariablePrefix { get; } = variablePrefix;
}