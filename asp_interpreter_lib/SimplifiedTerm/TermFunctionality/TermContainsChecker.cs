using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SimplifiedTerm.TermFunctionality
{
    public class TermContainsChecker
    {
        private TermEquivalenceChecker _equivalenceChecker;

        public TermContainsChecker()
        {
            _equivalenceChecker = new TermEquivalenceChecker();
        }

        public bool LeftContainsRight(ISimplifiedTerm term, ISimplifiedTerm other)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(other);

            if (_equivalenceChecker.AreEqual(term, other)) return true;

            bool atLeastOneChildContainsOther = false;
            foreach(var childTerm in term.Children) 
            {
                atLeastOneChildContainsOther |= LeftContainsRight(childTerm, other);
            }

            return atLeastOneChildContainsOther;
        }
    }
}
