// Generated from C:/Users/micha/Desktop/4_Semester/Logikprogrammierung/ASP_Interpreter/asp_interpreter_lib/ANTLR/ASP.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeVisitor;

/**
 * This interface defines a complete generic visitor for a parse tree produced
 * by {@link ASPParser}.
 *
 * @param <T> The return type of the visit operation. Use {@link Void} for
 * operations with no return type.
 */
public interface ASPVisitor<T> extends ParseTreeVisitor<T> {
	/**
	 * Visit a parse tree produced by {@link ASPParser#program}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitProgram(ASPParser.ProgramContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#query}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitQuery(ASPParser.QueryContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#statements}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitStatements(ASPParser.StatementsContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#statement}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitStatement(ASPParser.StatementContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#head}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitHead(ASPParser.HeadContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#body}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBody(ASPParser.BodyContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#naf_literals}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNaf_literals(ASPParser.Naf_literalsContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#naf_literal}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNaf_literal(ASPParser.Naf_literalContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#classical_literal}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitClassical_literal(ASPParser.Classical_literalContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#binary_operation}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBinary_operation(ASPParser.Binary_operationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code equalityOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitEqualityOperation(ASPParser.EqualityOperationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code disunificationOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitDisunificationOperation(ASPParser.DisunificationOperationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code lessOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLessOperation(ASPParser.LessOperationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code greaterOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGreaterOperation(ASPParser.GreaterOperationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code lessOrEqOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitLessOrEqOperation(ASPParser.LessOrEqOperationContext ctx);
	/**
	 * Visit a parse tree produced by the {@code greaterOrEqOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#terms}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitTerms(ASPParser.TermsContext ctx);
	/**
	 * Visit a parse tree produced by the {@code negatedTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNegatedTerm(ASPParser.NegatedTermContext ctx);
	/**
	 * Visit a parse tree produced by the {@code stringTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitStringTerm(ASPParser.StringTermContext ctx);
	/**
	 * Visit a parse tree produced by the {@code basicTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitBasicTerm(ASPParser.BasicTermContext ctx);
	/**
	 * Visit a parse tree produced by the {@code arithmeticOperationTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext ctx);
	/**
	 * Visit a parse tree produced by the {@code parenthesizedTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitParenthesizedTerm(ASPParser.ParenthesizedTermContext ctx);
	/**
	 * Visit a parse tree produced by the {@code anonymousVariableTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext ctx);
	/**
	 * Visit a parse tree produced by the {@code numberTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitNumberTerm(ASPParser.NumberTermContext ctx);
	/**
	 * Visit a parse tree produced by the {@code variableTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitVariableTerm(ASPParser.VariableTermContext ctx);
	/**
	 * Visit a parse tree produced by {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 * @return the visitor result
	 */
	T visitArithop(ASPParser.ArithopContext ctx);
}