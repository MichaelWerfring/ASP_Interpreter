using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving.DualRules;

public class HeadRewriter : TypeBaseVisitor<Statement>
{
    private readonly string _commonPrefix;

    private readonly Statement _statement;
    
    private readonly HashSet<string> _variables;

    private readonly TermCopyVisitor _termCopyVisitor = new();
    
    public HeadRewriter(string commonPrefix, Statement statement)
    {
        _commonPrefix = commonPrefix;
        _statement = statement;
        _variables = new HashSet<string>();
        //var variableGetter = new VariableFinder();
        //var terms = statement.Accept(variableGetter).GetValueOrThrow("Cannot retrieve variables from program!");
        //terms.ForEach(t => _variables.Add(t.Identifier));
    }


    public override IOption<Statement> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        
        statement.Head.Accept(this);

        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(Head head)
    {
        ArgumentNullException.ThrowIfNull(head);
        
        head.Literal?.Accept(this);
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(NafLiteral nafLiteral)
    {
        ArgumentNullException.ThrowIfNull(nafLiteral);
        
        nafLiteral.ClassicalLiteral.Accept(this);
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(ClassicalLiteral classicalLiteral)
    {
        ArgumentNullException.ThrowIfNull(classicalLiteral);

        for (var index = 0; index < classicalLiteral.Terms.Count; index++)
        {
            var term = classicalLiteral.Terms[index];
            term.Accept(this);
        }

        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        if (_variables.Add(term.Identifier))
        {
            return new Some<Statement>(_statement);    
        }
        
        var newVariable = new VariableTerm(
            ASPExtensions.GenerateUniqeName(term.Identifier, _variables, _commonPrefix));

        var oldTerm = term.Accept(_termCopyVisitor).GetValueOrThrow("Cannot copy term!");
        
        
        //Rewrite the head
        term.Identifier = newVariable.Identifier;
            
        //Rewrite the body
        _statement.Body.Literals.Insert(0,new NafLiteral(new BinaryOperation(
            term, new Equality(), oldTerm)));
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        foreach (var child in term.Terms)
        {
            child.Accept(this);
        }
        
        return new Some<Statement>(_statement);
    }
    
    public override IOption<Statement> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        //Accept ToString/Null values for now
        var newHeadVariable = new VariableTerm(
            ASPExtensions.GenerateUniqeName(term.ToString() ?? "", _variables, _commonPrefix));
                
        //replace head
        //_statement.Head.Literal?.Terms[_statement.Head.Literal?.Terms.IndexOf(term)] = newHeadVariable;
        int i = _statement.Head.Literal?.Terms.IndexOf(term) ?? 
                throw new InvalidOperationException("The given term is not contained in the head!");
        _statement.Head.Literal.Terms[i] = newHeadVariable;
                
        //replace body
        _statement.Body.Literals.Insert(0,new NafLiteral(new BinaryOperation(
            newHeadVariable, new Equality(), term)));
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        //Accept ToString/Null values for now
        var newHeadVariable = new VariableTerm(
            ASPExtensions.GenerateUniqeName(term.ToString() ?? "", _variables, _commonPrefix));
                
        //replace head
        //_statement.Head.Literal?.Terms[_statement.Head.Literal?.Terms.IndexOf(term)] = newHeadVariable;
        int i = _statement.Head.Literal?.Terms.IndexOf(term) ?? 
                throw new InvalidOperationException("The given term is not contained in the head!");
        _statement.Head.Literal.Terms[i] = newHeadVariable;
                
        //replace body
        _statement.Body.Literals.Insert(0,new NafLiteral(new BinaryOperation(
            newHeadVariable, new Equality(), term)));
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        term.Term.Accept(this);
        
        return new Some<Statement>(_statement);
    }

    public override IOption<Statement> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        term.Term.Accept(this);
        return new Some<Statement>(_statement);
    }
}