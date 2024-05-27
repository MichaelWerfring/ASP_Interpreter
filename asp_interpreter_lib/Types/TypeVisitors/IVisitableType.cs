//-----------------------------------------------------------------------
// <copyright file="IVisitableType.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public interface IVisitableType
{
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}