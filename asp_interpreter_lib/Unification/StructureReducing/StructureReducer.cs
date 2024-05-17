using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Unification.StructureReducers;

public class StructureReducer : ISimpleTermArgsVisitor<IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>>, IStructure>
{
    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> TryReduce(IStructure a, IStructure b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return a.Accept(this, b);
    }

    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Visit(Structure basicTerm, IStructure arguments)
    {
        Structure b;
        try
        {
            b = (Structure)arguments;
        }
        catch
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        if
        (
            basicTerm.Functor != b.Functor
            ||
            basicTerm.Children.Count() != b.Children.Count()
        )
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>(basicTerm.Children.Zip(b.Children));
    }

    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Visit(Integer integer, IStructure arguments)
    {
        if (arguments is Integer b && integer.Value == b.Value)
        {
            return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>([]);
        }
        else
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }
    }

    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Visit(Variable variableTerm, IStructure arguments)
    {
        return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
    }
}
