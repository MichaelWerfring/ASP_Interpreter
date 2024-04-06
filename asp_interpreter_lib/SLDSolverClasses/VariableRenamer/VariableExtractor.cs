using asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SLDSolverClasses.VariableRenamer
{
    public class VariableExtractor : IInternalTermVisitor<IEnumerable<Variable>>
    {
        public HashSet<Variable> GetVariableNames(IInternalTerm term)
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

        public IEnumerable<Variable> Visit(Integer integer)
        {
            ArgumentNullException.ThrowIfNull(integer);

            return Enumerable.Empty<Variable>();
        }
    }
}
