//-----------------------------------------------------------------------
// <copyright file="TypeBaseVisitor.cs" company="FHWN">
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
/// Allows to polymorphic visit all types of the asp type hierarchy.
/// </summary>
/// <typeparam name="T">The type to be returned by the given inheriting class.</typeparam>
public class TypeBaseVisitor<T>
{
    /// <summary>
    /// Visits the asp program type.
    /// </summary>
    /// <param name="program">The program to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(AspProgram program) => new None<T>();

    /// <summary>
    /// Visits a query.
    /// </summary>
    /// <param name="query">The query to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Query query) => new None<T>();

    /// <summary>
    /// Visits a statement.
    /// </summary>
    /// <param name="statement">The statement to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Statement statement) => new None<T>();

    /// <summary>
    /// Visits a forall goal.
    /// </summary>
    /// <param name="goal">The forall goal to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Forall goal) => new None<T>();

    /// <summary>
    /// Visits a literal.
    /// </summary>
    /// <param name="literal">The literal to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Literal literal) => new None<T>();

    /// <summary>
    /// Visits the binary operation type.
    /// </summary>
    /// <param name="binaryOperation">The type to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(BinaryOperation binaryOperation) => new None<T>();

    /// <summary>
    /// Visits the plus operator.
    /// </summary>
    /// <param name="plus">The operator to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Plus plus) => new None<T>();

    /// <summary>
    /// Visits the minus operator.
    /// </summary>
    /// <param name="minus">The minus to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Minus minus) => new None<T>();

    /// <summary>
    /// Visits the multiply operator.
    /// </summary>
    /// <param name="multiply">The operator to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Multiply multiply) => new None<T>();

    /// <summary>
    /// Visits the divide operator.
    /// </summary>
    /// <param name="divide">The operator to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Divide divide) => new None<T>();

    /// <summary>
    /// Visits the power operator.
    /// </summary>
    /// <param name="power">The operator to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Power power) => new None<T>();

    /// <summary>
    /// Visits a disunification operation.
    /// </summary>
    /// <param name="disunification">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Disunification disunification) => new None<T>();

    /// <summary>
    /// Visits the equality operation.
    /// </summary>
    /// <param name="equality">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Equality equality) => new None<T>();

    /// <summary>
    /// Visits a greater or equal than operation.
    /// </summary>
    /// <param name="greaterOrEqualThan">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(GreaterOrEqualThan greaterOrEqualThan) => new None<T>();

    /// <summary>
    /// Visits the greater than operation.
    /// </summary>
    /// <param name="greaterThan">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(GreaterThan greaterThan) => new None<T>();

    /// <summary>
    /// Visits a less or equal than operation.
    /// </summary>
    /// <param name="lessOrEqualThan">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(LessOrEqualThan lessOrEqualThan) => new None<T>();

    /// <summary>
    /// Visits a less than operation.
    /// </summary>
    /// <param name="lessThan">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(LessThan lessThan) => new None<T>();

    /// <summary>
    /// Visits the is operation.
    /// </summary>
    /// <param name="isOp">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(Is isOp) => new None<T>();

    /// <summary>
    /// Visits the anonymous variable term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(AnonymousVariableTerm term) => new None<T>();

    /// <summary>
    /// Visits the variable term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(VariableTerm term) => new None<T>();

    /// <summary>
    /// Visits the arithmetic operation term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(ArithmeticOperationTerm term) => new None<T>();

    /// <summary>
    /// Visits the basic term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(BasicTerm term) => new None<T>();

    /// <summary>
    /// Visits the string term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(StringTerm term) => new None<T>();

    /// <summary>
    /// Visits the number term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(NumberTerm term) => new None<T>();

    /// <summary>
    /// Visits the negated term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(NegatedTerm term) => new None<T>();

    /// <summary>
    /// Visits the parenthesized term.
    /// </summary>
    /// <param name="term">The term to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(ParenthesizedTerm term) => new None<T>();

    /// <summary>
    /// Visits the recursive list.
    /// </summary>
    /// <param name="list">The list to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(RecursiveList list) => new None<T>();

    /// <summary>
    /// Visits the conventional list.
    /// </summary>
    /// <param name="list">The list to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(ConventionalList list) => new None<T>();

    /// <summary>
    /// Visits the is not operation.
    /// </summary>
    /// <param name="isNotOp">The operation to be visited.</param>
    /// <returns>None if an error occurs or the given visitor does not override the method.
    /// Else it contains the value wrapped into the <see cref="Some{T}"/> class.</returns>
    public virtual IOption<T> Visit(IsNot isNotOp) => new None<T>();
}