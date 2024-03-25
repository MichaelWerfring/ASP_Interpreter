using System.Numerics;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Types;

public class BuiltinAtom
{
    private BinaryOperation _operation;
    private Term _left;
    private Term _right;

    public BuiltinAtom(Term left, BinaryOperation operation, Term right)
    {
        Left = left;
        Operation = operation;
        Right = right;
    }
        
    public BinaryOperation Operation
    {
        get => _operation;
        set => _operation = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Term Left
    {
        get => _left;
        set => _left = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Term Right
    {
        get => _right;
        set => _right = value ?? throw new ArgumentNullException(nameof(value));
    }
}