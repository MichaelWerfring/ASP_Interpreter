using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.InternalProgram.Database;

public interface IDatabase
{
    public IEnumerable<IEnumerable<ISimpleTerm>> GetMatchingClauses(ISimpleTerm term);

}
