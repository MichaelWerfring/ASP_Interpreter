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

/// <summary>
/// Iterates through a program and returns a list of all variables in the program.
/// </summary>
public class VariableFinder : TypeBaseVisitor<List<VariableTerm>>
{
    /// <summary>
    /// Retrieves all variables from a given program.
    /// </summary>
    /// <param name="program">The program to retrieve variables from.</param>
    /// <returns>None if the process failed else a list of variables found.</returns>
    /// <exception cref="ArgumentNullException">If the program is null.</exception>
    public override IOption<List<VariableTerm>> Visit(AspProgram program)
    {
        ArgumentNullException.ThrowIfNull(program);
        List<VariableTerm> variableTerms = new List<VariableTerm>();
        foreach (var statement in program.Statements)
        {
            statement.Accept(this).IfHasValue(variableTerms.AddRange);
        }

        program.Query.GetValueOrThrow("Inable to parse query!").Accept(this).IfHasValue(variableTerms.AddRange);

        return new Some<List<VariableTerm>>(variableTerms);
    }

    /// <summary>
    /// Retrieves all variables from a given statement.
    /// </summary>
    /// <param name="statement">The statement to retrieve the variables from.</param>
    /// <returns>None if the process failed else a list of variables found.</returns>
    /// <exception cref="ArgumentNullException">If the statement is null.</exception>
    public override IOption<List<VariableTerm>> Visit(Statement statement)
    {
        ArgumentNullException.ThrowIfNull(statement);
        List<VariableTerm> variables = new List<VariableTerm>();

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

    /// <summary>
    ///  Retrieves all variables from a given literal.
    /// </summary>
    /// <param name="literal">The literal to retrieve the variables from.</param>
    /// <returns>None if the process failed else a list of variables found.</returns>
    /// <exception cref="ArgumentNullException">If the literal is null.</exception>
    public override IOption<List<VariableTerm>> Visit(Literal literal)
    {
        ArgumentNullException.ThrowIfNull(literal);
        List<VariableTerm> variables = new List<VariableTerm>();
        foreach (var term in literal.Terms)
        {
            term.Accept(this).IfHasValue(variables.AddRange);
        }

        return new Some<List<VariableTerm>>(variables);
    }

    /// <summary>
    /// Visits the given operation.
    /// </summary>
    /// <param name="plus">The operation to visit.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Plus plus)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Visits the given operation.
    /// </summary>
    /// <param name="minus">The operation to visit.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Minus minus)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Visits the given operation.
    /// </summary>
    /// <param name="multiply">The operation to visit.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Multiply multiply)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Visits the given operation.
    /// </summary>
    /// <param name="power">The operation to visit.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Power power)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Visits the given operation.
    /// </summary>
    /// <param name="divide">The operation to visit.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Divide divide)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Retrieves all variables from a given binary operation.
    /// </summary>
    /// <param name="binOp">The operation to retrieve the variables from.</param>
    /// <returns>None if the process failed else a list of variables found.</returns>
    /// <exception cref="ArgumentNullException">If the operation is null.</exception>
    public override IOption<List<VariableTerm>> Visit(BinaryOperation binOp)
    {
        ArgumentNullException.ThrowIfNull(binOp);
        List<VariableTerm> variables = new List<VariableTerm>();

        binOp.Left.Accept(this).IfHasValue(variables.AddRange);

        binOp.Right.Accept(this).IfHasValue(variables.AddRange);

        return new Some<List<VariableTerm>>(variables);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="disunification">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Disunification disunification)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="equality">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Equality equality)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="greaterOrEqualThan">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(GreaterOrEqualThan greaterOrEqualThan)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="greaterThan">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(GreaterThan greaterThan)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="lessOrEqualThan">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(LessOrEqualThan lessOrEqualThan)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="lessThan">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(LessThan lessThan)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="isOperator">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Is isOperator)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="term">The given operation.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(AnonymousVariableTerm term)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns a list containing only the given variable.
    /// </summary>
    /// <param name="variable">The variable found.</param>
    /// <returns>A list only containing the given variable.</returns>
    public override IOption<List<VariableTerm>> Visit(VariableTerm variable)
    {
        return new Some<List<VariableTerm>>([variable]);
    }

    /// <summary>
    /// Retrieves all variables from a given arithmetic operation.
    /// </summary>
    /// <param name="arithmeticOperation">The operation to retrieve the variables from.</param>
    /// <returns>None if the process failed else a list of variables found.</returns>
    /// <exception cref="ArgumentNullException">If the operation is null.</exception>
    public override IOption<List<VariableTerm>> Visit(ArithmeticOperationTerm arithmeticOperation)
    {
        ArgumentNullException.ThrowIfNull(arithmeticOperation);

        List<VariableTerm> variables = new List<VariableTerm>();
        variables.AddRange(arithmeticOperation.Left.Accept(this).
            GetValueOrThrow("Cannot get variables from arithmetic operation!"));
        variables.AddRange(arithmeticOperation.Right.Accept(this).
            GetValueOrThrow("Cannot get variables from arithmetic operation!"));

        return new Some<List<VariableTerm>>(variables);
    }

    /// <summary>
    /// Retrieves all variables from a given basic term.
    /// </summary>
    /// <param name="basicTerm">The term to retrieve the variables form.</param>
    /// <returns>A list containing all the variables from the given term.</returns>
    /// <exception cref="ArgumentNullException">If the term is null.</exception>
    public override IOption<List<VariableTerm>> Visit(BasicTerm basicTerm)
    {
        ArgumentNullException.ThrowIfNull(basicTerm);
        List<VariableTerm> variables = new List<VariableTerm>();

        foreach (var term in basicTerm.Terms)
        {
            term.Accept(this).IfHasValue(variables.AddRange);
        }

        return new Some<List<VariableTerm>>(variables);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="term">The given term.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(StringTerm term)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="term">The given term.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(NumberTerm term)
    {
        return new Some<List<VariableTerm>>([]);
    }

    /// <summary>
    /// Retrieves all variables from a given negated term.
    /// </summary>
    /// <param name="term">The term to retrieve the variables from.</param>
    /// <returns>All the variables contained in the term.</returns>
    /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
    public override IOption<List<VariableTerm>> Visit(NegatedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        List<VariableTerm> variables = new List<VariableTerm>();
        term.Term.Accept(this).IfHasValue(v =>
        {
            variables = v;
        });

        return new Some<List<VariableTerm>>(variables);
    }

    /// <summary>
    /// Retrieves all variables from a given parenthesized term.
    /// </summary>
    /// <param name="term">The term to retrieve the variables from.</param>
    /// <returns>All the variables contained in the term.</returns>
    /// <exception cref="ArgumentNullException">Is thrown if the term is null.</exception>
    public override IOption<List<VariableTerm>> Visit(ParenthesizedTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);
        List<VariableTerm> variables = new List<VariableTerm>();
        term.Term.Accept(this).IfHasValue(v =>
        {
            variables = v;
        });

        return new Some<List<VariableTerm>>(variables);
    }

    /// <summary>
    /// Retrieves all variables from a given list term.
    /// </summary>
    /// <param name="list">The list to retrieve the variables from.</param>
    /// <returns>A list of variables found.</returns>
    /// <exception cref="ArgumentNullException">If the list is null.</exception>
    public override IOption<List<VariableTerm>> Visit(RecursiveList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        List<VariableTerm> terms = new List<VariableTerm>();

        list.Head.Accept(this).IfHasValue(terms.AddRange);
        list.Tail.Accept(this).IfHasValue(terms.AddRange);

        return new Some<List<VariableTerm>>(terms);
    }

    /// <summary>
    /// Retrieves all variables from a given list term.
    /// </summary>
    /// <param name="list">The list to retrieve the variables from.</param>
    /// <returns>A list of variables found.</returns>
    /// <exception cref="ArgumentNullException">If the list is null.</exception>
    public override IOption<List<VariableTerm>> Visit(ConventionalList list)
    {
        ArgumentNullException.ThrowIfNull(list);
        List<VariableTerm> variables = new List<VariableTerm>();
        foreach (var term in list.Terms)
        {
            term.Accept(this).IfHasValue(variables.AddRange);
        }

        return new Some<List<VariableTerm>>(variables);
    }

    /// <summary>
    /// Returns an empty list because the given type cannot contain any variables.
    /// </summary>
    /// <param name="goal">The given term.</param>
    /// <returns>An empty list.</returns>
    public override IOption<List<VariableTerm>> Visit(Forall goal)
    {
        return new Some<List<VariableTerm>>([]);
    }
}