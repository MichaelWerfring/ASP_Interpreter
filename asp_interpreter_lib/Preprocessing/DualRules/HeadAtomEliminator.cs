using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Preprocessing.DualRules;

//returns the term to be used at the occuring position (or same as before if no change)
//and a goal to be added to the body (else its none)
public class HeadAtomEliminator : TypeBaseVisitor<(ITerm, List<Goal>)>
{
    private readonly PrefixOptions _options;
    
    private readonly HashSet<string> _variables;

    private int _counter;
    
    public HeadAtomEliminator(PrefixOptions options, Statement statement)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(statement);
        _options = options;
        _variables = new HashSet<string>();
        _counter = 1;
    }
    
    public Statement Rewrite(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);

        if (!statement.HasHead)
        {
            return statement;
        }
        
        var head = statement.Head.GetValueOrThrow("Cannot rewrite headless statement!");

        for (var index = 0; index < head.Terms.Count; index++)
        {
            var term = head.Terms[index];
            var rewrite = term.Accept(this).GetValueOrThrow("Unable to parse head!");
            head.Terms[index] = rewrite.Item1;
            statement.Body.InsertRange(0, rewrite.Item2);
        }
        
        return statement;
    }
    
    public override IOption<(ITerm, List<Goal>)> Visit(BasicTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        var body = new BinaryOperation(newVariable, new Equality(), term);
        
        return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
    }

    public override IOption<(ITerm, List<Goal>)> Visit(AnonymousVariableTerm term)
    {
        return new Some<(ITerm, List<Goal>)>((term, []));
    }

    public override IOption<(ITerm, List<Goal>)> Visit(VariableTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        if (_variables.Add(term.Identifier))
        {
            //If the variable only occurs once in the head, we can just return it again
            return new Some<(ITerm, List<Goal>)>((term, []));    
        }
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        var body = new BinaryOperation(newVariable, new Equality(), term);

        return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
    }

    public override IOption<(ITerm, List<Goal>)> Visit(ArithmeticOperationTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        return new Some<(ITerm, List<Goal>)>((term, []));
    }
    
    public override IOption<(ITerm, List<Goal>)> Visit(StringTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        var body = new BinaryOperation(newVariable, new Equality(), term);

        return new Some<(ITerm, List<Goal>)>((newVariable,[body]));
    }

    public override IOption<(ITerm, List<Goal>)> Visit(NumberTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        var body = new BinaryOperation(newVariable, new Equality(), term);

        return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
    }

    public override IOption<(ITerm, List<Goal>)> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        
        var number = 
            term.Term.Accept(new TermToNumberConverter()).GetValueOrThrow("Cannot convert term to number!");
        
        var body = new BinaryOperation(
            newVariable, new Equality(), new NumberTerm(-number));
        
        return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
    }

    public override IOption<(ITerm, List<Goal>)> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        return term.Term.Accept(this);
    }

    public override IOption<(ITerm, List<Goal>)> Visit(RecursiveList term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        var body = new BinaryOperation(newVariable, new Equality(), term);

        return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
    }

    public override IOption<(ITerm, List<Goal>)> Visit(ConventionalList term)
    {
        ArgumentNullException.ThrowIfNull(term);
        
        var newVariable = new VariableTerm(_options.VariablePrefix + _counter++);
        var body = new BinaryOperation(newVariable, new Equality(), term);

        return new Some<(ITerm, List<Goal>)>((newVariable, [body]));
    }
}