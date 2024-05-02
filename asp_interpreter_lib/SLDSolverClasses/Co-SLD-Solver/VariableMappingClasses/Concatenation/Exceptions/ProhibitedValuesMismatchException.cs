using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Concatenation.Exceptions;
using System.Runtime.Serialization;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class ProhibitedValuesMismatchException : ConcatenationException
{
    public ProhibitedValuesMismatchException(string message) : base(message)
    { }
}