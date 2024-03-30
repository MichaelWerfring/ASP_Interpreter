using asp_interpreter_lib.ListExtensions;
using System.Text;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types;

public class Body : IVisitableType
{
    private List<NafLiteral> _literals;

    //Does not support body consisting of bodies yet eg. grammar
    public Body(List<NafLiteral> literals)
    {
        Literals = literals;
    }

    //A body consists of any literals 
    public List<NafLiteral> Literals
    {
        get => _literals;
        private set => _literals = value ?? throw new ArgumentNullException(nameof(Literals));
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Literals.ListToString());

        return builder.ToString();
    }
    
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}