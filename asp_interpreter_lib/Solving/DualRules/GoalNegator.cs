using Antlr4.Runtime.Atn;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving.DualRules;

public class GoalNegator
{
    private static GoalToBinaryOperationConverter _binOpConverter = new();
    
    private static GoalToLiteralConverter _literalConverter = new();

    private static BinaryOperatorNegator _binaryOperatorNegator = new();
    
    private static TermCopyVisitor _termCopyVisitor = new();
    
    public static Goal Negate(Goal goal)
    {
        var literal = goal.Accept(_literalConverter);
        
        if (literal.HasValue)
        {
            var actualLiteral = literal.GetValueOrThrow();
            
            //Copy the terms
            var terms =
                actualLiteral.Terms.Select(t=> t.Accept(_termCopyVisitor).
                    GetValueOrThrow("Failed to parse term!")).ToList();
                
            return new Literal(
                actualLiteral.Identifier.ToString(),
                !actualLiteral.HasNafNegation,
                actualLiteral.HasStrongNegation,
                terms);
        }
        
        //Convert goal to bino operation
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