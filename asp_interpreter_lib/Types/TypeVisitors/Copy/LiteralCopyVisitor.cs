//-----------------------------------------------------------------------
// <copyright file="LiteralCopyVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors.Copy;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;

public class LiteralCopyVisitor(TypeBaseVisitor<ITerm> termCopyVisitor) : TypeBaseVisitor<Literal>
{
    public override IOption<Literal> Visit(Literal literal)
    {
        bool naf = literal.HasNafNegation;
        bool classical = literal.HasStrongNegation;
        string identifier = literal.Identifier.GetCopy();
        List<ITerm> terms =[];

        foreach (var term in literal.Terms)
        {
            terms.Add(term.Accept(termCopyVisitor).GetValueOrThrow(
                "The given term cannot be read!"));
        }

        return new Some<Literal>(new Literal(identifier, naf, classical, terms));
    }
}