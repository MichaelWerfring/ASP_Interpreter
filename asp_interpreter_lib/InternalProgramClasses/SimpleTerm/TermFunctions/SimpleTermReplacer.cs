using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;

namespace asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

public class SimpleTermReplacer : ISimpleTermArgsVisitor<ISimpleTerm, (ISimpleTerm, ISimpleTerm)>
{
    private SimpleTermComparer _checker;
    private StructureChildrenReplacer _childrenReplacer;
    private SimpleTermCloner _termCloner;

    public SimpleTermReplacer()
    {
        _checker = new SimpleTermComparer();
        _childrenReplacer = new StructureChildrenReplacer();
        _termCloner = new SimpleTermCloner();
    }

    public ISimpleTerm Replace(ISimpleTerm termToReplaceVariablesIn, ISimpleTerm termToReplace, ISimpleTerm replacement)
    {
        ArgumentNullException.ThrowIfNull(termToReplaceVariablesIn);
        ArgumentNullException.ThrowIfNull(termToReplace);
        ArgumentNullException.ThrowIfNull(replacement);

        return termToReplaceVariablesIn.Accept(this, (termToReplace, replacement));
    }

    public ISimpleTerm Visit(Variable variableTerm, (ISimpleTerm, ISimpleTerm) arguments)
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

    public ISimpleTerm Visit(Structure structure, (ISimpleTerm, ISimpleTerm) arguments)
    {
        ArgumentNullException.ThrowIfNull(structure);
        ArgumentNullException.ThrowIfNull(arguments);
        ArgumentNullException.ThrowIfNull(arguments.Item1);
        ArgumentNullException.ThrowIfNull(arguments.Item2);

        if (_checker.Equals(structure, arguments.Item1))
        {
            return _termCloner.Clone(arguments.Item2);
        }

        var children = new ISimpleTerm[structure.Children.Count()];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = structure.Children.ElementAt(i).Accept(this, arguments);
        }

        return _childrenReplacer.Replace(structure, children);
    }
}
