using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms
{
    public class Integer : IInternalTerm
    {
        public Integer(int integer)
        {
            Number = integer;
        }

        public int Number { get; }

        public void Accept(IInternalTermVisitor visitor)
        {
            visitor.Visit(this);
        }

        public T Accept<T>(IInternalTermVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public TResult Accept<TResult, TArgs>(IInternalTermVisitor<TResult, TArgs> visitor, TArgs arguments)
        {
            return visitor.Visit(this, arguments);
        }

        public override string ToString()
        {
           return Number.ToString();
        }
    }
}
