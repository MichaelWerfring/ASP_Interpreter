//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/micha/Desktop/4_Semester/Logikprogrammierung/ASP_Interpreter/asp_interpreter_lib/asp_interpreter_lib/ANTLR/ASP.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace asp_interpreter_lib.ANTLR {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="ASPParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IASPListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] ASPParser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] ASPParser.ProgramContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.query"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterQuery([NotNull] ASPParser.QueryContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.query"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitQuery([NotNull] ASPParser.QueryContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.statements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatements([NotNull] ASPParser.StatementsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.statements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatements([NotNull] ASPParser.StatementsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] ASPParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] ASPParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.head"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterHead([NotNull] ASPParser.HeadContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.head"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitHead([NotNull] ASPParser.HeadContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.body"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBody([NotNull] ASPParser.BodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.body"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBody([NotNull] ASPParser.BodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.disjunction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDisjunction([NotNull] ASPParser.DisjunctionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.disjunction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDisjunction([NotNull] ASPParser.DisjunctionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.choice"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterChoice([NotNull] ASPParser.ChoiceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.choice"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitChoice([NotNull] ASPParser.ChoiceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.choice_elements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterChoice_elements([NotNull] ASPParser.Choice_elementsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.choice_elements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitChoice_elements([NotNull] ASPParser.Choice_elementsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.choice_element"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterChoice_element([NotNull] ASPParser.Choice_elementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.choice_element"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitChoice_element([NotNull] ASPParser.Choice_elementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.naf_literals"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNaf_literals([NotNull] ASPParser.Naf_literalsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.naf_literals"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNaf_literals([NotNull] ASPParser.Naf_literalsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.naf_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNaf_literal([NotNull] ASPParser.Naf_literalContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.naf_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNaf_literal([NotNull] ASPParser.Naf_literalContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.classical_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassical_literal([NotNull] ASPParser.Classical_literalContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.classical_literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassical_literal([NotNull] ASPParser.Classical_literalContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.builtin_atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBuiltin_atom([NotNull] ASPParser.Builtin_atomContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.builtin_atom"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBuiltin_atom([NotNull] ASPParser.Builtin_atomContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>equalityOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEqualityOperation([NotNull] ASPParser.EqualityOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>equalityOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEqualityOperation([NotNull] ASPParser.EqualityOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>unequalityOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnequalityOperation([NotNull] ASPParser.UnequalityOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>unequalityOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnequalityOperation([NotNull] ASPParser.UnequalityOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>lessOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLessOperation([NotNull] ASPParser.LessOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>lessOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLessOperation([NotNull] ASPParser.LessOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>greaterOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterGreaterOperation([NotNull] ASPParser.GreaterOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>greaterOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitGreaterOperation([NotNull] ASPParser.GreaterOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>lessOrEqOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLessOrEqOperation([NotNull] ASPParser.LessOrEqOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>lessOrEqOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLessOrEqOperation([NotNull] ASPParser.LessOrEqOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>greaterOrEqOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterGreaterOrEqOperation([NotNull] ASPParser.GreaterOrEqOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>greaterOrEqOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitGreaterOrEqOperation([NotNull] ASPParser.GreaterOrEqOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>disunificationOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDisunificationOperation([NotNull] ASPParser.DisunificationOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>disunificationOperation</c>
	/// labeled alternative in <see cref="ASPParser.binop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDisunificationOperation([NotNull] ASPParser.DisunificationOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.terms"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerms([NotNull] ASPParser.TermsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.terms"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerms([NotNull] ASPParser.TermsContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>negatedTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNegatedTerm([NotNull] ASPParser.NegatedTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>negatedTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNegatedTerm([NotNull] ASPParser.NegatedTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>stringTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStringTerm([NotNull] ASPParser.StringTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>stringTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStringTerm([NotNull] ASPParser.StringTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>basicTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBasicTerm([NotNull] ASPParser.BasicTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>basicTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBasicTerm([NotNull] ASPParser.BasicTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>arithmeticOperationTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArithmeticOperationTerm([NotNull] ASPParser.ArithmeticOperationTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>arithmeticOperationTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArithmeticOperationTerm([NotNull] ASPParser.ArithmeticOperationTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesizedTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesizedTerm([NotNull] ASPParser.ParenthesizedTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesizedTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesizedTerm([NotNull] ASPParser.ParenthesizedTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>anonymousVariableTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAnonymousVariableTerm([NotNull] ASPParser.AnonymousVariableTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>anonymousVariableTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAnonymousVariableTerm([NotNull] ASPParser.AnonymousVariableTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>numberTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumberTerm([NotNull] ASPParser.NumberTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>numberTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumberTerm([NotNull] ASPParser.NumberTermContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>variableTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableTerm([NotNull] ASPParser.VariableTermContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>variableTerm</c>
	/// labeled alternative in <see cref="ASPParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableTerm([NotNull] ASPParser.VariableTermContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ASPParser.arithop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArithop([NotNull] ASPParser.ArithopContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ASPParser.arithop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArithop([NotNull] ASPParser.ArithopContext context);
}
} // namespace asp_interpreter_lib.ANTLR
