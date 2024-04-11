using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_lib.SLDSolverClasses.StandardSolver.SubstitutionPostProcessing
{
    internal class TermFromMappingBuilder : ISimpleTermArgsVisitor<ISimpleTerm, Dictionary<Variable, ISimpleTerm>>
    {
        private SimpleTermCloner _cloner = new SimpleTermCloner();

        public ISimpleTerm BuildTerm(ISimpleTerm term, Dictionary<Variable, ISimpleTerm> mapping)
        {
            ArgumentNullException.ThrowIfNull(term, nameof(term));
            ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));

            return term.Accept(this, mapping);
        }

        public ISimpleTerm Visit(Variable variableTerm, Dictionary<Variable, ISimpleTerm> mapping)
        {
            if (!mapping.ContainsKey(variableTerm)) { return _cloner.Clone(variableTerm); }

            return mapping[variableTerm].Accept(this, mapping);
        }

        public ISimpleTerm Visit(Structure basicTerm, Dictionary<Variable, ISimpleTerm> mapping)
        {
            var newChildren = new ISimpleTerm[basicTerm.Children.Count()];

            for (int i = 0; i < basicTerm.Children.Count(); i++)
            {
                newChildren[i] = basicTerm.Children.ElementAt(i).Accept(this, mapping);
            }

            return new Structure(basicTerm.Functor.GetCopy(), newChildren);
        }
    }
}
