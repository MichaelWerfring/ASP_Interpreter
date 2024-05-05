using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

public class SimpleTermFlattener : ISimpleTermArgsVisitor<List<ISimpleTerm>>
{
    public List<ISimpleTerm> ToList(ISimpleTerm term)
    {
        var list = new List<ISimpleTerm>();
        term.Accept(this, list);
        return list;
    }

    public void Visit(Variable variableTerm, List<ISimpleTerm> arguments)
    {
        arguments.Add(variableTerm);
    }

    public void Visit(Structure basicTerm, List<ISimpleTerm> arguments)
    {
        arguments.Add(basicTerm);

        foreach (var childTerm in basicTerm.Children)
        {
            childTerm.Accept(this, arguments);
        }
    }

    public void Visit(Integer integer, List<ISimpleTerm> arguments)
    {
        arguments.Add(integer);
    }
}
