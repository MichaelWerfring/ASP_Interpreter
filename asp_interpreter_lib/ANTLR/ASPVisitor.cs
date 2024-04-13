//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/micha/Desktop/4_Semester/Logikprogrammierung/ASP_Interpreter/asp_interpreter_lib/ANTLR/ASP.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="ASPParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IASPVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] ASPParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.query"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitQuery([NotNull] ASPParser.QueryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.statements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatements([NotNull] ASPParser.StatementsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] ASPParser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.goal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGoal([NotNull] ASPParser.GoalContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.binary_operation"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBinary_operation([NotNull] ASPParser.Binary_operationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] ASPParser.LiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>equalityOperation</c>
	/// labeled alternative in <see cref="ASPParser.binary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEqualityOperation([NotNull] ASPParser.EqualityOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>disunificationOperation</c>
	/// labeled alternative in <see cref="ASPParser.binary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDisunificationOperation([NotNull] ASPParser.DisunificationOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>lessOperation</c>
	/// labeled alternative in <see cref="ASPParser.binary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLessOperation([NotNull] ASPParser.LessOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>greaterOperation</c>
	/// labeled alternative in <see cref="ASPParser.binary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGreaterOperation([NotNull] ASPParser.GreaterOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>lessOrEqOperation</c>
	/// labeled alternative in <see cref="ASPParser.binary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLessOrEqOperation([NotNull] ASPParser.LessOrEqOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>greaterOrEqOperation</c>
	/// labeled alternative in <see cref="ASPParser.binary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGreaterOrEqOperation([NotNull] ASPParser.GreaterOrEqOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>isOperation</c>
	/// labeled alternative in <see cref="ASPParser.binary_operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIsOperation([NotNull] ASPParser.IsOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ASPParser.terms"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTerms([NotNull] ASPParser.TermsContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>negatedTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNegatedTerm([NotNull] ASPParser.NegatedTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>stringTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringTerm([NotNull] ASPParser.StringTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>basicTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBasicTerm([NotNull] ASPParser.BasicTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>arithmeticOperationTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArithmeticOperationTerm([NotNull] ASPParser.ArithmeticOperationTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parenthesizedTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesizedTerm([NotNull] ASPParser.ParenthesizedTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>anonymousVariableTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAnonymousVariableTerm([NotNull] ASPParser.AnonymousVariableTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>numberTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberTerm([NotNull] ASPParser.NumberTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>variableTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableTerm([NotNull] ASPParser.VariableTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>listTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListTerm([NotNull] ASPParser.ListTermContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>conventionalList</c>
	/// labeled alternative in <see cref="ASPParser.list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConventionalList([NotNull] ASPParser.ConventionalListContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>recursiveList</c>
	/// labeled alternative in <see cref="ASPParser.list"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRecursiveList([NotNull] ASPParser.RecursiveListContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>plusOperation</c>
	/// labeled alternative in <see cref="ASPParser.arithop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlusOperation([NotNull] ASPParser.PlusOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>minusOperation</c>
	/// labeled alternative in <see cref="ASPParser.arithop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMinusOperation([NotNull] ASPParser.MinusOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>timesOperation</c>
	/// labeled alternative in <see cref="ASPParser.arithop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTimesOperation([NotNull] ASPParser.TimesOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>divOperation</c>
	/// labeled alternative in <see cref="ASPParser.arithop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDivOperation([NotNull] ASPParser.DivOperationContext context);
}
