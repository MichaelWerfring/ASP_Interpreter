using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class StructureReducer : ISimpleTermArgsVisitor<IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>>, IStructure>
{
    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> TryReduce(IStructure term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return term.Accept(this, other);
    }

    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Visit(Structure term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        var otherAsStructureMaybe = TermFuncs.ReturnStructureOrNone(other);

        if (!otherAsStructureMaybe.HasValue)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        Structure otherAsStructure = otherAsStructureMaybe.GetValueOrThrow();

        if (term.Functor != otherAsStructure.Functor)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        if (term.Children.Count != otherAsStructure.Children.Count)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>(term.Children.Zip(otherAsStructure.Children));
    }

    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Visit(Integer term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        var otherAsIntegerMaybe = TermFuncs.ReturnIntegerOrNone(other);

        if (!otherAsIntegerMaybe.HasValue)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        Integer otherAsInteger = otherAsIntegerMaybe.GetValueOrThrow();

        if (term.Value != otherAsInteger.Value)
        {
            return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
        }

        return new Some<IEnumerable<(ISimpleTerm, ISimpleTerm)>>([]);

    }

    public IOption<IEnumerable<(ISimpleTerm, ISimpleTerm)>> Visit(Variable term, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(other);

        return new None<IEnumerable<(ISimpleTerm, ISimpleTerm)>>();
    }
}
