using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Util.ErrorHandling;

namespace asp_interpreter_lib.Types.TypeVisitors;

public class VariableFinder : TypeBaseVisitor<List<VariableTerm>>
{
    public override IOption<List<VariableTerm>> Visit(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);
        List<VariableTerm> variableTerms = [];
        foreach (var statement in program.Statements)
        {
            statement.Accept(this).IfHasValue(v =>
            {
                variableTerms.AddRange(v);
            });
        }

        program.Query.GetValueOrThrow("Inable to parse query!").Accept(this).IfHasValue(v =>
        {
            variableTerms.AddRange(v);
        });
        
        return new Some<List<VariableTerm>>(variableTerms);
    }

    public override IOption<List<VariableTerm>> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        List<VariableTerm> variables = [];

        if (statement.HasHead)
        {
            statement.Head.GetValueOrThrow().Accept(this).
                IfHasValue(v => variables.AddRange(v));
        }

        if (statement.HasBody)
        {
            foreach (var goal in statement.Body)
            {
                goal.Accept(this).IfHasValue(v => variables.AddRange(v));
            }
        }
        
        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Literal literal)
    {
        ArgumentNullException.ThrowIfNull(literal);
        List<VariableTerm> variables = [];
        foreach (var term in literal.Terms)
        {
            term.Accept(this).IfHasValue(v =>
            {
                variables.AddRange(v);
            });
        }

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Plus plus)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(Minus minus)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(Multiply multiply)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(Divide divide)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(BinaryOperation binaryOperation)
    {
        ArgumentNullException.ThrowIfNull(binaryOperation);
        List<VariableTerm> variables = [];
        
        binaryOperation.Left.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });
        
        binaryOperation.Right.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Disunification _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(Equality _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(GreaterOrEqualThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(GreaterThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(LessOrEqualThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(LessThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(Is _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(AnonymusVariableTerm anonymusVariable)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(VariableTerm variable)
    {
        return new Some<List<VariableTerm>>([variable]);
    }

    public override IOption<List<VariableTerm>> Visit(ArithmeticOperationTerm arithmeticOperation)
    {
        ArgumentNullException.ThrowIfNull(arithmeticOperation);

        List<VariableTerm> variables = [];
        variables.AddRange(arithmeticOperation.Left.Accept(this).
            GetValueOrThrow("Cannot get variables from arithmetic operation!"));
        variables.AddRange(arithmeticOperation.Right.Accept(this).
            GetValueOrThrow("Cannot get variables from arithmetic operation!"));
        
        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(BasicTerm basicTerm)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);
        List<VariableTerm> variables = [];

        foreach (var term in basicTerm.Terms)
        {
            term.Accept(this).IfHasValue(v =>
            {
                variables.AddRange(v);
            });
        }

        return new Some<List<VariableTerm>>(variables);
    }
    
    public override IOption<List<VariableTerm>> Visit(StringTerm _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(NumberTerm _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(NegatedTerm innerTerm)
    {
        ArgumentNullException.ThrowIfNull(innerTerm);
        List<VariableTerm> variables = [];
        innerTerm.Term.Accept(this).IfHasValue(v =>
        {
            variables = v;
        });

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(ParenthesizedTerm innerTerm)
    {
        ArgumentNullException.ThrowIfNull(innerTerm);
        List<VariableTerm> variables = [];
        innerTerm.Term.Accept(this).IfHasValue(v =>
        {
            variables = v;
        });

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(RecursiveList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        List<VariableTerm> terms = [];

        list.Head.Accept(this).IfHasValue(v => terms.AddRange(v));
        list.Tail.Accept(this).IfHasValue(v => terms.AddRange(v));

        return new Some<List<VariableTerm>>(terms);
    }

    public override IOption<List<VariableTerm>> Visit(ConventionalList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        List<VariableTerm> variables = [];
        foreach (var term in list.Terms)
        {
            term.Accept(this).IfHasValue(v =>
            {
                variables.AddRange(v);
            });
        }

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Forall _)
    {
        return new Some<List<VariableTerm>>([]);
    }
}