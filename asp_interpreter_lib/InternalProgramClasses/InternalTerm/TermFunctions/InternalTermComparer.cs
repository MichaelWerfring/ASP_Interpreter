using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions
{
    public class InternalTermComparer : IInternalTermVisitor<bool, IInternalTerm>, IEqualityComparer<IInternalTerm>
    {
        public bool Equals(IInternalTerm? x, IInternalTerm? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            return x.Accept(this, y);
        }

        public int GetHashCode([DisallowNull] IInternalTerm obj)
        {
            return obj.GetHashCode();
        }

        public bool Visit(Variable a, IInternalTerm b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);

            Variable bVar;
            try
            {
                bVar = (Variable)b;
            }
            catch
            {
                return false;
            }

            return a.Identifier == bVar.Identifier;
        }

        public bool Visit(Structure a, IInternalTerm b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);

            Structure bStruct;
            try
            {
                bStruct = (Structure)b;
            }
            catch
            {
                return false;
            }

            if (a.Functor != bStruct.Functor)
            {
                return false;
            }

            if (a.Children.Count() != bStruct.Children.Count())
            {
                return false;
            }

     

            for (int i = 0; i < a.Children.Count(); i++)
            {

                if (!Equals(a.Children.ElementAt(i), bStruct.Children.ElementAt(i)))
                {
                    return false;
                }

            }

            return true;
        }

        public bool Visit(Integer a, IInternalTerm b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);

            Integer bInt;
            try
            {
                bInt = (Integer)b;
            }
            catch
            {
                return false;
            }

            return a.Number == bInt.Number;
        }
    }
}
