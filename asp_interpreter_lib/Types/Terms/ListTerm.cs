//-----------------------------------------------------------------------
// <copyright file="ListTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public abstract class ListTerm : ITerm
{
    public abstract override string ToString();

    public abstract IOption<T> Accept<T>(TypeBaseVisitor<T> visitor);
}