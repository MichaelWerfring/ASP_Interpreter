using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Visitor;
using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram
{
    public class ArithmeticOperationConverter : TypeBaseVisitor<ISimpleTerm>
    {
        private TermConverter _converter;
        private ArithmeticOperationTerm _term;

        public ArithmeticOperationConverter(ArithmeticOperationTerm term, TermConverter termConverter)
        {
            ArgumentNullException.ThrowIfNull(term);
            ArgumentNullException.ThrowIfNull(termConverter);

            _term = term;
            _converter = termConverter;
        }

        public ISimpleTerm Convert()
        {
            var resultMaybe = _term.Operation.Accept(this);

            return resultMaybe.GetValueOrThrow();
        }

        public override IOption<ISimpleTerm> Visit(Divide divide)
        {
            var left = _converter.Convert(_term.Left);
            var right = _converter.Convert(_term.Right);

            return new Some<ISimpleTerm>(new Structure("/", [left, right], false));
        }

        public override IOption<ISimpleTerm> Visit(Minus minus)
        {
            var left = _converter.Convert(_term.Left);
            var right = _converter.Convert(_term.Right);

            return new Some<ISimpleTerm>(new Structure("+", [left, right], false));
        }

        public override IOption<ISimpleTerm> Visit(Plus plus)
        {
            var left = _converter.Convert(_term.Left);
            var right = _converter.Convert(_term.Right);

            return new Some<ISimpleTerm>(new Structure("-", [left, right], false));
        }

        public override IOption<ISimpleTerm> Visit(Multiply multiply)
        {
            var left = _converter.Convert(_term.Left);
            var right = _converter.Convert(_term.Right);

            return new Some<ISimpleTerm>(new Structure("*", [left, right], false));
        }
    }
}
