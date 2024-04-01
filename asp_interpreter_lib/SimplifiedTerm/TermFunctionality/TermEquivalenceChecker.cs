using asp_interpreter_lib.SimplifiedTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SimplifiedTerm.TermFunctionality
{
    public class TermEquivalenceChecker
    {
        /// <summary>
        /// Checks if two terms are equal.
        /// </summary>
        /// <exception cref="Exception">Thrown if unknown type is given (for now).</exception>
        public bool AreEqual(ISimplifiedTerm term1, ISimplifiedTerm term2)
        {
            ArgumentNullException.ThrowIfNull(term1);
            ArgumentNullException.ThrowIfNull(term2);

            if (term1.GetType() != term2.GetType())
            {
                return false;
            }

            if (term1.GetType() == typeof(VariableTerm))
            {
               return AreVariablesEqual((VariableTerm)term1, (VariableTerm)term2);
            }
            else if (term1.GetType() == typeof(BasicTerm))
            {
                return AreBasicTermsEqual((BasicTerm) term1, (BasicTerm)term2);
            }
            else
            {
                throw new Exception("unknown type!");
            }
        }

        private bool AreVariablesEqual(VariableTerm a, VariableTerm b)
        {
            return a.Identifier == b.Identifier;
        }

        private bool AreBasicTermsEqual(BasicTerm a, BasicTerm b)
        {
           if (a.Functor != b.Functor)
           {
                return false;
           }

           if (a.IsNegated != b.IsNegated)
           {
                return false;
           }

           if (a.Children.Count() != b.Children.Count())
           {
                return false;
           }

            bool areAllChildrenEqual = true;

           for(int i = 0; i < a.Children.Count(); i++)
           {
                areAllChildrenEqual &= AreEqual(a.Children.ElementAt(i), b.Children.ElementAt(i));
           }

           return areAllChildrenEqual;
        }
    }
}
