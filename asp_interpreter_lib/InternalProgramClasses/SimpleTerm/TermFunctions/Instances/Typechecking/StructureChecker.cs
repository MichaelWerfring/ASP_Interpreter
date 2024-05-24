using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;

internal class StructureChecker : ISimpleTermVisitor<IOption<Structure>>
{
    public IOption<Structure> ReturnStructureOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public IOption<Structure> Visit(Variable term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Structure>();
    }

    public IOption<Structure> Visit(Structure term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new Some<Structure>(term);
    }

    public IOption<Structure> Visit(Integer term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return new None<Structure>();
    }
}
