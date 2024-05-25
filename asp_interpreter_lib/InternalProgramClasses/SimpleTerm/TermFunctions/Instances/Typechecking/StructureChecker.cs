using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;

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
