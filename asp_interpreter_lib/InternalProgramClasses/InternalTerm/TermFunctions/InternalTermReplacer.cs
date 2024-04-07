using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;

public class InternalTermReplacer : IInternalTermVisitor<IInternalTerm, (IInternalTerm, IInternalTerm)>
{
    private InternalTermComparer _checker;
    private StructureChildrenReplacer _childrenReplacer;
    private InternalTermCloner _termCloner;

    public InternalTermReplacer()
    {
        _checker = new InternalTermComparer();
        _childrenReplacer = new StructureChildrenReplacer();
        _termCloner = new InternalTermCloner();
    }

    public IInternalTerm Replace(IInternalTerm termToReplaceVariablesIn, IInternalTerm termToReplace, IInternalTerm replacement)
    {
        ArgumentNullException.ThrowIfNull(termToReplaceVariablesIn);
        ArgumentNullException.ThrowIfNull(termToReplace);
        ArgumentNullException.ThrowIfNull(replacement);

        return termToReplaceVariablesIn.Accept(this, (termToReplace, replacement));
    }

    public IInternalTerm Visit(Variable variableTerm, (IInternalTerm, IInternalTerm) arguments)
    {
        ArgumentNullException.ThrowIfNull(variableTerm);
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentNullException.ThrowIfNull(arguments.Item1);
        ArgumentNullException.ThrowIfNull(arguments.Item2);

        if (_checker.Equals(variableTerm, arguments.Item1))
        {
            return _termCloner.Clone(arguments.Item2);
        }
        else
        {
            return _termCloner.Clone(variableTerm);
        }
    }

    public IInternalTerm Visit(Structure structure, (IInternalTerm, IInternalTerm) arguments)
    {
        ArgumentNullException.ThrowIfNull(structure);
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentNullException.ThrowIfNull(arguments.Item1);
        ArgumentNullException.ThrowIfNull(arguments.Item2);

        if (_checker.Equals(structure, arguments.Item1))
        {
            return _termCloner.Clone(arguments.Item2);
        }

        var children = new IInternalTerm[structure.Children.Count()];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = structure.Children.ElementAt(i).Accept(this, arguments);
        }

        return _childrenReplacer.Replace(structure, children);
    }

    public IInternalTerm Visit(Integer integer, (IInternalTerm, IInternalTerm) arguments)
    {
        ArgumentNullException.ThrowIfNull(integer);
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentNullException.ThrowIfNull(arguments.Item1);
        ArgumentNullException.ThrowIfNull(arguments.Item2);

        if (_checker.Equals(integer, arguments.Item1))
        {
            return _termCloner.Clone(arguments.Item2);
        }
        else
        {
            return _termCloner.Clone(integer);
        }
    }

}
