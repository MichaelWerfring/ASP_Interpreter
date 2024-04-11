using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions
{
    public class SimpleTermContainsChecker : ISimpleTermArgsVisitor<bool, ISimpleTerm>
    {
        private SimpleTermComparer _equivalenceChecker = new SimpleTermComparer();


        public bool LeftContainsRight(ISimpleTerm term, ISimpleTerm other)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(other);

            return term.Accept(this, other);
        }

        public bool Visit(Variable term, ISimpleTerm other)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(other);

            return _equivalenceChecker.Visit(term, other);
        }

        public bool Visit(Structure term, ISimpleTerm other)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(other);

            var areEqual = _equivalenceChecker.Visit(term, other);
            if (areEqual)
            {
                return true;
            }

            foreach (var child in term.Children)
            {
                bool containsEqualChild = child.Accept(this, other);

                if (containsEqualChild)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
