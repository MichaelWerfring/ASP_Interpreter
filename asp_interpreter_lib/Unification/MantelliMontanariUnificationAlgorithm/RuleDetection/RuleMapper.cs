using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types.TermEquivalence;
using asp_interpreter_lib.Types.TermFunctionality;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection
{
    public class RuleMapper
    {
        private TermEquivalenceChecker _termEquivalenceChecker;
        private TermContainsChecker _termContainsChecker;

        /// <summary>
        /// LeftContainsRight the transformation rules in the following order:
        /// rewrite, erasure, reduction, elimination.
        /// </summary>
        private IMMRule[] _rules;

        private bool _doOccursCheck;

        public RuleMapper(bool doOccursCheck)
        {
            _termEquivalenceChecker = new TermEquivalenceChecker();
            _termContainsChecker = new TermContainsChecker();

            _rules = new IMMRule[4];
            _rules[0] = new RewriteRule();
            _rules[1] = new ErasureRule();
            _rules[2] = new ReductionRule();
            _rules[3] = new EliminationRule();

            _doOccursCheck = doOccursCheck;
        }

        public IOption<IMMRule> GetAppropriateRule((ITerm, ITerm) equation, IEnumerable<(ITerm, ITerm)> equations)
        {
            ArgumentNullException.ThrowIfNull(equation);
            ArgumentNullException.ThrowIfNull(equations);
            if(! equations.Contains(equation))
            {
                throw new ArgumentException(nameof(equations), $"Must contain {nameof(equation)}.");
            }


            if(IsRewriteCase(equation))
            {
                return new Some<IMMRule>(_rules[0]);
            }

            if(IsErasureCase(equation))
            {
                return new Some<IMMRule>(_rules[1]);
            }

            if (IsReductionCase(equation))
            {
                return new Some<IMMRule>(_rules[2]);
            }


            if (IsEliminationCase(equation, equations, _doOccursCheck))
            {
                return new Some<IMMRule>(_rules[3]);
            }

            return new None<IMMRule>();
        }

        private bool IsRewriteCase( (ITerm, ITerm) equation)
        {     
            if (equation.Item1.GetType() == typeof(VariableTerm))
            {
                return false;
            }

            if (equation.Item2.GetType() != typeof(VariableTerm))
            {
                return false;
            }

            return true;
        }

        private bool IsErasureCase( (ITerm, ITerm) equation)
        {
            VariableTerm aVar;
            VariableTerm bVar;
            try
            {
                aVar = (VariableTerm) equation.Item1;
                bVar = (VariableTerm) equation.Item2;
            }
            catch
            {
                return false;
            }

            if (aVar.Identifier != bVar.Identifier)
            {
                return false;
            }

            return true;
        }

        private bool IsReductionCase((ITerm, ITerm) equation)
        {
            if (equation.Item1.GetType() == typeof(VariableTerm))
            {
                return false;
            }

            if (equation.Item2.GetType() == typeof(VariableTerm))
            {
                return false;
            }

            return true;
        }

        private bool IsEliminationCase((ITerm, ITerm) equation, IEnumerable<(ITerm, ITerm)> equations, bool doOccursCheck)
        {
            if(equation.Item1.GetType() != typeof(VariableTerm))
            {
                return false;
            }

            if (_termEquivalenceChecker.AreEqual(equation.Item1,equation.Item2))
            {
                return false;
            }
            
            if( doOccursCheck && _termContainsChecker.LeftContainsRight(equation.Item2, equation.Item1))
            {
                return false;
            }

            IEnumerable<(ITerm, ITerm)> filteredList = equations.ToList();
            filteredList = filteredList.Where((eq) =>
            {
                return eq != equation;
            })
                .Where((eq) =>
            {
                return _termContainsChecker.LeftContainsRight(eq.Item1, equation.Item1)
                ||
                _termContainsChecker.LeftContainsRight(eq.Item2, equation.Item1);
            });

            if(filteredList.Count() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
