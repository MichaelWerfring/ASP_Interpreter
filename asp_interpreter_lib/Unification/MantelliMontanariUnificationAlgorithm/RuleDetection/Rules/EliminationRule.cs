using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.InternalTerm.Terms;


namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.RuleDetection.Rules
{
    public class EliminationRule : IMMRule
    {
        private InternalTermReplacer _replacer;

        public EliminationRule()
        {
            _replacer = new InternalTermReplacer();
        }

        public IOption<IEnumerable<(IInternalTerm, IInternalTerm)>> ApplyRule
        (
            (IInternalTerm, IInternalTerm) equation,
            IEnumerable<(IInternalTerm, IInternalTerm)> equations
        )
        {
            ArgumentNullException.ThrowIfNull(equation);
            ArgumentNullException.ThrowIfNull(equations);

            var newEquations = equations.ToList();
            newEquations.Remove(equation);
            newEquations = newEquations.Select
            (
                (eq) =>
                {
                    IInternalTerm left = _replacer.Replace(eq.Item1, equation.Item1, equation.Item2);
                    IInternalTerm right = _replacer.Replace(eq.Item2, equation.Item1, equation.Item2);

                    return (left, right);
                }
            )
            .ToList();

            newEquations.Add(equation);

            return new Some<IEnumerable<(IInternalTerm, IInternalTerm)>>(newEquations);
        }
    }
}
