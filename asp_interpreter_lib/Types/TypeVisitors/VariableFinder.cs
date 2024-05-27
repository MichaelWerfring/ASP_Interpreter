//-----------------------------------------------------------------------
// <copyright file="VariableFinder.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Types.ArithmeticOperations;
using Asp_interpreter_lib.Types.BinaryOperations;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Util.ErrorHandling;

public class VariableFinder : TypeBaseVisitor<List<VariableTerm>>
{
    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);
        List<VariableTerm> variableTerms =[];
        foreach (var statement in program.Statements)
        {
            statement.Accept(this).IfHasValue(variableTerms.AddRange);
        }

        program.Query.GetValueOrThrow("Inable to parse query!").Accept(this).IfHasValue(variableTerms.AddRange);

        return new Some<List<VariableTerm>>(variableTerms);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        List<VariableTerm> variables =[];

        if (statement.HasHead)
        {
            statement.Head.GetValueOrThrow().Accept(this).
                IfHasValue(variables.AddRange);
        }

        if (statement.HasBody)
        {
            foreach (var goal in statement.Body)
            {
                goal.Accept(this).IfHasValue(variables.AddRange);
            }
        }

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Literal literal)
    {
        ArgumentNullException.ThrowIfNull(literal);
        List<VariableTerm> variables =[];
        foreach (var term in literal.Terms)
        {
            term.Accept(this).IfHasValue(variables.AddRange);
        }

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Plus plus)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Minus minus)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Multiply multiply)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Divide divide)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(BinaryOperation binOp)
    {
        ArgumentNullException.ThrowIfNull(binOp);
        List<VariableTerm> variables =[];

        binOp.Left.Accept(this).IfHasValue(variables.AddRange);

        binOp.Right.Accept(this).IfHasValue(variables.AddRange);

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Disunification _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Equality _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(GreaterOrEqualThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(GreaterThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(LessOrEqualThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(LessThan _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Is _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(AnonymousVariableTerm variable)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(VariableTerm variable)
    {
        return new Some<List<VariableTerm>>([variable]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(ArithmeticOperationTerm arithmeticOperation)
    {
        ArgumentNullException.ThrowIfNull(arithmeticOperation);

        List<VariableTerm> variables =[];
        variables.AddRange(arithmeticOperation.Left.Accept(this).
            GetValueOrThrow("Cannot get variables from arithmetic operation!"));
        variables.AddRange(arithmeticOperation.Right.Accept(this).
            GetValueOrThrow("Cannot get variables from arithmetic operation!"));

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(BasicTerm basicTerm)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);
        List<VariableTerm> variables =[];

        foreach (var term in basicTerm.Terms)
        {
            term.Accept(this).IfHasValue(variables.AddRange);
        }

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(StringTerm _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(NumberTerm _)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(NegatedTerm innerTerm)
    {
        ArgumentNullException.ThrowIfNull(innerTerm);
        List<VariableTerm> variables =[];
        innerTerm.Term.Accept(this).IfHasValue(v =>
        {
            variables = v;
        });

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(ParenthesizedTerm innerTerm)
    {
        ArgumentNullException.ThrowIfNull(innerTerm);
        List<VariableTerm> variables =[];
        innerTerm.Term.Accept(this).IfHasValue(v =>
        {
            variables = v;
        });

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(RecursiveList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        List<VariableTerm> terms =[];

        list.Head.Accept(this).IfHasValue(terms.AddRange);
        list.Tail.Accept(this).IfHasValue(terms.AddRange);

        return new Some<List<VariableTerm>>(terms);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(ConventionalList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        List<VariableTerm> variables =[];
        foreach (var term in list.Terms)
        {
            term.Accept(this).IfHasValue(variables.AddRange);
        }

        return new Some<List<VariableTerm>>(variables);
    }

    /// <inheritdoc/>
    public override IOption<List<VariableTerm>> Visit(Forall _)
    {
        return new Some<List<VariableTerm>>([]);
    }
}