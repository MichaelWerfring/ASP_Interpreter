using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;

namespace asp_interpreter_lib.SLDSolverClasses.StandardSolver.VariableRenamer
{
    public class VariableExtractor : ISimpleTermVisitor<IEnumerable<Variable>>
    {
        public HashSet<Variable> GetVariableNames(ISimpleTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            return term.Accept(this).ToHashSet(new VariableComparer());
        }

        public IEnumerable<Variable> Visit(Variable variableTerm)
        {
            ArgumentNullException.ThrowIfNull(variableTerm);

            return new List<Variable>() { variableTerm };
        }

        public IEnumerable<Variable> Visit(Structure basicTerm)
        {
            ArgumentNullException.ThrowIfNull(basicTerm);

            IEnumerable<Variable> result = new List<Variable>();

            foreach (var child in basicTerm.Children)
            {
                var childVars = child.Accept(this);

                result = result.Concat(childVars);
            }

            return result;
        }
    }
}
