// Generated from d:/FH/Semester 4/Logikprogrammierung ILV/SASP-Projekt/ASP_Interpreter/asp_interpreter_lib/ANTLR/ASP.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link ASPParser}.
 */
public interface ASPListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link ASPParser#program}.
	 * @param ctx the parse tree
	 */
	void enterProgram(ASPParser.ProgramContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#program}.
	 * @param ctx the parse tree
	 */
	void exitProgram(ASPParser.ProgramContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#query}.
	 * @param ctx the parse tree
	 */
	void enterQuery(ASPParser.QueryContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#query}.
	 * @param ctx the parse tree
	 */
	void exitQuery(ASPParser.QueryContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#statements}.
	 * @param ctx the parse tree
	 */
	void enterStatements(ASPParser.StatementsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#statements}.
	 * @param ctx the parse tree
	 */
	void exitStatements(ASPParser.StatementsContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterStatement(ASPParser.StatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitStatement(ASPParser.StatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#goal}.
	 * @param ctx the parse tree
	 */
	void enterGoal(ASPParser.GoalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#goal}.
	 * @param ctx the parse tree
	 */
	void exitGoal(ASPParser.GoalContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#binary_operation}.
	 * @param ctx the parse tree
	 */
	void enterBinary_operation(ASPParser.Binary_operationContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#binary_operation}.
	 * @param ctx the parse tree
	 */
	void exitBinary_operation(ASPParser.Binary_operationContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#literal}.
	 * @param ctx the parse tree
	 */
	void enterLiteral(ASPParser.LiteralContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#literal}.
	 * @param ctx the parse tree
	 */
	void exitLiteral(ASPParser.LiteralContext ctx);
	/**
	 * Enter a parse tree produced by the {@code equalityOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void enterEqualityOperation(ASPParser.EqualityOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code equalityOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void exitEqualityOperation(ASPParser.EqualityOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code disunificationOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void enterDisunificationOperation(ASPParser.DisunificationOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code disunificationOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void exitDisunificationOperation(ASPParser.DisunificationOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code lessOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void enterLessOperation(ASPParser.LessOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code lessOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void exitLessOperation(ASPParser.LessOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code greaterOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void enterGreaterOperation(ASPParser.GreaterOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code greaterOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void exitGreaterOperation(ASPParser.GreaterOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code lessOrEqOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void enterLessOrEqOperation(ASPParser.LessOrEqOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code lessOrEqOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void exitLessOrEqOperation(ASPParser.LessOrEqOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code greaterOrEqOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void enterGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code greaterOrEqOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void exitGreaterOrEqOperation(ASPParser.GreaterOrEqOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code isOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void enterIsOperation(ASPParser.IsOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code isOperation}
	 * labeled alternative in {@link ASPParser#binary_operator}.
	 * @param ctx the parse tree
	 */
	void exitIsOperation(ASPParser.IsOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#terms}.
	 * @param ctx the parse tree
	 */
	void enterTerms(ASPParser.TermsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#terms}.
	 * @param ctx the parse tree
	 */
	void exitTerms(ASPParser.TermsContext ctx);
	/**
	 * Enter a parse tree produced by the {@code negatedTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterNegatedTerm(ASPParser.NegatedTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code negatedTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitNegatedTerm(ASPParser.NegatedTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code stringTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterStringTerm(ASPParser.StringTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code stringTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitStringTerm(ASPParser.StringTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code basicTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterBasicTerm(ASPParser.BasicTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code basicTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitBasicTerm(ASPParser.BasicTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code arithmeticOperationTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code arithmeticOperationTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitArithmeticOperationTerm(ASPParser.ArithmeticOperationTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code parenthesizedTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterParenthesizedTerm(ASPParser.ParenthesizedTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code parenthesizedTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitParenthesizedTerm(ASPParser.ParenthesizedTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code anonymousVariableTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code anonymousVariableTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitAnonymousVariableTerm(ASPParser.AnonymousVariableTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code numberTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterNumberTerm(ASPParser.NumberTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code numberTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitNumberTerm(ASPParser.NumberTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code variableTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterVariableTerm(ASPParser.VariableTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code variableTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitVariableTerm(ASPParser.VariableTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code listTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterListTerm(ASPParser.ListTermContext ctx);
	/**
	 * Exit a parse tree produced by the {@code listTerm}
	 * labeled alternative in {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitListTerm(ASPParser.ListTermContext ctx);
	/**
	 * Enter a parse tree produced by the {@code conventionalList}
	 * labeled alternative in {@link ASPParser#list}.
	 * @param ctx the parse tree
	 */
	void enterConventionalList(ASPParser.ConventionalListContext ctx);
	/**
	 * Exit a parse tree produced by the {@code conventionalList}
	 * labeled alternative in {@link ASPParser#list}.
	 * @param ctx the parse tree
	 */
	void exitConventionalList(ASPParser.ConventionalListContext ctx);
	/**
	 * Enter a parse tree produced by the {@code recursiveList}
	 * labeled alternative in {@link ASPParser#list}.
	 * @param ctx the parse tree
	 */
	void enterRecursiveList(ASPParser.RecursiveListContext ctx);
	/**
	 * Exit a parse tree produced by the {@code recursiveList}
	 * labeled alternative in {@link ASPParser#list}.
	 * @param ctx the parse tree
	 */
	void exitRecursiveList(ASPParser.RecursiveListContext ctx);
	/**
	 * Enter a parse tree produced by the {@code plusOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void enterPlusOperation(ASPParser.PlusOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code plusOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void exitPlusOperation(ASPParser.PlusOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code minusOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void enterMinusOperation(ASPParser.MinusOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code minusOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void exitMinusOperation(ASPParser.MinusOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code timesOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void enterTimesOperation(ASPParser.TimesOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code timesOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void exitTimesOperation(ASPParser.TimesOperationContext ctx);
	/**
	 * Enter a parse tree produced by the {@code divOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void enterDivOperation(ASPParser.DivOperationContext ctx);
	/**
	 * Exit a parse tree produced by the {@code divOperation}
	 * labeled alternative in {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void exitDivOperation(ASPParser.DivOperationContext ctx);
}