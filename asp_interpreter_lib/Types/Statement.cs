using System.Globalization;
using System.Text;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.Types;

public class Statement : IVisitableType
{
    public bool HasBody => Body.Count != 0;
    public bool HasHead => Head.HasValue;
    
    // Empty per default
    public IOption<Literal> Head { get; set; } = new None<Literal>();
    public List<Goal> Body { get; private set; } = new([]);

    public void AddHead(Literal head)
    {
        ArgumentNullException.ThrowIfNull(head);
        if (HasHead)
        {
            throw new ArgumentException("A statement can only have one head");
        }        
        Head = new Some<Literal>(head);
    }
    
    public void AddBody(List<Goal> body)
    {
        ArgumentNullException.ThrowIfNull(body);
        if (HasBody)
        {
            throw new ArgumentException("A statement can only have one body");
        }
        Body = body;
    }

    public override string ToString()
    {
        if (HasBody && HasHead)
        {
            return $"{Head.GetValueOrThrow().ToString()} :- {GetBodyAsString()}.";
        }

        if (HasBody && !HasHead)
        {
            return ":- " + GetBodyAsString() + ".";
        }
        
        if(!this.HasBody && !this.HasHead)
        {
            return string.Empty;
        }

        return $"{Head.GetValueOrThrow().ToString()}.";
    }

    private string GetBodyAsString()
    {
        var builder = new StringBuilder();

        if (Body.Count < 1)
        {
            return "";
        }

        builder.Append(Body[0].ToString());
        for (var index = 1; index < Body.Count; index++)
        {
            var goal = Body[index];
            builder.Append(", ");
            builder.Append(goal.ToString());
        }

        return builder.ToString();
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}
