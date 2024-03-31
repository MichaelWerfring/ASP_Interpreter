using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;

namespace asp_interpreter_lib.Unification
{
    public class ClassicalLiteralToTermConverter
    {
        public ITerm Convert(ClassicalLiteral literal)
        {
            return new BasicTerm(literal.Identifier, literal.Terms);
        }
    }
}
