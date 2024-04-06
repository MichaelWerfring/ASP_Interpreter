using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;

namespace asp_interpreter_lib.ProgramToInternalProgramConversion;

public class BuiltInAtomConverter
{
    private TermConverter _converter = new TermConverter();

    public IInternalTerm Convert(BinaryOperation binaryOperation)
    {
        ArgumentNullException.ThrowIfNull(binaryOperation);

        var left = _converter.Convert(binaryOperation.Left);
        var right = _converter.Convert(binaryOperation.Right);

        return new Structure(binaryOperation.BinaryOperator!.ToString(), new List<IInternalTerm>() { left, right });
    }
}