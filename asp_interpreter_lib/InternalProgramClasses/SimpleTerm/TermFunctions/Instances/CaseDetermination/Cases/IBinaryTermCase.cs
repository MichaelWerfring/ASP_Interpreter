//-----------------------------------------------------------------------
// <copyright file="IBinaryTermCase.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

/// <summary>
/// An interface for a binary term case: this is basically for visiting two terms at once./>.
/// </summary>
public interface IBinaryTermCase
{
    /// <summary>
    /// Accepts a binary term case visitor.
    /// </summary>
    /// <param name="visitor">The visitor to accept.</param>
    public void Accept(IBinaryTermCaseVisitor visitor);

    /// <summary>
    /// Accepts a binary term case visitor.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    /// <param name="visitor">The visitor to accept.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Accept<T>(IBinaryTermCaseVisitor<T> visitor);
}