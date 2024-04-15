using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.General;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures.List;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram;

public class StringTermConverter
{
    public ISimpleTerm Convert(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return ConvertString(term.Value);
    }

    private ISimpleTerm ConvertString(string str)
    {
        if (str.Equals(string.Empty))
        {
            return new Nil();
        }

        var head = new Structure(str.First().ToString(), Enumerable.Empty<ISimpleTerm>());
        var tail = ConvertString(new string(str.Skip(1).ToArray()));

        return new List(head , tail );
    }
}
