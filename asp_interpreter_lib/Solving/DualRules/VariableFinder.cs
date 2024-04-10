using System.Reflection.Metadata.Ecma335;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.ArithmeticOperations;
using asp_interpreter_lib.Types.BinaryOperations;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;

namespace asp_interpreter_lib.Solving.DualRules;

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

        program.Query.ClassicalLiteral.Accept(this).IfHasValue(v =>
        {
            variableTerms.AddRange(v);
        });
        
        return new Some<List<VariableTerm>>(variableTerms);
    }

    public override IOption<List<VariableTerm>> Visit(Query query)
    {
        ArgumentNullException.ThrowIfNull(query);
        return query.ClassicalLiteral.Accept(this);
    }

    public override IOption<List<VariableTerm>> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        List<VariableTerm> variables = [];

        if (statement.HasHead)
        {
            statement.Head.Accept(this).IfHasValue(v =>
            {
                variables.AddRange(v);
            });
        }

        if (statement.HasBody)
        {
            statement.Body.Accept(this).IfHasValue(v =>
            {
                variables.AddRange(v);
            });   
        }

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Head head)
    {
        ArgumentNullException.ThrowIfNull(head);
        return head.Literal?.Accept(this) ?? new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(Body body)
    {
        ArgumentNullException.ThrowIfNull(body);
        List<VariableTerm> variables = [];
        foreach (var literal in body.Literals)
        {
            literal.Accept(this).IfHasValue(v =>
            {
                variables.AddRange(v);
            });
        }

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(NafLiteral nafLiteral)
    {
        ArgumentNullException.ThrowIfNull(nafLiteral);
        return nafLiteral.ClassicalLiteral?.Accept(this) ?? new Some<List<VariableTerm>>([]);
    }

    public override IOption<List<VariableTerm>> Visit(ClassicalLiteral literal)
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
        ArgumentNullException.ThrowIfNull(plus);
        List<VariableTerm> variables = [];
        
        plus.Left.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });
        
        plus.Right.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Minus minus)
    {
        ArgumentNullException.ThrowIfNull(minus);
        List<VariableTerm> variables = [];
        
        minus.Left.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });
        
        minus.Right.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Multiply multiply)
    {
        ArgumentNullException.ThrowIfNull(multiply);
        List<VariableTerm> variables = [];
        
        multiply.Left.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });
        
        multiply.Right.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });

        return new Some<List<VariableTerm>>(variables);
    }

    public override IOption<List<VariableTerm>> Visit(Divide divide)
    {
        ArgumentNullException.ThrowIfNull(divide);
        List<VariableTerm> variables = [];
        
        divide.Left.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });
        
        divide.Right.Accept(this).IfHasValue(v =>
        {
            variables.AddRange(v);
        });

        return new Some<List<VariableTerm>>(variables);
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

        return new Some<List<VariableTerm>>([]);
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

    public override IOption<List<VariableTerm>> Visit(Forall _)
    {
        return new Some<List<VariableTerm>>([]);
    }
}