using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Types.TermEquivalence
{
    public class TermEquivalenceChecker
    {
        public bool AreEqual(ITerm term1, ITerm term2)
        {
            return term1.ToString() == term2.ToString();
        }
    }
}
