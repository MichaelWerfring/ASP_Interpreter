//-----------------------------------------------------------------------
// <copyright file="StandardConstructiveUnificationAlgorithm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

using Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using Asp_interpreter_lib.Unification.Constructive.Target;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Unification.Constructive.Unification.Standard;

public class StandardConstructiveUnificationAlgorithm : IConstructiveUnificationAlgorithm
{
    private readonly bool _doOccursCheck;

    public StandardConstructiveUnificationAlgorithm(bool doOccursCheck)
    {
        _doOccursCheck = doOccursCheck;
    }

    /// <summary>
    /// Try to unify two terms, based on mapping of variables to prohibited value lists.
    /// Exceptions are thrown if mapping does not contain a value for each variable in left and right.
    /// </summary>
    /// <returns></returns>
    public IOption<VariableMapping> Unify(ConstructiveTarget target)
    {
        ArgumentNullException.ThrowIfNull(target);

        var constructiveUnifier = new ConstructiveUnifier
        (
            _doOccursCheck,
            target
        );

        return constructiveUnifier.Unify();
    }
}