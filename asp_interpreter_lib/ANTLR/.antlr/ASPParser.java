// Generated from d:/FH/Semester 4/Logikprogrammierung ILV/SASP-Projekt/ASP_Interpreter/asp_interpreter_lib/ANTLR/ASP.g4 by ANTLR 4.13.1
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
		STRING=1, NUMBER=2, ANONYMOUS_VARIABLE=3, DOT=4, COMMA=5, QUERY_MARK=6, 
		COLON=7, SEMICOLON=8, OR=9, NAF=10, CONS=11, PLUS=12, MINUS=13, TIMES=14, 
		DIV=15, AT=16, PAREN_OPEN=17, PAREN_CLOSE=18, SQUARE_OPEN=19, SQUARE_CLOSE=20, 
		CURLY_OPEN=21, CURLY_CLOSE=22, EQUAL=23, LESS=24, GREATER=25, LESS_OR_EQ=26, 
		GREATER_OR_EQ=27, DISUNIFICATION=28, IS=29, ID=30, VARIABLE=31, COMMENT=32, 
		MULTI_LINE_COMMENT=33, BLANK=34, NEWLINE=35, TAB=36, WS=37;
	public static final int
		RULE_program = 0, RULE_query = 1, RULE_statements = 2, RULE_statement = 3, 
		RULE_goal = 4, RULE_binary_operation = 5, RULE_literal = 6, RULE_binary_operator = 7, 
		RULE_terms = 8, RULE_term = 9, RULE_list = 10, RULE_arithop = 11;
	private static String[] makeRuleNames() {
		return new String[] {
			"program", "query", "statements", "statement", "goal", "binary_operation", 
			"literal", "binary_operator", "terms", "term", "list", "arithop"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, "'_'", "'.'", "','", "'?'", "':'", "';'", "'|'", "'not'", 
			"':-'", "'+'", "'-'", "'*'", "'/'", "'@'", "'('", "')'", "'['", "']'", 
			"'{'", "'}'", "'='", "'<'", "'>'", "'<='", "'>='", "'\\='", "'is'", null, 
			null, null, null, null, null, "'\\t'", "' '"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "STRING", "NUMBER", "ANONYMOUS_VARIABLE", "DOT", "COMMA", "QUERY_MARK", 
			"COLON", "SEMICOLON", "OR", "NAF", "CONS", "PLUS", "MINUS", "TIMES", 
			"DIV", "AT", "PAREN_OPEN", "PAREN_CLOSE", "SQUARE_OPEN", "SQUARE_CLOSE", 
			"CURLY_OPEN", "CURLY_CLOSE", "EQUAL", "LESS", "GREATER", "LESS_OR_EQ", 
			"GREATER_OR_EQ", "DISUNIFICATION", "IS", "ID", "VARIABLE", "COMMENT", 
			"MULTI_LINE_COMMENT", "BLANK", "NEWLINE", "TAB", "WS"
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
	}

	public final ProgramContext program() throws RecognitionException {
		ProgramContext _localctx = new ProgramContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_program);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(24);
			statements();
			setState(25);
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
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public TerminalNode QUERY_MARK() { return getToken(ASPParser.QUERY_MARK, 0); }
		public QueryContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_query; }
	}

	public final QueryContext query() throws RecognitionException {
		QueryContext _localctx = new QueryContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_query);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(27);
			literal();
			setState(28);
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
	}

	public final StatementsContext statements() throws RecognitionException {
		StatementsContext _localctx = new StatementsContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_statements);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(33);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,0,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(30);
					statement();
					}
					} 
				}
				setState(35);
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
		public List<GoalContext> goal() {
			return getRuleContexts(GoalContext.class);
		}
		public GoalContext goal(int i) {
			return getRuleContext(GoalContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(ASPParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(ASPParser.COMMA, i);
		}
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_statement; }
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_statement);
		int _la;
		try {
			setState(62);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CONS:
				enterOuterAlt(_localctx, 1);
				{
				setState(36);
				match(CONS);
				setState(45);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 3221890062L) != 0)) {
					{
					setState(37);
					goal();
					setState(42);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==COMMA) {
						{
						{
						setState(38);
						match(COMMA);
						setState(39);
						goal();
						}
						}
						setState(44);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					}
				}

				setState(47);
				match(DOT);
				}
				break;
			case NAF:
			case MINUS:
			case ID:
				enterOuterAlt(_localctx, 2);
				{
				setState(48);
				literal();
				setState(58);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==CONS) {
					{
					setState(49);
					match(CONS);
					{
					setState(50);
					goal();
					setState(55);
					_errHandler.sync(this);
					_la = _input.LA(1);
					while (_la==COMMA) {
						{
						{
						setState(51);
						match(COMMA);
						setState(52);
						goal();
						}
						}
						setState(57);
						_errHandler.sync(this);
						_la = _input.LA(1);
					}
					}
					}
				}

				setState(60);
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
	public static class GoalContext extends ParserRuleContext {
		public LiteralContext literal() {
			return getRuleContext(LiteralContext.class,0);
		}
		public Binary_operationContext binary_operation() {
			return getRuleContext(Binary_operationContext.class,0);
		}
		public GoalContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_goal; }
	}

	public final GoalContext goal() throws RecognitionException {
		GoalContext _localctx = new GoalContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_goal);
		try {
			setState(66);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(64);
				literal();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(65);
				binary_operation();
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
	public static class Binary_operationContext extends ParserRuleContext {
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public Binary_operatorContext binary_operator() {
			return getRuleContext(Binary_operatorContext.class,0);
		}
		public Binary_operationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_binary_operation; }
	}

	public final Binary_operationContext binary_operation() throws RecognitionException {
		Binary_operationContext _localctx = new Binary_operationContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_binary_operation);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(68);
			term(0);
			setState(69);
			binary_operator();
			setState(70);
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
	public static class LiteralContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(ASPParser.ID, 0); }
		public TerminalNode NAF() { return getToken(ASPParser.NAF, 0); }
		public TerminalNode MINUS() { return getToken(ASPParser.MINUS, 0); }
		public TerminalNode PAREN_OPEN() { return getToken(ASPParser.PAREN_OPEN, 0); }
		public TerminalNode PAREN_CLOSE() { return getToken(ASPParser.PAREN_CLOSE, 0); }
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public LiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_literal; }
	}

	public final LiteralContext literal() throws RecognitionException {
		LiteralContext _localctx = new LiteralContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(73);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==NAF) {
				{
				setState(72);
				match(NAF);
				}
			}

			setState(76);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==MINUS) {
				{
				setState(75);
				match(MINUS);
				}
			}

			setState(78);
			match(ID);
			setState(84);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==PAREN_OPEN) {
				{
				setState(79);
				match(PAREN_OPEN);
				setState(81);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 3221889038L) != 0)) {
					{
					setState(80);
					terms();
					}
				}

				setState(83);
				match(PAREN_CLOSE);
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
	public static class Binary_operatorContext extends ParserRuleContext {
		public Binary_operatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_binary_operator; }
	 
		public Binary_operatorContext() { }
		public void copyFrom(Binary_operatorContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class LessOrEqOperationContext extends Binary_operatorContext {
		public TerminalNode LESS_OR_EQ() { return getToken(ASPParser.LESS_OR_EQ, 0); }
		public LessOrEqOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class GreaterOrEqOperationContext extends Binary_operatorContext {
		public TerminalNode GREATER_OR_EQ() { return getToken(ASPParser.GREATER_OR_EQ, 0); }
		public GreaterOrEqOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class IsOperationContext extends Binary_operatorContext {
		public TerminalNode IS() { return getToken(ASPParser.IS, 0); }
		public IsOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class DisunificationOperationContext extends Binary_operatorContext {
		public TerminalNode DISUNIFICATION() { return getToken(ASPParser.DISUNIFICATION, 0); }
		public DisunificationOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class LessOperationContext extends Binary_operatorContext {
		public TerminalNode LESS() { return getToken(ASPParser.LESS, 0); }
		public LessOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class GreaterOperationContext extends Binary_operatorContext {
		public TerminalNode GREATER() { return getToken(ASPParser.GREATER, 0); }
		public GreaterOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class EqualityOperationContext extends Binary_operatorContext {
		public TerminalNode EQUAL() { return getToken(ASPParser.EQUAL, 0); }
		public EqualityOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
	}

	public final Binary_operatorContext binary_operator() throws RecognitionException {
		Binary_operatorContext _localctx = new Binary_operatorContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_binary_operator);
		try {
			setState(93);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case EQUAL:
				_localctx = new EqualityOperationContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(86);
				match(EQUAL);
				}
				break;
			case DISUNIFICATION:
				_localctx = new DisunificationOperationContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(87);
				match(DISUNIFICATION);
				}
				break;
			case LESS:
				_localctx = new LessOperationContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(88);
				match(LESS);
				}
				break;
			case GREATER:
				_localctx = new GreaterOperationContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(89);
				match(GREATER);
				}
				break;
			case LESS_OR_EQ:
				_localctx = new LessOrEqOperationContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(90);
				match(LESS_OR_EQ);
				}
				break;
			case GREATER_OR_EQ:
				_localctx = new GreaterOrEqOperationContext(_localctx);
				enterOuterAlt(_localctx, 6);
				{
				setState(91);
				match(GREATER_OR_EQ);
				}
				break;
			case IS:
				_localctx = new IsOperationContext(_localctx);
				enterOuterAlt(_localctx, 7);
				{
				setState(92);
				match(IS);
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
	}

	public final TermsContext terms() throws RecognitionException {
		TermsContext _localctx = new TermsContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_terms);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(95);
			term(0);
			setState(98);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(96);
				match(COMMA);
				setState(97);
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
		public TermContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_term; }
	 
		public TermContext() { }
		public void copyFrom(TermContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NegatedTermContext extends TermContext {
		public TerminalNode MINUS() { return getToken(ASPParser.MINUS, 0); }
		public TermContext term() {
			return getRuleContext(TermContext.class,0);
		}
		public NegatedTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class StringTermContext extends TermContext {
		public TerminalNode STRING() { return getToken(ASPParser.STRING, 0); }
		public StringTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class BasicTermContext extends TermContext {
		public TerminalNode ID() { return getToken(ASPParser.ID, 0); }
		public TerminalNode PAREN_OPEN() { return getToken(ASPParser.PAREN_OPEN, 0); }
		public TerminalNode PAREN_CLOSE() { return getToken(ASPParser.PAREN_CLOSE, 0); }
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public BasicTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ArithmeticOperationTermContext extends TermContext {
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public ArithopContext arithop() {
			return getRuleContext(ArithopContext.class,0);
		}
		public ArithmeticOperationTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ParenthesizedTermContext extends TermContext {
		public TerminalNode PAREN_OPEN() { return getToken(ASPParser.PAREN_OPEN, 0); }
		public TermContext term() {
			return getRuleContext(TermContext.class,0);
		}
		public TerminalNode PAREN_CLOSE() { return getToken(ASPParser.PAREN_CLOSE, 0); }
		public ParenthesizedTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class AnonymousVariableTermContext extends TermContext {
		public TerminalNode ANONYMOUS_VARIABLE() { return getToken(ASPParser.ANONYMOUS_VARIABLE, 0); }
		public AnonymousVariableTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NumberTermContext extends TermContext {
		public TerminalNode NUMBER() { return getToken(ASPParser.NUMBER, 0); }
		public NumberTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class VariableTermContext extends TermContext {
		public TerminalNode VARIABLE() { return getToken(ASPParser.VARIABLE, 0); }
		public VariableTermContext(TermContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ListTermContext extends TermContext {
		public ListContext list() {
			return getRuleContext(ListContext.class,0);
		}
		public ListTermContext(TermContext ctx) { copyFrom(ctx); }
	}

	public final TermContext term() throws RecognitionException {
		return term(0);
	}

	private TermContext term(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		TermContext _localctx = new TermContext(_ctx, _parentState);
		TermContext _prevctx = _localctx;
		int _startState = 18;
		enterRecursionRule(_localctx, 18, RULE_term, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(120);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case ID:
				{
				_localctx = new BasicTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(101);
				match(ID);
				setState(107);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,14,_ctx) ) {
				case 1:
					{
					setState(102);
					match(PAREN_OPEN);
					setState(104);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 3221889038L) != 0)) {
						{
						setState(103);
						terms();
						}
					}

					setState(106);
					match(PAREN_CLOSE);
					}
					break;
				}
				}
				break;
			case NUMBER:
				{
				_localctx = new NumberTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(109);
				match(NUMBER);
				}
				break;
			case STRING:
				{
				_localctx = new StringTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(110);
				match(STRING);
				}
				break;
			case VARIABLE:
				{
				_localctx = new VariableTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(111);
				match(VARIABLE);
				}
				break;
			case ANONYMOUS_VARIABLE:
				{
				_localctx = new AnonymousVariableTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(112);
				match(ANONYMOUS_VARIABLE);
				}
				break;
			case PAREN_OPEN:
				{
				_localctx = new ParenthesizedTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(113);
				match(PAREN_OPEN);
				setState(114);
				term(0);
				setState(115);
				match(PAREN_CLOSE);
				}
				break;
			case MINUS:
				{
				_localctx = new NegatedTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(117);
				match(MINUS);
				setState(118);
				term(3);
				}
				break;
			case SQUARE_OPEN:
				{
				_localctx = new ListTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(119);
				list();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(128);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,16,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new ArithmeticOperationTermContext(new TermContext(_parentctx, _parentState));
					pushNewRecursionContext(_localctx, _startState, RULE_term);
					setState(122);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(123);
					arithop();
					setState(124);
					term(2);
					}
					} 
				}
				setState(130);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,16,_ctx);
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
	public static class ListContext extends ParserRuleContext {
		public ListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_list; }
	 
		public ListContext() { }
		public void copyFrom(ListContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ConventionalListContext extends ListContext {
		public TerminalNode SQUARE_OPEN() { return getToken(ASPParser.SQUARE_OPEN, 0); }
		public TermsContext terms() {
			return getRuleContext(TermsContext.class,0);
		}
		public TerminalNode SQUARE_CLOSE() { return getToken(ASPParser.SQUARE_CLOSE, 0); }
		public ConventionalListContext(ListContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class RecursiveListContext extends ListContext {
		public TerminalNode SQUARE_OPEN() { return getToken(ASPParser.SQUARE_OPEN, 0); }
		public List<TermContext> term() {
			return getRuleContexts(TermContext.class);
		}
		public TermContext term(int i) {
			return getRuleContext(TermContext.class,i);
		}
		public TerminalNode OR() { return getToken(ASPParser.OR, 0); }
		public TerminalNode SQUARE_CLOSE() { return getToken(ASPParser.SQUARE_CLOSE, 0); }
		public RecursiveListContext(ListContext ctx) { copyFrom(ctx); }
	}

	public final ListContext list() throws RecognitionException {
		ListContext _localctx = new ListContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_list);
		try {
			setState(141);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,17,_ctx) ) {
			case 1:
				_localctx = new ConventionalListContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(131);
				match(SQUARE_OPEN);
				setState(132);
				terms();
				setState(133);
				match(SQUARE_CLOSE);
				}
				break;
			case 2:
				_localctx = new RecursiveListContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(135);
				match(SQUARE_OPEN);
				setState(136);
				term(0);
				setState(137);
				match(OR);
				setState(138);
				term(0);
				setState(139);
				match(SQUARE_CLOSE);
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
	public static class ArithopContext extends ParserRuleContext {
		public ArithopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arithop; }
	 
		public ArithopContext() { }
		public void copyFrom(ArithopContext ctx) {
			super.copyFrom(ctx);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class TimesOperationContext extends ArithopContext {
		public TerminalNode TIMES() { return getToken(ASPParser.TIMES, 0); }
		public TimesOperationContext(ArithopContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class MinusOperationContext extends ArithopContext {
		public TerminalNode MINUS() { return getToken(ASPParser.MINUS, 0); }
		public MinusOperationContext(ArithopContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class PlusOperationContext extends ArithopContext {
		public TerminalNode PLUS() { return getToken(ASPParser.PLUS, 0); }
		public PlusOperationContext(ArithopContext ctx) { copyFrom(ctx); }
	}
	@SuppressWarnings("CheckReturnValue")
	public static class DivOperationContext extends ArithopContext {
		public TerminalNode DIV() { return getToken(ASPParser.DIV, 0); }
		public DivOperationContext(ArithopContext ctx) { copyFrom(ctx); }
	}

	public final ArithopContext arithop() throws RecognitionException {
		ArithopContext _localctx = new ArithopContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_arithop);
		try {
			setState(147);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case PLUS:
				_localctx = new PlusOperationContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(143);
				match(PLUS);
				}
				break;
			case MINUS:
				_localctx = new MinusOperationContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(144);
				match(MINUS);
				}
				break;
			case TIMES:
				_localctx = new TimesOperationContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(145);
				match(TIMES);
				}
				break;
			case DIV:
				_localctx = new DivOperationContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(146);
				match(DIV);
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

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 9:
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
		"\u0004\u0001%\u0096\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001\u0002"+
		"\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004\u0002"+
		"\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007\u0002"+
		"\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b\u0001"+
		"\u0000\u0001\u0000\u0001\u0000\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0002\u0005\u0002 \b\u0002\n\u0002\f\u0002#\t\u0002\u0001\u0003\u0001"+
		"\u0003\u0001\u0003\u0001\u0003\u0005\u0003)\b\u0003\n\u0003\f\u0003,\t"+
		"\u0003\u0003\u0003.\b\u0003\u0001\u0003\u0001\u0003\u0001\u0003\u0001"+
		"\u0003\u0001\u0003\u0001\u0003\u0005\u00036\b\u0003\n\u0003\f\u00039\t"+
		"\u0003\u0003\u0003;\b\u0003\u0001\u0003\u0001\u0003\u0003\u0003?\b\u0003"+
		"\u0001\u0004\u0001\u0004\u0003\u0004C\b\u0004\u0001\u0005\u0001\u0005"+
		"\u0001\u0005\u0001\u0005\u0001\u0006\u0003\u0006J\b\u0006\u0001\u0006"+
		"\u0003\u0006M\b\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0003\u0006"+
		"R\b\u0006\u0001\u0006\u0003\u0006U\b\u0006\u0001\u0007\u0001\u0007\u0001"+
		"\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0003\u0007^\b"+
		"\u0007\u0001\b\u0001\b\u0001\b\u0003\bc\b\b\u0001\t\u0001\t\u0001\t\u0001"+
		"\t\u0003\ti\b\t\u0001\t\u0003\tl\b\t\u0001\t\u0001\t\u0001\t\u0001\t\u0001"+
		"\t\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0003\ty\b\t\u0001"+
		"\t\u0001\t\u0001\t\u0001\t\u0005\t\u007f\b\t\n\t\f\t\u0082\t\t\u0001\n"+
		"\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001\n\u0001"+
		"\n\u0003\n\u008e\b\n\u0001\u000b\u0001\u000b\u0001\u000b\u0001\u000b\u0003"+
		"\u000b\u0094\b\u000b\u0001\u000b\u0000\u0001\u0012\f\u0000\u0002\u0004"+
		"\u0006\b\n\f\u000e\u0010\u0012\u0014\u0016\u0000\u0000\u00a9\u0000\u0018"+
		"\u0001\u0000\u0000\u0000\u0002\u001b\u0001\u0000\u0000\u0000\u0004!\u0001"+
		"\u0000\u0000\u0000\u0006>\u0001\u0000\u0000\u0000\bB\u0001\u0000\u0000"+
		"\u0000\nD\u0001\u0000\u0000\u0000\fI\u0001\u0000\u0000\u0000\u000e]\u0001"+
		"\u0000\u0000\u0000\u0010_\u0001\u0000\u0000\u0000\u0012x\u0001\u0000\u0000"+
		"\u0000\u0014\u008d\u0001\u0000\u0000\u0000\u0016\u0093\u0001\u0000\u0000"+
		"\u0000\u0018\u0019\u0003\u0004\u0002\u0000\u0019\u001a\u0003\u0002\u0001"+
		"\u0000\u001a\u0001\u0001\u0000\u0000\u0000\u001b\u001c\u0003\f\u0006\u0000"+
		"\u001c\u001d\u0005\u0006\u0000\u0000\u001d\u0003\u0001\u0000\u0000\u0000"+
		"\u001e \u0003\u0006\u0003\u0000\u001f\u001e\u0001\u0000\u0000\u0000 #"+
		"\u0001\u0000\u0000\u0000!\u001f\u0001\u0000\u0000\u0000!\"\u0001\u0000"+
		"\u0000\u0000\"\u0005\u0001\u0000\u0000\u0000#!\u0001\u0000\u0000\u0000"+
		"$-\u0005\u000b\u0000\u0000%*\u0003\b\u0004\u0000&\'\u0005\u0005\u0000"+
		"\u0000\')\u0003\b\u0004\u0000(&\u0001\u0000\u0000\u0000),\u0001\u0000"+
		"\u0000\u0000*(\u0001\u0000\u0000\u0000*+\u0001\u0000\u0000\u0000+.\u0001"+
		"\u0000\u0000\u0000,*\u0001\u0000\u0000\u0000-%\u0001\u0000\u0000\u0000"+
		"-.\u0001\u0000\u0000\u0000./\u0001\u0000\u0000\u0000/?\u0005\u0004\u0000"+
		"\u00000:\u0003\f\u0006\u000012\u0005\u000b\u0000\u000027\u0003\b\u0004"+
		"\u000034\u0005\u0005\u0000\u000046\u0003\b\u0004\u000053\u0001\u0000\u0000"+
		"\u000069\u0001\u0000\u0000\u000075\u0001\u0000\u0000\u000078\u0001\u0000"+
		"\u0000\u00008;\u0001\u0000\u0000\u000097\u0001\u0000\u0000\u0000:1\u0001"+
		"\u0000\u0000\u0000:;\u0001\u0000\u0000\u0000;<\u0001\u0000\u0000\u0000"+
		"<=\u0005\u0004\u0000\u0000=?\u0001\u0000\u0000\u0000>$\u0001\u0000\u0000"+
		"\u0000>0\u0001\u0000\u0000\u0000?\u0007\u0001\u0000\u0000\u0000@C\u0003"+
		"\f\u0006\u0000AC\u0003\n\u0005\u0000B@\u0001\u0000\u0000\u0000BA\u0001"+
		"\u0000\u0000\u0000C\t\u0001\u0000\u0000\u0000DE\u0003\u0012\t\u0000EF"+
		"\u0003\u000e\u0007\u0000FG\u0003\u0012\t\u0000G\u000b\u0001\u0000\u0000"+
		"\u0000HJ\u0005\n\u0000\u0000IH\u0001\u0000\u0000\u0000IJ\u0001\u0000\u0000"+
		"\u0000JL\u0001\u0000\u0000\u0000KM\u0005\r\u0000\u0000LK\u0001\u0000\u0000"+
		"\u0000LM\u0001\u0000\u0000\u0000MN\u0001\u0000\u0000\u0000NT\u0005\u001e"+
		"\u0000\u0000OQ\u0005\u0011\u0000\u0000PR\u0003\u0010\b\u0000QP\u0001\u0000"+
		"\u0000\u0000QR\u0001\u0000\u0000\u0000RS\u0001\u0000\u0000\u0000SU\u0005"+
		"\u0012\u0000\u0000TO\u0001\u0000\u0000\u0000TU\u0001\u0000\u0000\u0000"+
		"U\r\u0001\u0000\u0000\u0000V^\u0005\u0017\u0000\u0000W^\u0005\u001c\u0000"+
		"\u0000X^\u0005\u0018\u0000\u0000Y^\u0005\u0019\u0000\u0000Z^\u0005\u001a"+
		"\u0000\u0000[^\u0005\u001b\u0000\u0000\\^\u0005\u001d\u0000\u0000]V\u0001"+
		"\u0000\u0000\u0000]W\u0001\u0000\u0000\u0000]X\u0001\u0000\u0000\u0000"+
		"]Y\u0001\u0000\u0000\u0000]Z\u0001\u0000\u0000\u0000][\u0001\u0000\u0000"+
		"\u0000]\\\u0001\u0000\u0000\u0000^\u000f\u0001\u0000\u0000\u0000_b\u0003"+
		"\u0012\t\u0000`a\u0005\u0005\u0000\u0000ac\u0003\u0010\b\u0000b`\u0001"+
		"\u0000\u0000\u0000bc\u0001\u0000\u0000\u0000c\u0011\u0001\u0000\u0000"+
		"\u0000de\u0006\t\uffff\uffff\u0000ek\u0005\u001e\u0000\u0000fh\u0005\u0011"+
		"\u0000\u0000gi\u0003\u0010\b\u0000hg\u0001\u0000\u0000\u0000hi\u0001\u0000"+
		"\u0000\u0000ij\u0001\u0000\u0000\u0000jl\u0005\u0012\u0000\u0000kf\u0001"+
		"\u0000\u0000\u0000kl\u0001\u0000\u0000\u0000ly\u0001\u0000\u0000\u0000"+
		"my\u0005\u0002\u0000\u0000ny\u0005\u0001\u0000\u0000oy\u0005\u001f\u0000"+
		"\u0000py\u0005\u0003\u0000\u0000qr\u0005\u0011\u0000\u0000rs\u0003\u0012"+
		"\t\u0000st\u0005\u0012\u0000\u0000ty\u0001\u0000\u0000\u0000uv\u0005\r"+
		"\u0000\u0000vy\u0003\u0012\t\u0003wy\u0003\u0014\n\u0000xd\u0001\u0000"+
		"\u0000\u0000xm\u0001\u0000\u0000\u0000xn\u0001\u0000\u0000\u0000xo\u0001"+
		"\u0000\u0000\u0000xp\u0001\u0000\u0000\u0000xq\u0001\u0000\u0000\u0000"+
		"xu\u0001\u0000\u0000\u0000xw\u0001\u0000\u0000\u0000y\u0080\u0001\u0000"+
		"\u0000\u0000z{\n\u0001\u0000\u0000{|\u0003\u0016\u000b\u0000|}\u0003\u0012"+
		"\t\u0002}\u007f\u0001\u0000\u0000\u0000~z\u0001\u0000\u0000\u0000\u007f"+
		"\u0082\u0001\u0000\u0000\u0000\u0080~\u0001\u0000\u0000\u0000\u0080\u0081"+
		"\u0001\u0000\u0000\u0000\u0081\u0013\u0001\u0000\u0000\u0000\u0082\u0080"+
		"\u0001\u0000\u0000\u0000\u0083\u0084\u0005\u0013\u0000\u0000\u0084\u0085"+
		"\u0003\u0010\b\u0000\u0085\u0086\u0005\u0014\u0000\u0000\u0086\u008e\u0001"+
		"\u0000\u0000\u0000\u0087\u0088\u0005\u0013\u0000\u0000\u0088\u0089\u0003"+
		"\u0012\t\u0000\u0089\u008a\u0005\t\u0000\u0000\u008a\u008b\u0003\u0012"+
		"\t\u0000\u008b\u008c\u0005\u0014\u0000\u0000\u008c\u008e\u0001\u0000\u0000"+
		"\u0000\u008d\u0083\u0001\u0000\u0000\u0000\u008d\u0087\u0001\u0000\u0000"+
		"\u0000\u008e\u0015\u0001\u0000\u0000\u0000\u008f\u0094\u0005\f\u0000\u0000"+
		"\u0090\u0094\u0005\r\u0000\u0000\u0091\u0094\u0005\u000e\u0000\u0000\u0092"+
		"\u0094\u0005\u000f\u0000\u0000\u0093\u008f\u0001\u0000\u0000\u0000\u0093"+
		"\u0090\u0001\u0000\u0000\u0000\u0093\u0091\u0001\u0000\u0000\u0000\u0093"+
		"\u0092\u0001\u0000\u0000\u0000\u0094\u0017\u0001\u0000\u0000\u0000\u0013"+
		"!*-7:>BILQT]bhkx\u0080\u008d\u0093";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}