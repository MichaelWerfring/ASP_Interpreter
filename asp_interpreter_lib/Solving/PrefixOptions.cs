﻿namespace asp_interpreter_lib.Solving;

public class PrefixOptions(
    string rewriteHeadPrefix, 
    string forallPrefix,
    string emptyHeadPrefix,
    string checkPrefix,
    string dualPrefix,
    string variablePrefix)
{
    public string RewriteHeadPrefix { get; } = rewriteHeadPrefix;
    
    public string ForallPrefix { get; } = forallPrefix;
    
    public string EmptyHeadPrefix { get; } = emptyHeadPrefix;
    
    public string CheckPrefix { get; } = checkPrefix;
    
    public string DualPrefix { get; } = dualPrefix;
    
    public string VariablePrefix { get; } = variablePrefix;
}