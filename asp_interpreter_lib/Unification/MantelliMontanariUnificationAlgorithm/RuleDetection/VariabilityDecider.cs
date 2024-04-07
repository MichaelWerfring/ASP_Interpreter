using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Visitor;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection
{
    public class VariabilityDecider : IInternalTermVisitor<bool>
    {
        public bool CountsAsVariable(IInternalTerm term)
        {
            ArgumentNullException.ThrowIfNull(term);

            return term.Accept(this);
        }

        public bool Visit(Variable variableTerm)
        {
            ArgumentNullException.ThrowIfNull(variableTerm);

            return true;
        }

        public bool Visit(Structure basicTerm)
        {
            ArgumentNullException.ThrowIfNull(basicTerm);

            return false;
        }

        public bool Visit(Integer integer)
        {
            ArgumentNullException.ThrowIfNull(integer);

            return false;
        }
    }
}
