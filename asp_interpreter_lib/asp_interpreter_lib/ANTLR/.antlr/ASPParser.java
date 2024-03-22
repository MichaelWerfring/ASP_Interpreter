// Generated from c://Users//micha//Desktop//4_Semester//Logikprogrammierung//ASP_Interpreter//asp_interpreter_lib//asp_interpreter_lib//ANTLR//ASP.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue"})
public class ASPParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		ID=1, VARIABLE=2, STRING=3, NUMBER=4, ANONYMOUS_VARIABLE=5, DOT=6, COMMA=7, 
		QUERY_MARK=8, COLON=9, SEMICOLON=10, OR=11, NAF=12, CONS=13, WCONS=14, 
		PLUS=15, MINUS=16, TIMES=17, DIV=18, AT=19, PAREN_OPEN=20, PAREN_CLOSE=21, 
		SQUARE_OPEN=22, SQUARE_CLOSE=23, CURLY_OPEN=24, CURLY_CLOSE=25, EQUAL=26, 
		UNEQUAL=27, LESS=28, GREATER=29, LESS_OR_EQ=30, GREATER_OR_EQ=31, AGGREGATE_COUNT=32, 
		AGGREGATE_MAX=33, AGGREGATE_MIN=34, AGGREGATE_SUM=35, MAXIMIZE=36, MINIMIZE=37, 
		COMMENT=38, BLANK=39, NEWLINE=40, TAB=41, WS=42;
	public static final int
		RULE_program = 0, RULE_query = 1, RULE_statements = 2, RULE_statement = 3, 
		RULE_head = 4, RULE_body = 5, RULE_disjunction = 6, RULE_choice = 7, RULE_choice_elements = 8, 
		RULE_choice_element = 9, RULE_aggregate = 10, RULE_aggregate_elements = 11, 
		RULE_aggregate_element = 12, RULE_aggregate_function = 13, RULE_optimize = 14, 
		RULE_optimize_elements = 15, RULE_optimize_element = 16, RULE_optimize_function = 17, 
		RULE_weight_at_level = 18, RULE_naf_literals = 19, RULE_naf_literal = 20, 
		RULE_classical_literal = 21, RULE_builtin_atom = 22, RULE_binop = 23, 
		RULE_terms = 24, RULE_term = 25, RULE_arithop = 26;
	private static String[] makeRuleNames() {
		return new String[] {
			"program", "query", "statements", "statement", "head", "body", "disjunction", 
			"choice", "choice_elements", "choice_element", "aggregate", "aggregate_elements", 
			"aggregate_element", "aggregate_function", "optimize", "optimize_elements", 
			"optimize_element", "optimize_function", "weight_at_level", "naf_literals", 
			"naf_literal", "classical_literal", "builtin_atom", "binop", "terms", 
			"term", "arithop"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, null, null, "'_'", "'.'", "','", "'?'", "':'", "';'", 
			"'|'", "'not'", "':-'", "':~'", "'+'", "'-'", "'*'", "'/'", "'@'", "'('", 
			"')'", "'['", "']'", "'{'", "'}'", "'='", null, "'<'", "'>'", "'<='", 
			"'>='", "'#count'", "'#max'", "'#min'", "'#sum'", null, null, null, null, 
			null, "'\\t'", "' '"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "ID", "VARIABLE", "STRING", "NUMBER", "ANONYMOUS_VARIABLE", "DOT", 
			"COMMA", "QUERY_MARK", "COLON", "SEMICOLON", "OR", "NAF", "CONS", "WCONS", 
			"PLUS", "MINUS", "TIMES", "DIV", "AT", "PAREN_OPEN", "PAREN_CLOSE", "SQUARE_OPEN", 
			"SQUARE_CLOSE", "CURLY_OPEN", "CURLY_CLOSE", "EQUAL", "UNEQUAL", "LESS", 
			"GREATER", "LESS_OR_EQ", "GREATER_OR_EQ", "AGGREGATE_COUNT", "AGGREGATE_MAX", 
			"AGGREGATE_MIN", "AGGREGATE_SUM", "MAXIMIZE", "MINIMIZE", "COMMENT", 
			"BLANK", "NEWLINE", "TAB", "WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "ASP.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public ASPParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ProgramContext extends ParserRuleContext {
		public StatementsContext statements() {
			return getRuleContext(StatementsContext.class,0);
		}
		public QueryContext query() {
			return getRuleContext(QueryContext.class,0);
		}
		public ProgramContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_program; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterProgram(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitProgram(this);
		}
	}

	public final ProgramContext program() throws RecognitionException {
		ProgramContext _localctx = new ProgramContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_program);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(54);
			statements();
			setState(55);
			query();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class QueryContext extends ParserRuleContext {
		public Classical_literalContext classical_literal() {
			return getRuleContext(Classical_literalContext.class,0);
		}
		public TerminalNode QUERY_MARK() { return getToken(ASPParser.QUERY_MARK, 0); }
		public QueryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_query; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterQuery(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitQuery(this);
		}
	}

	public final QueryContext query() throws RecognitionException {
		QueryContext _localctx = new QueryContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_query);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(57);
			classical_literal();
			setState(58);
			match(QUERY_MARK);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StatementsContext extends ParserRuleContext {
		public List<StatementContext> statement() {
			return getRuleContexts(StatementContext.class);
		}
		public StatementContext statement(int i) {
			return getRuleContext(StatementContext.class,i);
		}
		public StatementsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statements; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterStatements(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitStatements(this);
		}
	}

	public final StatementsContext statements() throws RecognitionException {
		StatementsContext _localctx = new StatementsContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_statements);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(63);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,0,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(60);
					statement();
					}
					} 
				}
				setState(65);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,0,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class StatementContext extends ParserRuleContext {
		public TerminalNode CONS() { return getToken(ASPParser.CONS, 0); }
		public TerminalNode DOT() { return getToken(ASPParser.DOT, 0); }
		public BodyContext body() {
			return getRuleContext(BodyContext.class,0);
		}
		public HeadContext head() {
			return getRuleContext(HeadContext.class,0);
		}
		public TerminalNode WCONS() { return getToken(ASPParser.WCONS, 0); }
		public TerminalNode SQUARE_OPEN() { return getToken(ASPParser.SQUARE_OPEN, 0); }
		public Weight_at_levelContext weight_at_level() {
			return getRuleContext(Weight_at_levelContext.class,0);
		}
		public TerminalNode SQUARE_CLOSE() { return getToken(ASPParser.SQUARE_CLOSE, 0); }
		public OptimizeContext optimize() {
			return getRuleContext(OptimizeContext.class,0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterStatement(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitStatement(this);
		}
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_statement);
		int _la;
		try {
			setState(92);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CONS:
				enterOuterAlt(_localctx, 1);
				{
				setState(66);
				match(CONS);
				setState(68);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 64425627710L) != 0)) {
					{
					setState(67);
					body();
					}
				}

				setState(70);
				match(DOT);
				}
				break;
			case ID:
			case VARIABLE:
			case STRING:
			case NUMBER:
			case ANONYMOUS_VARIABLE:
			case MINUS:
			case PAREN_OPEN:
			case CURLY_OPEN:
				enterOuterAlt(_localctx, 2);
				{
				setState(71);
				head();
				setState(76);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==CONS) {
					{
					setState(72);
					match(CONS);
					setState(74);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 64425627710L) != 0)) {
						{
						setState(73);
						body();
						}
					}

					}
				}

				setState(78);
				match(DOT);
				}
				break;
			case WCONS:
				enterOuterAlt(_localctx, 3);
				{
				setState(80);
				match(WCONS);
				setState(82);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 64425627710L) != 0)) {
					{
					setState(81);
					body();
					}
				}

				setState(84);
				match(DOT);
				setState(85);
				match(SQUARE_OPEN);
				setState(86);
				weight_at_level();
				setState(87);
				match(SQUARE_CLOSE);
				}
				break;
			case MAXIMIZE:
			case MINIMIZE:
				enterOuterAlt(_localctx, 4);
				{
				setState(89);
				optimize();
				setState(90);
				match(DOT);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class HeadContext extends ParserRuleContext {
		public DisjunctionContext disjunction() {
			return getRuleContext(DisjunctionContext.class,0);
		}
		public ChoiceContext choice() {
			return getRuleContext(ChoiceContext.class,0);
		}
		public HeadContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_head; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterHead(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitHead(this);
		}
	}

	public final HeadContext head() throws RecognitionException {
		HeadContext _localctx = new HeadContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_head);
		try {
			setState(96);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(94);
				disjunction();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(95);
				choice();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BodyContext extends ParserRuleContext {
		public Naf_literalContext naf_literal() {
			return getRuleContext(Naf_literalContext.class,0);
		}
		public AggregateContext aggregate() {
			return getRuleContext(AggregateContext.class,0);
		}
		public TerminalNode COMMA() { return getToken(ASPParser.COMMA, 0); }
		public BodyContext body() {
			return getRuleContext(BodyContext.class,0);
		}
		public TerminalNode NAF() { return getToken(ASPParser.NAF, 0); }
		public BodyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_body; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterBody(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitBody(this);
		}
	}

	public final BodyContext body() throws RecognitionException {
		BodyContext _localctx = new BodyContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(103);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,8,_ctx) ) {
			case 1:
				{
				setState(98);
				naf_literal();
				}
				break;
			case 2:
				{
				setState(100);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAF) {
					{
					setState(99);
					match(NAF);
					}
				}

				setState(102);
				aggregate();
				}
				break;
			}
			setState(107);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(105);
				match(COMMA);
				setState(106);
				body();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class DisjunctionContext extends ParserRuleContext {
		public List<Classical_literalContext> classical_literal() {
			return getRuleContexts(Classical_literalContext.class);
		}
		public Classical_literalContext classical_literal(int i) {
			return getRuleContext(Classical_literalContext.class,i);
		}
		public List<TerminalNode> OR() { return getTokens(ASPParser.OR); }
		public TerminalNode OR(int i) {
			return getToken(ASPParser.OR, i);
		}
		public DisjunctionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_disjunction; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterDisjunction(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitDisjunction(this);
		}
	}

	public final DisjunctionContext disjunction() throws RecognitionException {
		DisjunctionContext _localctx = new DisjunctionContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_disjunction);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(109);
			classical_literal();
			setState(114);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==OR) {
				{
				{
				setState(110);
				match(OR);
				setState(111);
				classical_literal();
				}
				}
				setState(116);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ChoiceContext extends ParserRuleContext {
		public TerminalNode CURLY_OPEN() { return getToken(ASPParser.CURLY_OPEN, 0); }
		public TerminalNode CURLY_CLOSE() { return getToken(ASPParser.CURLY_CLOSE, 0); }
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public List<BinopContext> binop() {
			return getRuleContexts(BinopContext.class);
		}
		public BinopContext binop(int i) {
			return getRuleContext(BinopContext.class,i);
		}
		public Choice_elementsContext choice_elements() {
			return getRuleContext(Choice_elementsContext.class,0);
		}
		public ChoiceContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_choice; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterChoice(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitChoice(this);
		}
	}

	public final ChoiceContext choice() throws RecognitionException {
		ChoiceContext _localctx = new ChoiceContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_choice);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(120);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1114174L) != 0)) {
				{
				setState(117);
				term(0);
				setState(118);
				binop();
				}
			}

			setState(122);
			match(CURLY_OPEN);
			setState(124);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ID || _la==MINUS) {
				{
				setState(123);
				choice_elements();
				}
			}

			setState(126);
			match(CURLY_CLOSE);
			setState(130);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4227858432L) != 0)) {
				{
				setState(127);
				binop();
				setState(128);
				term(0);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Choice_elementsContext extends ParserRuleContext {
		public Choice_elementContext choice_element() {
			return getRuleContext(Choice_elementContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(ASPParser.SEMICOLON, 0); }
		public Choice_elementsContext choice_elements() {
			return getRuleContext(Choice_elementsContext.class,0);
		}
		public Choice_elementsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_choice_elements; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterChoice_elements(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitChoice_elements(this);
		}
	}

	public final Choice_elementsContext choice_elements() throws RecognitionException {
		Choice_elementsContext _localctx = new Choice_elementsContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_choice_elements);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(132);
			choice_element();
			setState(135);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==SEMICOLON) {
				{
				setState(133);
				match(SEMICOLON);
				setState(134);
				choice_elements();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Choice_elementContext extends ParserRuleContext {
		public Classical_literalContext classical_literal() {
			return getRuleContext(Classical_literalContext.class,0);
		}
		public TerminalNode COLON() { return getToken(ASPParser.COLON, 0); }
		public Naf_literalsContext naf_literals() {
			return getRuleContext(Naf_literalsContext.class,0);
		}
		public Choice_elementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_choice_element; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterChoice_element(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitChoice_element(this);
		}
	}

	public final Choice_elementContext choice_element() throws RecognitionException {
		Choice_elementContext _localctx = new Choice_elementContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_choice_element);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(137);
			classical_literal();
			setState(142);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(138);
				match(COLON);
				setState(140);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1118270L) != 0)) {
					{
					setState(139);
					naf_literals();
					}
				}

				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AggregateContext extends ParserRuleContext {
		public Aggregate_functionContext aggregate_function() {
			return getRuleContext(Aggregate_functionContext.class,0);
		}
		public TerminalNode CURLY_OPEN() { return getToken(ASPParser.CURLY_OPEN, 0); }
		public TerminalNode CURLY_CLOSE() { return getToken(ASPParser.CURLY_CLOSE, 0); }
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public List<BinopContext> binop() {
			return getRuleContexts(BinopContext.class);
		}
		public BinopContext binop(int i) {
			return getRuleContext(BinopContext.class,i);
		}
		public Aggregate_elementsContext aggregate_elements() {
			return getRuleContext(Aggregate_elementsContext.class,0);
		}
		public AggregateContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_aggregate; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterAggregate(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitAggregate(this);
		}
	}

	public final AggregateContext aggregate() throws RecognitionException {
		AggregateContext _localctx = new AggregateContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_aggregate);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(147);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1114174L) != 0)) {
				{
				setState(144);
				term(0);
				setState(145);
				binop();
				}
			}

			setState(149);
			aggregate_function();
			setState(150);
			match(CURLY_OPEN);
			setState(152);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1115710L) != 0)) {
				{
				setState(151);
				aggregate_elements();
				}
			}

			setState(154);
			match(CURLY_CLOSE);
			setState(158);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4227858432L) != 0)) {
				{
				setState(155);
				binop();
				setState(156);
				term(0);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Aggregate_elementsContext extends ParserRuleContext {
		public Aggregate_elementContext aggregate_element() {
			return getRuleContext(Aggregate_elementContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(ASPParser.SEMICOLON, 0); }
		public Aggregate_elementsContext aggregate_elements() {
			return getRuleContext(Aggregate_elementsContext.class,0);
		}
		public Aggregate_elementsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_aggregate_elements; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterAggregate_elements(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitAggregate_elements(this);
		}
	}

	public final Aggregate_elementsContext aggregate_elements() throws RecognitionException {
		Aggregate_elementsContext _localctx = new Aggregate_elementsContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_aggregate_elements);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(160);
			aggregate_element();
			{
			setState(161);
			match(SEMICOLON);
			setState(162);
			aggregate_elements();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Aggregate_elementContext extends ParserRuleContext {
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public TerminalNode COLON() { return getToken(ASPParser.COLON, 0); }
		public Naf_literalsContext naf_literals() {
			return getRuleContext(Naf_literalsContext.class,0);
		}
		public Aggregate_elementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_aggregate_element; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterAggregate_element(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitAggregate_element(this);
		}
	}

	public final Aggregate_elementContext aggregate_element() throws RecognitionException {
		Aggregate_elementContext _localctx = new Aggregate_elementContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_aggregate_element);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(165);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1114174L) != 0)) {
				{
				setState(164);
				terms();
				}
			}

			setState(171);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(167);
				match(COLON);
				setState(169);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1118270L) != 0)) {
					{
					setState(168);
					naf_literals();
					}
				}

				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Aggregate_functionContext extends ParserRuleContext {
		public TerminalNode AGGREGATE_COUNT() { return getToken(ASPParser.AGGREGATE_COUNT, 0); }
		public TerminalNode AGGREGATE_MAX() { return getToken(ASPParser.AGGREGATE_MAX, 0); }
		public TerminalNode AGGREGATE_MIN() { return getToken(ASPParser.AGGREGATE_MIN, 0); }
		public TerminalNode AGGREGATE_SUM() { return getToken(ASPParser.AGGREGATE_SUM, 0); }
		public Aggregate_functionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_aggregate_function; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterAggregate_function(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitAggregate_function(this);
		}
	}

	public final Aggregate_functionContext aggregate_function() throws RecognitionException {
		Aggregate_functionContext _localctx = new Aggregate_functionContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_aggregate_function);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(173);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 64424509440L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class OptimizeContext extends ParserRuleContext {
		public Optimize_functionContext optimize_function() {
			return getRuleContext(Optimize_functionContext.class,0);
		}
		public TerminalNode CURLY_OPEN() { return getToken(ASPParser.CURLY_OPEN, 0); }
		public TerminalNode CURLY_CLOSE() { return getToken(ASPParser.CURLY_CLOSE, 0); }
		public Optimize_elementsContext optimize_elements() {
			return getRuleContext(Optimize_elementsContext.class,0);
		}
		public OptimizeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optimize; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterOptimize(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitOptimize(this);
		}
	}

	public final OptimizeContext optimize() throws RecognitionException {
		OptimizeContext _localctx = new OptimizeContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_optimize);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(175);
			optimize_function();
			setState(176);
			match(CURLY_OPEN);
			setState(178);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1114174L) != 0)) {
				{
				setState(177);
				optimize_elements();
				}
			}

			setState(180);
			match(CURLY_CLOSE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Optimize_elementsContext extends ParserRuleContext {
		public Optimize_elementContext optimize_element() {
			return getRuleContext(Optimize_elementContext.class,0);
		}
		public Optimize_elementsContext optimize_elements() {
			return getRuleContext(Optimize_elementsContext.class,0);
		}
		public TerminalNode SEMICOLON() { return getToken(ASPParser.SEMICOLON, 0); }
		public Optimize_elementsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optimize_elements; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterOptimize_elements(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitOptimize_elements(this);
		}
	}

	public final Optimize_elementsContext optimize_elements() throws RecognitionException {
		Optimize_elementsContext _localctx = new Optimize_elementsContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_optimize_elements);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(182);
			optimize_element();
			setState(186);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1114174L) != 0)) {
				{
				setState(183);
				optimize_elements();
				setState(184);
				match(SEMICOLON);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Optimize_elementContext extends ParserRuleContext {
		public Weight_at_levelContext weight_at_level() {
			return getRuleContext(Weight_at_levelContext.class,0);
		}
		public TerminalNode COLON() { return getToken(ASPParser.COLON, 0); }
		public Naf_literalsContext naf_literals() {
			return getRuleContext(Naf_literalsContext.class,0);
		}
		public Optimize_elementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optimize_element; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterOptimize_element(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitOptimize_element(this);
		}
	}

	public final Optimize_elementContext optimize_element() throws RecognitionException {
		Optimize_elementContext _localctx = new Optimize_elementContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_optimize_element);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(188);
			weight_at_level();
			setState(193);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COLON) {
				{
				setState(189);
				match(COLON);
				setState(191);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,25,_ctx) ) {
				case 1:
					{
					setState(190);
					naf_literals();
					}
					break;
				}
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Optimize_functionContext extends ParserRuleContext {
		public TerminalNode MAXIMIZE() { return getToken(ASPParser.MAXIMIZE, 0); }
		public TerminalNode MINIMIZE() { return getToken(ASPParser.MINIMIZE, 0); }
		public Optimize_functionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_optimize_function; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterOptimize_function(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitOptimize_function(this);
		}
	}

	public final Optimize_functionContext optimize_function() throws RecognitionException {
		Optimize_functionContext _localctx = new Optimize_functionContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_optimize_function);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(195);
			_la = _input.LA(1);
			if ( !(_la==MAXIMIZE || _la==MINIMIZE) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Weight_at_levelContext extends ParserRuleContext {
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public TerminalNode AT() { return getToken(ASPParser.AT, 0); }
		public TerminalNode COMMA() { return getToken(ASPParser.COMMA, 0); }
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public Weight_at_levelContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_weight_at_level; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterWeight_at_level(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitWeight_at_level(this);
		}
	}

	public final Weight_at_levelContext weight_at_level() throws RecognitionException {
		Weight_at_levelContext _localctx = new Weight_at_levelContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_weight_at_level);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(197);
			term(0);
			setState(200);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AT) {
				{
				setState(198);
				match(AT);
				setState(199);
				term(0);
				}
			}

			setState(204);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(202);
				match(COMMA);
				setState(203);
				terms();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Naf_literalsContext extends ParserRuleContext {
		public Naf_literalContext naf_literal() {
			return getRuleContext(Naf_literalContext.class,0);
		}
		public Naf_literalsContext naf_literals() {
			return getRuleContext(Naf_literalsContext.class,0);
		}
		public TerminalNode COMMA() { return getToken(ASPParser.COMMA, 0); }
		public Naf_literalsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_naf_literals; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterNaf_literals(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitNaf_literals(this);
		}
	}

	public final Naf_literalsContext naf_literals() throws RecognitionException {
		Naf_literalsContext _localctx = new Naf_literalsContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_naf_literals);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(206);
			naf_literal();
			setState(210);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,29,_ctx) ) {
			case 1:
				{
				setState(207);
				naf_literals();
				setState(208);
				match(COMMA);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Naf_literalContext extends ParserRuleContext {
		public Classical_literalContext classical_literal() {
			return getRuleContext(Classical_literalContext.class,0);
		}
		public TerminalNode NAF() { return getToken(ASPParser.NAF, 0); }
		public Builtin_atomContext builtin_atom() {
			return getRuleContext(Builtin_atomContext.class,0);
		}
		public Naf_literalContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_naf_literal; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterNaf_literal(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitNaf_literal(this);
		}
	}

	public final Naf_literalContext naf_literal() throws RecognitionException {
		Naf_literalContext _localctx = new Naf_literalContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_naf_literal);
		int _la;
		try {
			setState(217);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,31,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(213);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAF) {
					{
					setState(212);
					match(NAF);
					}
				}

				setState(215);
				classical_literal();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(216);
				builtin_atom();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Classical_literalContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(ASPParser.ID, 0); }
		public TerminalNode MINUS() { return getToken(ASPParser.MINUS, 0); }
		public TerminalNode PAREN_OPEN() { return getToken(ASPParser.PAREN_OPEN, 0); }
		public TerminalNode PAREN_CLOSE() { return getToken(ASPParser.PAREN_CLOSE, 0); }
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public Classical_literalContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_classical_literal; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterClassical_literal(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitClassical_literal(this);
		}
	}

	public final Classical_literalContext classical_literal() throws RecognitionException {
		Classical_literalContext _localctx = new Classical_literalContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_classical_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(220);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==MINUS) {
				{
				setState(219);
				match(MINUS);
				}
			}

			setState(222);
			match(ID);
			setState(228);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,34,_ctx) ) {
			case 1:
				{
				setState(223);
				match(PAREN_OPEN);
				setState(225);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1114174L) != 0)) {
					{
					setState(224);
					terms();
					}
				}

				setState(227);
				match(PAREN_CLOSE);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class Builtin_atomContext extends ParserRuleContext {
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public BinopContext binop() {
			return getRuleContext(BinopContext.class,0);
		}
		public Builtin_atomContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_builtin_atom; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterBuiltin_atom(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitBuiltin_atom(this);
		}
	}

	public final Builtin_atomContext builtin_atom() throws RecognitionException {
		Builtin_atomContext _localctx = new Builtin_atomContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_builtin_atom);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(230);
			term(0);
			setState(231);
			binop();
			setState(232);
			term(0);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BinopContext extends ParserRuleContext {
		public TerminalNode EQUAL() { return getToken(ASPParser.EQUAL, 0); }
		public TerminalNode UNEQUAL() { return getToken(ASPParser.UNEQUAL, 0); }
		public TerminalNode LESS() { return getToken(ASPParser.LESS, 0); }
		public TerminalNode GREATER() { return getToken(ASPParser.GREATER, 0); }
		public TerminalNode LESS_OR_EQ() { return getToken(ASPParser.LESS_OR_EQ, 0); }
		public TerminalNode GREATER_OR_EQ() { return getToken(ASPParser.GREATER_OR_EQ, 0); }
		public BinopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_binop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterBinop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitBinop(this);
		}
	}

	public final BinopContext binop() throws RecognitionException {
		BinopContext _localctx = new BinopContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_binop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(234);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 4227858432L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TermsContext extends ParserRuleContext {
		public TermContext term() {
			return getRuleContext(TermContext.class,0);
		}
		public TerminalNode COMMA() { return getToken(ASPParser.COMMA, 0); }
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public TermsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_terms; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterTerms(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitTerms(this);
		}
	}

	public final TermsContext terms() throws RecognitionException {
		TermsContext _localctx = new TermsContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_terms);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(236);
			term(0);
			setState(239);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(237);
				match(COMMA);
				setState(238);
				terms();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class TermContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(ASPParser.ID, 0); }
		public TerminalNode PAREN_OPEN() { return getToken(ASPParser.PAREN_OPEN, 0); }
		public TerminalNode PAREN_CLOSE() { return getToken(ASPParser.PAREN_CLOSE, 0); }
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public TerminalNode NUMBER() { return getToken(ASPParser.NUMBER, 0); }
		public TerminalNode STRING() { return getToken(ASPParser.STRING, 0); }
		public TerminalNode VARIABLE() { return getToken(ASPParser.VARIABLE, 0); }
		public TerminalNode ANONYMOUS_VARIABLE() { return getToken(ASPParser.ANONYMOUS_VARIABLE, 0); }
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public TerminalNode MINUS() { return getToken(ASPParser.MINUS, 0); }
		public ArithopContext arithop() {
			return getRuleContext(ArithopContext.class,0);
		}
		public TermContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_term; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitTerm(this);
		}
	}

	public final TermContext term() throws RecognitionException {
		return term(0);
	}

	private TermContext term(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		TermContext _localctx = new TermContext(_ctx, _parentState);
		TermContext _prevctx = _localctx;
		int _startState = 50;
		enterRecursionRule(_localctx, 50, RULE_term, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(260);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case ID:
				{
				setState(242);
				match(ID);
				setState(248);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,37,_ctx) ) {
				case 1:
					{
					setState(243);
					match(PAREN_OPEN);
					setState(245);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1114174L) != 0)) {
						{
						setState(244);
						terms();
						}
					}

					setState(247);
					match(PAREN_CLOSE);
					}
					break;
				}
				}
				break;
			case NUMBER:
				{
				setState(250);
				match(NUMBER);
				}
				break;
			case STRING:
				{
				setState(251);
				match(STRING);
				}
				break;
			case VARIABLE:
				{
				setState(252);
				match(VARIABLE);
				}
				break;
			case ANONYMOUS_VARIABLE:
				{
				setState(253);
				match(ANONYMOUS_VARIABLE);
				}
				break;
			case PAREN_OPEN:
				{
				setState(254);
				match(PAREN_OPEN);
				setState(255);
				term(0);
				setState(256);
				match(PAREN_CLOSE);
				}
				break;
			case MINUS:
				{
				setState(258);
				match(MINUS);
				setState(259);
				term(2);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(268);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,39,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new TermContext(_parentctx, _parentState);
					pushNewRecursionContext(_localctx, _startState, RULE_term);
					setState(262);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(263);
					arithop();
					setState(264);
					term(2);
					}
					} 
				}
				setState(270);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,39,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ArithopContext extends ParserRuleContext {
		public TerminalNode PLUS() { return getToken(ASPParser.PLUS, 0); }
		public TerminalNode MINUS() { return getToken(ASPParser.MINUS, 0); }
		public TerminalNode TIMES() { return getToken(ASPParser.TIMES, 0); }
		public TerminalNode DIV() { return getToken(ASPParser.DIV, 0); }
		public ArithopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arithop; }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterArithop(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitArithop(this);
		}
	}

	public final ArithopContext arithop() throws RecognitionException {
		ArithopContext _localctx = new ArithopContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_arithop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(271);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 491520L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 25:
			return term_sempred((TermContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean term_sempred(TermContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 1);
		}
		return true;
	}

	public static final String _serializedATN =
		"\u0004\u0001*\u0112\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001\u0002"+
		"\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004\u0002"+
		"\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007\u0002"+
		"\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b\u0002"+
		"\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002\u000f\u0007\u000f"+
		"\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002\u0012\u0007\u0012"+
		"\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002\u0015\u0007\u0015"+
		"\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002\u0018\u0007\u0018"+
		"\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a\u0001\u0000\u0001\u0000"+
		"\u0001\u0000\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0002\u0005\u0002"+
		">\b\u0002\n\u0002\f\u0002A\t\u0002\u0001\u0003\u0001\u0003\u0003\u0003"+
		"E\b\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0003\u0003"+
		"K\b\u0003\u0003\u0003M\b\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001"+
		"\u0003\u0003\u0003S\b\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001"+
		"\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0003\u0003]\b"+
		"\u0003\u0001\u0004\u0001\u0004\u0003\u0004a\b\u0004\u0001\u0005\u0001"+
		"\u0005\u0003\u0005e\b\u0005\u0001\u0005\u0003\u0005h\b\u0005\u0001\u0005"+
		"\u0001\u0005\u0003\u0005l\b\u0005\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0005\u0006q\b\u0006\n\u0006\f\u0006t\t\u0006\u0001\u0007\u0001\u0007"+
		"\u0001\u0007\u0003\u0007y\b\u0007\u0001\u0007\u0001\u0007\u0003\u0007"+
		"}\b\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0003\u0007"+
		"\u0083\b\u0007\u0001\b\u0001\b\u0001\b\u0003\b\u0088\b\b\u0001\t\u0001"+
		"\t\u0001\t\u0003\t\u008d\b\t\u0003\t\u008f\b\t\u0001\n\u0001\n\u0001\n"+
		"\u0003\n\u0094\b\n\u0001\n\u0001\n\u0001\n\u0003\n\u0099\b\n\u0001\n\u0001"+
		"\n\u0001\n\u0001\n\u0003\n\u009f\b\n\u0001\u000b\u0001\u000b\u0001\u000b"+
		"\u0001\u000b\u0001\f\u0003\f\u00a6\b\f\u0001\f\u0001\f\u0003\f\u00aa\b"+
		"\f\u0003\f\u00ac\b\f\u0001\r\u0001\r\u0001\u000e\u0001\u000e\u0001\u000e"+
		"\u0003\u000e\u00b3\b\u000e\u0001\u000e\u0001\u000e\u0001\u000f\u0001\u000f"+
		"\u0001\u000f\u0001\u000f\u0003\u000f\u00bb\b\u000f\u0001\u0010\u0001\u0010"+
		"\u0001\u0010\u0003\u0010\u00c0\b\u0010\u0003\u0010\u00c2\b\u0010\u0001"+
		"\u0011\u0001\u0011\u0001\u0012\u0001\u0012\u0001\u0012\u0003\u0012\u00c9"+
		"\b\u0012\u0001\u0012\u0001\u0012\u0003\u0012\u00cd\b\u0012\u0001\u0013"+
		"\u0001\u0013\u0001\u0013\u0001\u0013\u0003\u0013\u00d3\b\u0013\u0001\u0014"+
		"\u0003\u0014\u00d6\b\u0014\u0001\u0014\u0001\u0014\u0003\u0014\u00da\b"+
		"\u0014\u0001\u0015\u0003\u0015\u00dd\b\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0015\u0003\u0015\u00e2\b\u0015\u0001\u0015\u0003\u0015\u00e5\b\u0015"+
		"\u0001\u0016\u0001\u0016\u0001\u0016\u0001\u0016\u0001\u0017\u0001\u0017"+
		"\u0001\u0018\u0001\u0018\u0001\u0018\u0003\u0018\u00f0\b\u0018\u0001\u0019"+
		"\u0001\u0019\u0001\u0019\u0001\u0019\u0003\u0019\u00f6\b\u0019\u0001\u0019"+
		"\u0003\u0019\u00f9\b\u0019\u0001\u0019\u0001\u0019\u0001\u0019\u0001\u0019"+
		"\u0001\u0019\u0001\u0019\u0001\u0019\u0001\u0019\u0001\u0019\u0001\u0019"+
		"\u0003\u0019\u0105\b\u0019\u0001\u0019\u0001\u0019\u0001\u0019\u0001\u0019"+
		"\u0005\u0019\u010b\b\u0019\n\u0019\f\u0019\u010e\t\u0019\u0001\u001a\u0001"+
		"\u001a\u0001\u001a\u0000\u00012\u001b\u0000\u0002\u0004\u0006\b\n\f\u000e"+
		"\u0010\u0012\u0014\u0016\u0018\u001a\u001c\u001e \"$&(*,.024\u0000\u0004"+
		"\u0001\u0000 #\u0001\u0000$%\u0001\u0000\u001a\u001f\u0001\u0000\u000f"+
		"\u0012\u0125\u00006\u0001\u0000\u0000\u0000\u00029\u0001\u0000\u0000\u0000"+
		"\u0004?\u0001\u0000\u0000\u0000\u0006\\\u0001\u0000\u0000\u0000\b`\u0001"+
		"\u0000\u0000\u0000\ng\u0001\u0000\u0000\u0000\fm\u0001\u0000\u0000\u0000"+
		"\u000ex\u0001\u0000\u0000\u0000\u0010\u0084\u0001\u0000\u0000\u0000\u0012"+
		"\u0089\u0001\u0000\u0000\u0000\u0014\u0093\u0001\u0000\u0000\u0000\u0016"+
		"\u00a0\u0001\u0000\u0000\u0000\u0018\u00a5\u0001\u0000\u0000\u0000\u001a"+
		"\u00ad\u0001\u0000\u0000\u0000\u001c\u00af\u0001\u0000\u0000\u0000\u001e"+
		"\u00b6\u0001\u0000\u0000\u0000 \u00bc\u0001\u0000\u0000\u0000\"\u00c3"+
		"\u0001\u0000\u0000\u0000$\u00c5\u0001\u0000\u0000\u0000&\u00ce\u0001\u0000"+
		"\u0000\u0000(\u00d9\u0001\u0000\u0000\u0000*\u00dc\u0001\u0000\u0000\u0000"+
		",\u00e6\u0001\u0000\u0000\u0000.\u00ea\u0001\u0000\u0000\u00000\u00ec"+
		"\u0001\u0000\u0000\u00002\u0104\u0001\u0000\u0000\u00004\u010f\u0001\u0000"+
		"\u0000\u000067\u0003\u0004\u0002\u000078\u0003\u0002\u0001\u00008\u0001"+
		"\u0001\u0000\u0000\u00009:\u0003*\u0015\u0000:;\u0005\b\u0000\u0000;\u0003"+
		"\u0001\u0000\u0000\u0000<>\u0003\u0006\u0003\u0000=<\u0001\u0000\u0000"+
		"\u0000>A\u0001\u0000\u0000\u0000?=\u0001\u0000\u0000\u0000?@\u0001\u0000"+
		"\u0000\u0000@\u0005\u0001\u0000\u0000\u0000A?\u0001\u0000\u0000\u0000"+
		"BD\u0005\r\u0000\u0000CE\u0003\n\u0005\u0000DC\u0001\u0000\u0000\u0000"+
		"DE\u0001\u0000\u0000\u0000EF\u0001\u0000\u0000\u0000F]\u0005\u0006\u0000"+
		"\u0000GL\u0003\b\u0004\u0000HJ\u0005\r\u0000\u0000IK\u0003\n\u0005\u0000"+
		"JI\u0001\u0000\u0000\u0000JK\u0001\u0000\u0000\u0000KM\u0001\u0000\u0000"+
		"\u0000LH\u0001\u0000\u0000\u0000LM\u0001\u0000\u0000\u0000MN\u0001\u0000"+
		"\u0000\u0000NO\u0005\u0006\u0000\u0000O]\u0001\u0000\u0000\u0000PR\u0005"+
		"\u000e\u0000\u0000QS\u0003\n\u0005\u0000RQ\u0001\u0000\u0000\u0000RS\u0001"+
		"\u0000\u0000\u0000ST\u0001\u0000\u0000\u0000TU\u0005\u0006\u0000\u0000"+
		"UV\u0005\u0016\u0000\u0000VW\u0003$\u0012\u0000WX\u0005\u0017\u0000\u0000"+
		"X]\u0001\u0000\u0000\u0000YZ\u0003\u001c\u000e\u0000Z[\u0005\u0006\u0000"+
		"\u0000[]\u0001\u0000\u0000\u0000\\B\u0001\u0000\u0000\u0000\\G\u0001\u0000"+
		"\u0000\u0000\\P\u0001\u0000\u0000\u0000\\Y\u0001\u0000\u0000\u0000]\u0007"+
		"\u0001\u0000\u0000\u0000^a\u0003\f\u0006\u0000_a\u0003\u000e\u0007\u0000"+
		"`^\u0001\u0000\u0000\u0000`_\u0001\u0000\u0000\u0000a\t\u0001\u0000\u0000"+
		"\u0000bh\u0003(\u0014\u0000ce\u0005\f\u0000\u0000dc\u0001\u0000\u0000"+
		"\u0000de\u0001\u0000\u0000\u0000ef\u0001\u0000\u0000\u0000fh\u0003\u0014"+
		"\n\u0000gb\u0001\u0000\u0000\u0000gd\u0001\u0000\u0000\u0000hk\u0001\u0000"+
		"\u0000\u0000ij\u0005\u0007\u0000\u0000jl\u0003\n\u0005\u0000ki\u0001\u0000"+
		"\u0000\u0000kl\u0001\u0000\u0000\u0000l\u000b\u0001\u0000\u0000\u0000"+
		"mr\u0003*\u0015\u0000no\u0005\u000b\u0000\u0000oq\u0003*\u0015\u0000p"+
		"n\u0001\u0000\u0000\u0000qt\u0001\u0000\u0000\u0000rp\u0001\u0000\u0000"+
		"\u0000rs\u0001\u0000\u0000\u0000s\r\u0001\u0000\u0000\u0000tr\u0001\u0000"+
		"\u0000\u0000uv\u00032\u0019\u0000vw\u0003.\u0017\u0000wy\u0001\u0000\u0000"+
		"\u0000xu\u0001\u0000\u0000\u0000xy\u0001\u0000\u0000\u0000yz\u0001\u0000"+
		"\u0000\u0000z|\u0005\u0018\u0000\u0000{}\u0003\u0010\b\u0000|{\u0001\u0000"+
		"\u0000\u0000|}\u0001\u0000\u0000\u0000}~\u0001\u0000\u0000\u0000~\u0082"+
		"\u0005\u0019\u0000\u0000\u007f\u0080\u0003.\u0017\u0000\u0080\u0081\u0003"+
		"2\u0019\u0000\u0081\u0083\u0001\u0000\u0000\u0000\u0082\u007f\u0001\u0000"+
		"\u0000\u0000\u0082\u0083\u0001\u0000\u0000\u0000\u0083\u000f\u0001\u0000"+
		"\u0000\u0000\u0084\u0087\u0003\u0012\t\u0000\u0085\u0086\u0005\n\u0000"+
		"\u0000\u0086\u0088\u0003\u0010\b\u0000\u0087\u0085\u0001\u0000\u0000\u0000"+
		"\u0087\u0088\u0001\u0000\u0000\u0000\u0088\u0011\u0001\u0000\u0000\u0000"+
		"\u0089\u008e\u0003*\u0015\u0000\u008a\u008c\u0005\t\u0000\u0000\u008b"+
		"\u008d\u0003&\u0013\u0000\u008c\u008b\u0001\u0000\u0000\u0000\u008c\u008d"+
		"\u0001\u0000\u0000\u0000\u008d\u008f\u0001\u0000\u0000\u0000\u008e\u008a"+
		"\u0001\u0000\u0000\u0000\u008e\u008f\u0001\u0000\u0000\u0000\u008f\u0013"+
		"\u0001\u0000\u0000\u0000\u0090\u0091\u00032\u0019\u0000\u0091\u0092\u0003"+
		".\u0017\u0000\u0092\u0094\u0001\u0000\u0000\u0000\u0093\u0090\u0001\u0000"+
		"\u0000\u0000\u0093\u0094\u0001\u0000\u0000\u0000\u0094\u0095\u0001\u0000"+
		"\u0000\u0000\u0095\u0096\u0003\u001a\r\u0000\u0096\u0098\u0005\u0018\u0000"+
		"\u0000\u0097\u0099\u0003\u0016\u000b\u0000\u0098\u0097\u0001\u0000\u0000"+
		"\u0000\u0098\u0099\u0001\u0000\u0000\u0000\u0099\u009a\u0001\u0000\u0000"+
		"\u0000\u009a\u009e\u0005\u0019\u0000\u0000\u009b\u009c\u0003.\u0017\u0000"+
		"\u009c\u009d\u00032\u0019\u0000\u009d\u009f\u0001\u0000\u0000\u0000\u009e"+
		"\u009b\u0001\u0000\u0000\u0000\u009e\u009f\u0001\u0000\u0000\u0000\u009f"+
		"\u0015\u0001\u0000\u0000\u0000\u00a0\u00a1\u0003\u0018\f\u0000\u00a1\u00a2"+
		"\u0005\n\u0000\u0000\u00a2\u00a3\u0003\u0016\u000b\u0000\u00a3\u0017\u0001"+
		"\u0000\u0000\u0000\u00a4\u00a6\u00030\u0018\u0000\u00a5\u00a4\u0001\u0000"+
		"\u0000\u0000\u00a5\u00a6\u0001\u0000\u0000\u0000\u00a6\u00ab\u0001\u0000"+
		"\u0000\u0000\u00a7\u00a9\u0005\t\u0000\u0000\u00a8\u00aa\u0003&\u0013"+
		"\u0000\u00a9\u00a8\u0001\u0000\u0000\u0000\u00a9\u00aa\u0001\u0000\u0000"+
		"\u0000\u00aa\u00ac\u0001\u0000\u0000\u0000\u00ab\u00a7\u0001\u0000\u0000"+
		"\u0000\u00ab\u00ac\u0001\u0000\u0000\u0000\u00ac\u0019\u0001\u0000\u0000"+
		"\u0000\u00ad\u00ae\u0007\u0000\u0000\u0000\u00ae\u001b\u0001\u0000\u0000"+
		"\u0000\u00af\u00b0\u0003\"\u0011\u0000\u00b0\u00b2\u0005\u0018\u0000\u0000"+
		"\u00b1\u00b3\u0003\u001e\u000f\u0000\u00b2\u00b1\u0001\u0000\u0000\u0000"+
		"\u00b2\u00b3\u0001\u0000\u0000\u0000\u00b3\u00b4\u0001\u0000\u0000\u0000"+
		"\u00b4\u00b5\u0005\u0019\u0000\u0000\u00b5\u001d\u0001\u0000\u0000\u0000"+
		"\u00b6\u00ba\u0003 \u0010\u0000\u00b7\u00b8\u0003\u001e\u000f\u0000\u00b8"+
		"\u00b9\u0005\n\u0000\u0000\u00b9\u00bb\u0001\u0000\u0000\u0000\u00ba\u00b7"+
		"\u0001\u0000\u0000\u0000\u00ba\u00bb\u0001\u0000\u0000\u0000\u00bb\u001f"+
		"\u0001\u0000\u0000\u0000\u00bc\u00c1\u0003$\u0012\u0000\u00bd\u00bf\u0005"+
		"\t\u0000\u0000\u00be\u00c0\u0003&\u0013\u0000\u00bf\u00be\u0001\u0000"+
		"\u0000\u0000\u00bf\u00c0\u0001\u0000\u0000\u0000\u00c0\u00c2\u0001\u0000"+
		"\u0000\u0000\u00c1\u00bd\u0001\u0000\u0000\u0000\u00c1\u00c2\u0001\u0000"+
		"\u0000\u0000\u00c2!\u0001\u0000\u0000\u0000\u00c3\u00c4\u0007\u0001\u0000"+
		"\u0000\u00c4#\u0001\u0000\u0000\u0000\u00c5\u00c8\u00032\u0019\u0000\u00c6"+
		"\u00c7\u0005\u0013\u0000\u0000\u00c7\u00c9\u00032\u0019\u0000\u00c8\u00c6"+
		"\u0001\u0000\u0000\u0000\u00c8\u00c9\u0001\u0000\u0000\u0000\u00c9\u00cc"+
		"\u0001\u0000\u0000\u0000\u00ca\u00cb\u0005\u0007\u0000\u0000\u00cb\u00cd"+
		"\u00030\u0018\u0000\u00cc\u00ca\u0001\u0000\u0000\u0000\u00cc\u00cd\u0001"+
		"\u0000\u0000\u0000\u00cd%\u0001\u0000\u0000\u0000\u00ce\u00d2\u0003(\u0014"+
		"\u0000\u00cf\u00d0\u0003&\u0013\u0000\u00d0\u00d1\u0005\u0007\u0000\u0000"+
		"\u00d1\u00d3\u0001\u0000\u0000\u0000\u00d2\u00cf\u0001\u0000\u0000\u0000"+
		"\u00d2\u00d3\u0001\u0000\u0000\u0000\u00d3\'\u0001\u0000\u0000\u0000\u00d4"+
		"\u00d6\u0005\f\u0000\u0000\u00d5\u00d4\u0001\u0000\u0000\u0000\u00d5\u00d6"+
		"\u0001\u0000\u0000\u0000\u00d6\u00d7\u0001\u0000\u0000\u0000\u00d7\u00da"+
		"\u0003*\u0015\u0000\u00d8\u00da\u0003,\u0016\u0000\u00d9\u00d5\u0001\u0000"+
		"\u0000\u0000\u00d9\u00d8\u0001\u0000\u0000\u0000\u00da)\u0001\u0000\u0000"+
		"\u0000\u00db\u00dd\u0005\u0010\u0000\u0000\u00dc\u00db\u0001\u0000\u0000"+
		"\u0000\u00dc\u00dd\u0001\u0000\u0000\u0000\u00dd\u00de\u0001\u0000\u0000"+
		"\u0000\u00de\u00e4\u0005\u0001\u0000\u0000\u00df\u00e1\u0005\u0014\u0000"+
		"\u0000\u00e0\u00e2\u00030\u0018\u0000\u00e1\u00e0\u0001\u0000\u0000\u0000"+
		"\u00e1\u00e2\u0001\u0000\u0000\u0000\u00e2\u00e3\u0001\u0000\u0000\u0000"+
		"\u00e3\u00e5\u0005\u0015\u0000\u0000\u00e4\u00df\u0001\u0000\u0000\u0000"+
		"\u00e4\u00e5\u0001\u0000\u0000\u0000\u00e5+\u0001\u0000\u0000\u0000\u00e6"+
		"\u00e7\u00032\u0019\u0000\u00e7\u00e8\u0003.\u0017\u0000\u00e8\u00e9\u0003"+
		"2\u0019\u0000\u00e9-\u0001\u0000\u0000\u0000\u00ea\u00eb\u0007\u0002\u0000"+
		"\u0000\u00eb/\u0001\u0000\u0000\u0000\u00ec\u00ef\u00032\u0019\u0000\u00ed"+
		"\u00ee\u0005\u0007\u0000\u0000\u00ee\u00f0\u00030\u0018\u0000\u00ef\u00ed"+
		"\u0001\u0000\u0000\u0000\u00ef\u00f0\u0001\u0000\u0000\u0000\u00f01\u0001"+
		"\u0000\u0000\u0000\u00f1\u00f2\u0006\u0019\uffff\uffff\u0000\u00f2\u00f8"+
		"\u0005\u0001\u0000\u0000\u00f3\u00f5\u0005\u0014\u0000\u0000\u00f4\u00f6"+
		"\u00030\u0018\u0000\u00f5\u00f4\u0001\u0000\u0000\u0000\u00f5\u00f6\u0001"+
		"\u0000\u0000\u0000\u00f6\u00f7\u0001\u0000\u0000\u0000\u00f7\u00f9\u0005"+
		"\u0015\u0000\u0000\u00f8\u00f3\u0001\u0000\u0000\u0000\u00f8\u00f9\u0001"+
		"\u0000\u0000\u0000\u00f9\u0105\u0001\u0000\u0000\u0000\u00fa\u0105\u0005"+
		"\u0004\u0000\u0000\u00fb\u0105\u0005\u0003\u0000\u0000\u00fc\u0105\u0005"+
		"\u0002\u0000\u0000\u00fd\u0105\u0005\u0005\u0000\u0000\u00fe\u00ff\u0005"+
		"\u0014\u0000\u0000\u00ff\u0100\u00032\u0019\u0000\u0100\u0101\u0005\u0015"+
		"\u0000\u0000\u0101\u0105\u0001\u0000\u0000\u0000\u0102\u0103\u0005\u0010"+
		"\u0000\u0000\u0103\u0105\u00032\u0019\u0002\u0104\u00f1\u0001\u0000\u0000"+
		"\u0000\u0104\u00fa\u0001\u0000\u0000\u0000\u0104\u00fb\u0001\u0000\u0000"+
		"\u0000\u0104\u00fc\u0001\u0000\u0000\u0000\u0104\u00fd\u0001\u0000\u0000"+
		"\u0000\u0104\u00fe\u0001\u0000\u0000\u0000\u0104\u0102\u0001\u0000\u0000"+
		"\u0000\u0105\u010c\u0001\u0000\u0000\u0000\u0106\u0107\n\u0001\u0000\u0000"+
		"\u0107\u0108\u00034\u001a\u0000\u0108\u0109\u00032\u0019\u0002\u0109\u010b"+
		"\u0001\u0000\u0000\u0000\u010a\u0106\u0001\u0000\u0000\u0000\u010b\u010e"+
		"\u0001\u0000\u0000\u0000\u010c\u010a\u0001\u0000\u0000\u0000\u010c\u010d"+
		"\u0001\u0000\u0000\u0000\u010d3\u0001\u0000\u0000\u0000\u010e\u010c\u0001"+
		"\u0000\u0000\u0000\u010f\u0110\u0007\u0003\u0000\u0000\u01105\u0001\u0000"+
		"\u0000\u0000(?DJLR\\`dgkrx|\u0082\u0087\u008c\u008e\u0093\u0098\u009e"+
		"\u00a5\u00a9\u00ab\u00b2\u00ba\u00bf\u00c1\u00c8\u00cc\u00d2\u00d5\u00d9"+
		"\u00dc\u00e1\u00e4\u00ef\u00f5\u00f8\u0104\u010c";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}