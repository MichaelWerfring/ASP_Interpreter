using Antlr4.Runtime.Misc;
using asp_interpreter_lib.SimplifiedTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SimplifiedTerm.TermFunctionality
{
    public class TermCloner : ISimplifiedTermVisitor<ISimplifiedTerm>
    {
        public ISimplifiedTerm Clone(ISimplifiedTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            return term.Accept(this);
        }

        public ISimplifiedTerm Visit(VariableTerm variableTerm)
        {
            ArgumentNullException.ThrowIfNull(variableTerm);

            return new VariableTerm(variableTerm.Identifier);
        }

        public ISimplifiedTerm Visit(BasicTerm basicTerm)
        {
            ArgumentNullException.ThrowIfNull(basicTerm);

            var copiedChildren = new List<ISimplifiedTerm>();
            foreach ( var child in basicTerm.Children )
            {
                copiedChildren.Add(child.Accept(this));
            }

            return new BasicTerm(basicTerm.Functor.ToString(), copiedChildren, basicTerm.IsNegated);
        }
    }
}
