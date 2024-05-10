using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;

internal class SimpleTermHasher : ISimpleTermVisitor<int>
{
    public int Hash(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return term.Accept(this);
    }

    public int Visit(Variable variableTerm)
    {
        return variableTerm.Identifier.GetHashCode();
    }

    public int Visit(Structure basicTerm)
    {
        int childHash = 0;

        foreach (var child in basicTerm.Children)
        {
            childHash += child.Accept(this);
        }

        return basicTerm.Functor.GetHashCode() + childHash;
    }

    public int Visit(Integer integer)
    {
        return integer.Value.GetHashCode();
    }
}
