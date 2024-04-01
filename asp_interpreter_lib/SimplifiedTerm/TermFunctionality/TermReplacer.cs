using asp_interpreter_lib.SimplifiedTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SimplifiedTerm.TermFunctionality
{
    public class TermReplacer
    {
        private TermEquivalenceChecker _checker;
        private TermChildrenReplacer _childrenReplacer;
        private TermCloner _termCloner;

        public TermReplacer()
        {
            _checker = new TermEquivalenceChecker();
            _childrenReplacer = new TermChildrenReplacer();
            _termCloner = new TermCloner();
        }

        public ISimplifiedTerm Replace(ISimplifiedTerm termToReplaceVariablesIn, ISimplifiedTerm termToReplace, ISimplifiedTerm replacement)
        {
            ArgumentNullException.ThrowIfNull(termToReplaceVariablesIn);
            ArgumentNullException.ThrowIfNull(termToReplace);
            ArgumentNullException.ThrowIfNull(replacement);

            return RecursiveReplace(termToReplaceVariablesIn, termToReplace, replacement);
        }

        private ISimplifiedTerm RecursiveReplace(ISimplifiedTerm currentTerm, ISimplifiedTerm termToReplace, ISimplifiedTerm replacement)
        {       
            if (_checker.AreEqual(currentTerm, termToReplace))
            {
                return _termCloner.Clone(replacement);
            }

            var children = currentTerm.Children.ToArray();
            for(int i = 0; i < children.Length; i++)
            {
                children[i] = RecursiveReplace(children[i], termToReplace, replacement);
            }

            return _childrenReplacer.Replace(currentTerm, children);
        }
    }
}
