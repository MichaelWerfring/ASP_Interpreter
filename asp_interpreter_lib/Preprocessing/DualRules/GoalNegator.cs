using Antlr4.Runtime.Atn;
using Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Types.TypeVisitors.Copy;

namespace Asp_interpreter_lib.Preprocessing.DualRules;

public class GoalNegator
{
    private static GoalToBinaryOperationConverter _binOpConverter = new();

    private static GoalToLiteralConverter _literalConverter = new();

    private static BinaryOperatorNegator _binaryOperatorNegator = new();

    private static TermCopyVisitor _termCopyVisitor = new();

    public static Goal Negate(Goal goal, bool wrapInNot = false)
    {
        var literal = goal.Accept(_literalConverter);

        if (literal.HasValue)
        {
            var actualLiteral = literal.GetValueOrThrow();

            //Copy the terms
            var terms =
                actualLiteral.Terms.Select(t => t.Accept(_termCopyVisitor).
                    GetValueOrThrow("Failed to parse term!")).ToList();

            if (!wrapInNot)
            {
                return new Literal(
                    actualLiteral.Identifier.ToString(),
                    !actualLiteral.HasNafNegation,
                    actualLiteral.HasStrongNegation,
                    terms);
            }

            if (actualLiteral.HasNafNegation)
            {
                return new Literal(
                    actualLiteral.Identifier.ToString(),
                    false,
                    actualLiteral.HasStrongNegation,
                    terms);
            }

            return new Literal("not", false, false,
                [new BasicTerm(
                    (actualLiteral.HasStrongNegation ? "-" : "") + actualLiteral.Identifier.ToString(),
                    actualLiteral.Terms)]);
        }

        //Convert goal to binary operation
        var binaryOperation = goal.Accept(_binOpConverter)
            .GetValueOrThrow("The value must be either a literal or a binary operation!");

        //Create new Binary Operation and negate just the operator
        var newBinaryOperation = new BinaryOperation(
            binaryOperation.Left,
            binaryOperation.BinaryOperator.Accept(_binaryOperatorNegator).
                GetValueOrThrow("Failed to negate binary operator!"),
            binaryOperation.Right);

        return newBinaryOperation;
    }
}
