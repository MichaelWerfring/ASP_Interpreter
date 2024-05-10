﻿using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Concatenation.Exceptions;

namespace asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Functions;

public class ProhibitedValuesMismatchException : UpdateException
{
    public ProhibitedValuesMismatchException(string message) : base(message)
    { }
}