using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;

namespace asp_interpreter_lib.InternalProgramClasses.Database;

public interface IDatabase
{
    public IEnumerable<IEnumerable<ISimpleTerm>> GetMatchingClauses(ISimpleTerm term);

}
