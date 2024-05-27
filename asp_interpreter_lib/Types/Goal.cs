//-----------------------------------------------------------------------
// <copyright file="Goal.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public abstract class Goal : IVisitableType
{
    /// <inheritdoc/>
    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);

    /// <inheritdoc/>
    public abstract override string ToString();
}