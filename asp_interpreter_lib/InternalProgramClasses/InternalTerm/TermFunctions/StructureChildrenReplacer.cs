using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;

public class StructureChildrenReplacer
{
    public IInternalTerm Replace(Structure term, IEnumerable<IInternalTerm> newChildren)
    {
        ArgumentNullException.ThrowIfNull(term);
        ArgumentNullException.ThrowIfNull(newChildren);

        return new Structure(term.Functor, newChildren);
    }
}
