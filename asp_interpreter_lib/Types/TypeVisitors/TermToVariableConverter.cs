//-----------------------------------------------------------------------
// <copyright file="TermToVariableConverter.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors
{
    using Asp_interpreter_lib.Types.Terms;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using System;

    internal class TermToVariableConverter : TypeBaseVisitor<VariableTerm>
    {
        public override IOption<VariableTerm> Visit(VariableTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            return new Some<VariableTerm>(term);
        }
    }
}