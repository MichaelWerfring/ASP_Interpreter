using asp_interpreter_lib.SimplifiedTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SimplifiedTerm.TermFunctionality
{
    public class TermChildrenReplacer : ISimplifiedTermVisitor<ISimplifiedTerm>
    {
        private IEnumerable<ISimplifiedTerm>? _children;

        public ISimplifiedTerm Replace(ISimplifiedTerm term, IEnumerable<ISimplifiedTerm> newChildren)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(newChildren);

            _children = newChildren;

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
            return new BasicTerm(basicTerm.Functor, _children!, basicTerm.IsNegated);
        }
    }
}
