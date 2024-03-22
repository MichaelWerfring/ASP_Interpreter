// Generated from c://Users//micha//Desktop//4_Semester//Logikprogrammierung//ASP_Interpreter//asp_interpreter_lib//asp_interpreter_lib//ANTLR//ASP.g4 by ANTLR 4.13.1
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
	 * Enter a parse tree produced by {@link ASPParser#head}.
	 * @param ctx the parse tree
	 */
	void enterHead(ASPParser.HeadContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#head}.
	 * @param ctx the parse tree
	 */
	void exitHead(ASPParser.HeadContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#body}.
	 * @param ctx the parse tree
	 */
	void enterBody(ASPParser.BodyContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#body}.
	 * @param ctx the parse tree
	 */
	void exitBody(ASPParser.BodyContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#disjunction}.
	 * @param ctx the parse tree
	 */
	void enterDisjunction(ASPParser.DisjunctionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#disjunction}.
	 * @param ctx the parse tree
	 */
	void exitDisjunction(ASPParser.DisjunctionContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#choice}.
	 * @param ctx the parse tree
	 */
	void enterChoice(ASPParser.ChoiceContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#choice}.
	 * @param ctx the parse tree
	 */
	void exitChoice(ASPParser.ChoiceContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#choice_elements}.
	 * @param ctx the parse tree
	 */
	void enterChoice_elements(ASPParser.Choice_elementsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#choice_elements}.
	 * @param ctx the parse tree
	 */
	void exitChoice_elements(ASPParser.Choice_elementsContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#choice_element}.
	 * @param ctx the parse tree
	 */
	void enterChoice_element(ASPParser.Choice_elementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#choice_element}.
	 * @param ctx the parse tree
	 */
	void exitChoice_element(ASPParser.Choice_elementContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#aggregate}.
	 * @param ctx the parse tree
	 */
	void enterAggregate(ASPParser.AggregateContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#aggregate}.
	 * @param ctx the parse tree
	 */
	void exitAggregate(ASPParser.AggregateContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#aggregate_elements}.
	 * @param ctx the parse tree
	 */
	void enterAggregate_elements(ASPParser.Aggregate_elementsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#aggregate_elements}.
	 * @param ctx the parse tree
	 */
	void exitAggregate_elements(ASPParser.Aggregate_elementsContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#aggregate_element}.
	 * @param ctx the parse tree
	 */
	void enterAggregate_element(ASPParser.Aggregate_elementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#aggregate_element}.
	 * @param ctx the parse tree
	 */
	void exitAggregate_element(ASPParser.Aggregate_elementContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#aggregate_function}.
	 * @param ctx the parse tree
	 */
	void enterAggregate_function(ASPParser.Aggregate_functionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#aggregate_function}.
	 * @param ctx the parse tree
	 */
	void exitAggregate_function(ASPParser.Aggregate_functionContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#optimize}.
	 * @param ctx the parse tree
	 */
	void enterOptimize(ASPParser.OptimizeContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#optimize}.
	 * @param ctx the parse tree
	 */
	void exitOptimize(ASPParser.OptimizeContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#optimize_elements}.
	 * @param ctx the parse tree
	 */
	void enterOptimize_elements(ASPParser.Optimize_elementsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#optimize_elements}.
	 * @param ctx the parse tree
	 */
	void exitOptimize_elements(ASPParser.Optimize_elementsContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#optimize_element}.
	 * @param ctx the parse tree
	 */
	void enterOptimize_element(ASPParser.Optimize_elementContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#optimize_element}.
	 * @param ctx the parse tree
	 */
	void exitOptimize_element(ASPParser.Optimize_elementContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#optimize_function}.
	 * @param ctx the parse tree
	 */
	void enterOptimize_function(ASPParser.Optimize_functionContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#optimize_function}.
	 * @param ctx the parse tree
	 */
	void exitOptimize_function(ASPParser.Optimize_functionContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#weight_at_level}.
	 * @param ctx the parse tree
	 */
	void enterWeight_at_level(ASPParser.Weight_at_levelContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#weight_at_level}.
	 * @param ctx the parse tree
	 */
	void exitWeight_at_level(ASPParser.Weight_at_levelContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#naf_literals}.
	 * @param ctx the parse tree
	 */
	void enterNaf_literals(ASPParser.Naf_literalsContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#naf_literals}.
	 * @param ctx the parse tree
	 */
	void exitNaf_literals(ASPParser.Naf_literalsContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#naf_literal}.
	 * @param ctx the parse tree
	 */
	void enterNaf_literal(ASPParser.Naf_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#naf_literal}.
	 * @param ctx the parse tree
	 */
	void exitNaf_literal(ASPParser.Naf_literalContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#classical_literal}.
	 * @param ctx the parse tree
	 */
	void enterClassical_literal(ASPParser.Classical_literalContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#classical_literal}.
	 * @param ctx the parse tree
	 */
	void exitClassical_literal(ASPParser.Classical_literalContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#builtin_atom}.
	 * @param ctx the parse tree
	 */
	void enterBuiltin_atom(ASPParser.Builtin_atomContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#builtin_atom}.
	 * @param ctx the parse tree
	 */
	void exitBuiltin_atom(ASPParser.Builtin_atomContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#binop}.
	 * @param ctx the parse tree
	 */
	void enterBinop(ASPParser.BinopContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#binop}.
	 * @param ctx the parse tree
	 */
	void exitBinop(ASPParser.BinopContext ctx);
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
	 * Enter a parse tree produced by {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void enterTerm(ASPParser.TermContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#term}.
	 * @param ctx the parse tree
	 */
	void exitTerm(ASPParser.TermContext ctx);
	/**
	 * Enter a parse tree produced by {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void enterArithop(ASPParser.ArithopContext ctx);
	/**
	 * Exit a parse tree produced by {@link ASPParser#arithop}.
	 * @param ctx the parse tree
	 */
	void exitArithop(ASPParser.ArithopContext ctx);
}