using System.Text;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types.Terms;

public class ConventionalList : ListTerm
{
    public ConventionalList()
    {
        Terms = [];
    }
    
    public ConventionalList(List<ITerm> terms)
    {
        Terms = terms;
    }
    
    public List<ITerm> Terms { get; private set; }

    public override string ToString()
    {
        StringBuilder sb = new();
        
        sb.Append("[");
        
        for (int i = 0; i < Terms.Count; i++)
        {
            sb.Append(Terms[i].ToString());
            if (i < Terms.Count - 1)
            {
                sb.Append(", ");
            }
        }
        
        sb.Append("]");
        
        return sb.ToString();
    }

    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}