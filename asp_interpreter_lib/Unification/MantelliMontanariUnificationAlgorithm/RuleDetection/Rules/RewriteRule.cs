﻿using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.SimplifiedTerm;
using asp_interpreter_lib.Types.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.Unification.MantelliMontanariUnificationAlgorithm.CaseDetection.Rules
{
    public class RewriteRule : IMMRule
    {
        public IOption<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>> ApplyRule
        (
            (ISimplifiedTerm, ISimplifiedTerm) equation,
            IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)> equations
        )
        {
            ArgumentNullException.ThrowIfNull(equation);
            ArgumentNullException.ThrowIfNull(equations);
            if (!equations.Contains(equation))
            {
                throw new ArgumentException(nameof(equations), $"Must contain {nameof(equation)}");
            }

            var newEquations = equations.ToList();
            newEquations.Remove(equation);
            newEquations.Add((equation.Item2, equation.Item1));

            return new Some<IEnumerable<(ISimplifiedTerm, ISimplifiedTerm)>>(newEquations);
        }
    }
}
