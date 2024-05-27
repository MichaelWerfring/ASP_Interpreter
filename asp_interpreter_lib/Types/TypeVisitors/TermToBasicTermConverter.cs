//-----------------------------------------------------------------------
// <copyright file="TermToBasicTermConverter.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

public class TermToBasicTermConverter : TypeBaseVisitor<BasicTerm>
{
    /// <inheritdoc/>
    public override IOption<BasicTerm> Visit(BasicTerm term)
    {
        return new Some<BasicTerm>(term);
    }
}