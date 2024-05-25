using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

namespace Asp_interpreter_lib.InternalProgramClasses.Database;

public interface IDatabase
{
    public IEnumerable<IEnumerable<Structure>> GetPotentialUnifications(Structure term);
}
