using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Concatenation.Exceptions;

public abstract class ConcatenationException : Exception
{
    public ConcatenationException(string message) : base(message)
    { }
}
