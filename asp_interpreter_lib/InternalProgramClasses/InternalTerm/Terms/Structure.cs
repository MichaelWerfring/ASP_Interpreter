using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.ListExtensions;
using System.Text;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms
{
    public class Structure : IInternalTerm
    {
        public Structure(string functor, IEnumerable<IInternalTerm> children)
        {
            ArgumentException.ThrowIfNullOrEmpty(functor);
            ArgumentNullException.ThrowIfNull(children);

            Functor = functor;
            Children = children;
        }

        public string Functor { get; }

        public IEnumerable<IInternalTerm> Children { get; }

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
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Functor);
            stringBuilder.Append('(');
            stringBuilder.Append(Children.ToList().ListToString());
            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }
    }
}
