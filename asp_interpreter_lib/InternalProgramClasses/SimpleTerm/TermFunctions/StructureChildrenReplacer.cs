using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class StructureChildrenReplacer
{
    public ISimpleTerm Replace(Structure term, IEnumerable<ISimpleTerm> newChildren)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(newChildren);

        return new Structure(term.Functor, newChildren);
    }
}
