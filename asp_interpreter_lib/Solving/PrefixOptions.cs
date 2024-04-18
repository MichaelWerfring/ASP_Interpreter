namespace asp_interpreter_lib.Solving;

public class PrefixOptions(
    string rewriteHeadPrefix, 
    string forallPrefix,
    string emptyHeadPrefix,
    string checkPrefix)
{
    public string RewriteHeadPrefix { get; } = rewriteHeadPrefix;
    public string ForallPrefix { get; } = forallPrefix;
    public string EmptyHeadPrefix { get; } = emptyHeadPrefix;
    public string CheckPrefix { get; } = checkPrefix;
}