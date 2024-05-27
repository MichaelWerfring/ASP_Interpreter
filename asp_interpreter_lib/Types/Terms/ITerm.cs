//-----------------------------------------------------------------------
// <copyright file="ITerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;

public interface ITerm : IVisitableType
{
    // public abstract override string ToString();
}