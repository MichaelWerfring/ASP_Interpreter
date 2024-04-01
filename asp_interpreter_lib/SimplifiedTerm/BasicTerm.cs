using asp_interpreter_lib.ListExtensions;
using asp_interpreter_lib.SimplifiedTerm.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SimplifiedTerm
{
    public class BasicTerm : ISimplifiedTerm
    {
        public BasicTerm(string functor, IEnumerable<ISimplifiedTerm> children, bool isNegated)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(functor);
            ArgumentNullException.ThrowIfNull(children);
  
            Functor = functor;
            Children = children;
            IsNegated = isNegated;
        }

        public bool IsNegated { get; }

        public string Functor { get; }

        public IEnumerable<ISimplifiedTerm> Children { get; }

        public void Accept(ISimplifiedTermVisitor visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            visitor.Visit(this);
        }

        public T Accept<T>(ISimplifiedTermVisitor<T> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor);
            return visitor.Visit(this);
        }

        public override string ToString() 
        {
            StringBuilder stringBuilder = new StringBuilder();

            if(IsNegated) { stringBuilder.Append('-'); }
            stringBuilder.Append(Functor);
            stringBuilder.Append('(');
            stringBuilder.Append(Children.ToList().ListToString());
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }
    }
}
