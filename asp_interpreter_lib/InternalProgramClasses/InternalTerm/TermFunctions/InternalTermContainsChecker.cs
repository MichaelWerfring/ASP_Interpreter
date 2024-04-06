using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions
{
    public class InternalTermContainsChecker : IInternalTermVisitor<bool, IInternalTerm>
    {
        private InternalTermComparer _equivalenceChecker;

        public InternalTermContainsChecker()
        {
            _equivalenceChecker = new InternalTermComparer();
        }

        public bool LeftContainsRight(IInternalTerm term, IInternalTerm other)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(other);

            return term.Accept(this, other);
        }

        public bool Visit(Variable term, IInternalTerm other)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(other);

            return _equivalenceChecker.Visit(term, other);
        }

        public bool Visit(Structure term, IInternalTerm other)
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

                if(containsEqualChild) 
                {
                    return true;
                }
            }

            return false;
        }

        public bool Visit(Integer term, IInternalTerm other)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(other);

            return _equivalenceChecker.Visit(term, other);
        }
    }
}
