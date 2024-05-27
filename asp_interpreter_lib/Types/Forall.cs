//-----------------------------------------------------------------------
// <copyright file="Forall.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class Forall : Goal
{
    public VariableTerm VariableTerm { get; }

    public Goal Goal { get; }

    public Forall(VariableTerm variableTerm, Goal goal)
    {
        this.VariableTerm = variableTerm;
        this.Goal = goal;
    }

    /// <inheritdoc/>
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string s = string.Empty;
        s += "forall(" + this.VariableTerm.ToString() + ", ";
        return s + this.Goal.ToString() + ")";
    }
}