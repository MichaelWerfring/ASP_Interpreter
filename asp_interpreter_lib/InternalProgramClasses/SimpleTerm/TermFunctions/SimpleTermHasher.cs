using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermHasher : ISimpleTermVisitor<string>
{
    public int Hash(ISimpleTerm term)
    {
        return term.Accept(this).Length.GetHashCode();
    }

    public  string Visit(Variable variableTerm)
    {
        return $"V:{variableTerm.Identifier}";
    }

    public string Visit(Structure basicTerm)
    {
        return $"S:{basicTerm.Functor}:C:{basicTerm.Children.Select(x => x.Accept(this))}";
    }

    public string Visit(Integer integer)
    {
        return $"I:{integer.Value}";
    }
}
