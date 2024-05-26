//-----------------------------------------------------------------------
// <copyright file="StandardConstructiveUnificationAlgorithm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Unification.Constructive.Unification.Standard;

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A simple implementation of a constructive unification algorithm.
/// </summary>
public class StandardConstructiveUnificationAlgorithm : IConstructiveUnificationAlgorithm
{
    private readonly bool doOccursCheck;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardConstructiveUnificationAlgorithm"/> class.
    /// </summary>
    /// <param name="doOccursCheck">Whether to do an occcurs check.</param>
    public StandardConstructiveUnificationAlgorithm(bool doOccursCheck)
    {
        this.doOccursCheck = doOccursCheck;
    }

    /// <summary>
    /// Try to unify two terms, based on mapping of variables to prohibited value lists.
    /// Exceptions are thrown if mapping does not contain a value for each variable in left and right.
    /// </summary>
    /// <param name="target">The target to unify.</param>
    /// <returns>A unifying mapping or none.</returns>
    public IOption<VariableMapping> Unify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        var constructiveUnifier = new ConstructiveUnifier
        (
            this.doOccursCheck,
            target
        );

        return constructiveUnifier.Unify();
    }
}