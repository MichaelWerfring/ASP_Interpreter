using asp_interpreter_lib.Types.BinaryOperations;

namespace asp_interpreter_lib.Types;

public class NafLiteral
{
    private ClassicalLiteral _classicalLiteral;
    private BinaryOperation _builtinAtom;

    public NafLiteral(ClassicalLiteral literal, bool isNafNegated)
    {
        ClassicalLiteral = literal;
        IsClassicalLiteral = true;
        IsNafNegated = isNafNegated;
    }

    public NafLiteral(BinaryOperation atom)
    {
        IsBuiltinAtom = true;
        BuiltinAtom = atom;
    }
    
    public NafLiteral()
    {
    }

    public bool IsBuiltinAtom { get; private set; }

    public bool IsClassicalLiteral { get; private set; }

    public bool IsNafNegated { get; private set; }

    public ClassicalLiteral ClassicalLiteral
    {
        get => _classicalLiteral;
        private set => _classicalLiteral = value ?? throw new ArgumentNullException(nameof(ClassicalLiteral));
    }

    public BinaryOperation BuiltinAtom
    {
        get => _builtinAtom;
        private set => _builtinAtom = value ?? throw new ArgumentNullException(nameof(BuiltinAtom));
    }
    
    public void AddClassicalLiteral(ClassicalLiteral literal, bool isNafNegated)
    {
        ArgumentNullException.ThrowIfNull(literal);
        
        if (IsBuiltinAtom)
        {
            throw new InvalidOperationException("Cannot add a classical literal to a builtin atom!");
        }
        
        if (IsClassicalLiteral)
        {
            throw new InvalidOperationException("The classical literal has already been set!");
        }
        
        ClassicalLiteral = literal;
        IsNafNegated = isNafNegated;
        IsClassicalLiteral = true;
    }
    
    public void AddBuiltinAtom(BinaryOperation atom)
    {
        ArgumentNullException.ThrowIfNull(atom);
        
        if (IsClassicalLiteral)
        {
            throw new InvalidOperationException("Cannot add a builtin atom to a classical literal!");
        }

        if (IsBuiltinAtom)
        {
            throw new InvalidOperationException("The builtin atom has already been set!");
        }
        
        BuiltinAtom = atom;
        IsBuiltinAtom = true;
    }
}