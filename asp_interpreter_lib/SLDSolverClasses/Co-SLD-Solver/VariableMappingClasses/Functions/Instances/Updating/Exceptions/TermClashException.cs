using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Concatenation.Exceptions;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class TermClashException : UpdateException
{
    public TermClashException(string message) : base(message)
    { }
}