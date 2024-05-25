namespace Asp_interpreter_lib.InternalProgramClasses.Database;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;

public interface IDatabase
{
    public IEnumerable<IEnumerable<Structure>> GetPotentialUnifications(Structure term);
}
