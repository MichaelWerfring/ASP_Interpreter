using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection
{
    public class ReducabilityDecider : IInternalTermVisitor<bool, IInternalTerm>
    {
        public bool CanReduce(IInternalTerm a, IInternalTerm b)
        {
            return a.Accept(this, b);
        }

        public bool Visit(Variable variableTerm, IInternalTerm arguments)
        {
            ArgumentNullException.ThrowIfNull(variableTerm);
            ArgumentNullException.ThrowIfNull(arguments);

            return false;
        }

        public bool Visit(Structure basicTerm, IInternalTerm arguments)
        {
            ArgumentNullException.ThrowIfNull(basicTerm);
            ArgumentNullException.ThrowIfNull(arguments);

            if (arguments is Structure s)
            {
                return basicTerm.Functor == s.Functor && basicTerm.Children.Count() == s.Children.Count();
            }
            else
            {
                return false;
            }
        }

        public bool Visit(Integer integer, IInternalTerm arguments)
        {
            ArgumentNullException.ThrowIfNull(integer);
            ArgumentNullException.ThrowIfNull(arguments);

            if (arguments is Integer s)
            {
                return integer.Number == s.Number;
            }
            else
            {
                return false;
            }
        }
    }
}
