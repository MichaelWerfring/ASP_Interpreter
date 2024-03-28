// Generated from C:/Users/micha/Desktop/4_Semester/Logikprogrammierung/ASP_Interpreter/asp_interpreter_lib/ANTLR/ASP.g4 by ANTLR 4.13.1
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
		GREATER_OR_EQ=27, DISUNIFICATION=28, ID=29, VARIABLE=30, COMMENT=31, MULTI_LINE_COMMENT=32, 
		BLANK=33, NEWLINE=34, TAB=35, WS=36;
	public static final int
		RULE_program = 0, RULE_query = 1, RULE_statements = 2, RULE_statement = 3, 
		RULE_head = 4, RULE_body = 5, RULE_naf_literals = 6, RULE_naf_literal = 7, 
		RULE_classical_literal = 8, RULE_binary_operation = 9, RULE_binary_operator = 10, 
		RULE_terms = 11, RULE_term = 12, RULE_arithop = 13;
	private static String[] makeRuleNames() {
		return new String[] {
			"program", "query", "statements", "statement", "head", "body", "naf_literals", 
			"naf_literal", "classical_literal", "binary_operation", "binary_operator", 
			"terms", "term", "arithop"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, null, null, "'_'", "'.'", "','", "'?'", "':'", "';'", "'|'", "'not'", 
			"':-'", "'+'", "'-'", "'*'", "'/'", "'@'", "'('", "')'", "'['", "']'", 
			"'{'", "'}'", "'='", "'<'", "'>'", "'<='", "'>='", "'\\='", null, null, 
			null, null, null, null, "'\\t'", "' '"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "STRING", "NUMBER", "ANONYMOUS_VARIABLE", "DOT", "COMMA", "QUERY_MARK", 
			"COLON", "SEMICOLON", "OR", "NAF", "CONS", "PLUS", "MINUS", "TIMES", 
			"DIV", "AT", "PAREN_OPEN", "PAREN_CLOSE", "SQUARE_OPEN", "SQUARE_CLOSE", 
			"CURLY_OPEN", "CURLY_CLOSE", "EQUAL", "LESS", "GREATER", "LESS_OR_EQ", 
			"GREATER_OR_EQ", "DISUNIFICATION", "ID", "VARIABLE", "COMMENT", "MULTI_LINE_COMMENT", 
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitProgram(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ProgramContext program() throws RecognitionException {
		ProgramContext _localctx = new ProgramContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_program);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(28);
			statements();
			setState(29);
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitQuery(this);
			else return visitor.visitChildren(this);
		}
	}

	public final QueryContext query() throws RecognitionException {
		QueryContext _localctx = new QueryContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_query);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(31);
			classical_literal();
			setState(32);
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitStatements(this);
			else return visitor.visitChildren(this);
		}
	}

	public final StatementsContext statements() throws RecognitionException {
		StatementsContext _localctx = new StatementsContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_statements);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(37);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,0,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(34);
					statement();
					}
					} 
				}
				setState(39);
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitStatement(this);
			else return visitor.visitChildren(this);
		}
	}

	public final StatementContext statement() throws RecognitionException {
		StatementContext _localctx = new StatementContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_statement);
		int _la;
		try {
			setState(54);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CONS:
				enterOuterAlt(_localctx, 1);
				{
				setState(40);
				match(CONS);
				setState(42);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1610753038L) != 0)) {
					{
					setState(41);
					body();
					}
				}

				setState(44);
				match(DOT);
				}
				break;
			case MINUS:
			case ID:
				enterOuterAlt(_localctx, 2);
				{
				setState(45);
				head();
				setState(50);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==CONS) {
					{
					setState(46);
					match(CONS);
					setState(48);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1610753038L) != 0)) {
						{
						setState(47);
						body();
						}
					}

					}
				}

				setState(52);
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
		public Classical_literalContext classical_literal() {
			return getRuleContext(Classical_literalContext.class,0);
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitHead(this);
			else return visitor.visitChildren(this);
		}
	}

	public final HeadContext head() throws RecognitionException {
		HeadContext _localctx = new HeadContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_head);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(56);
			classical_literal();
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
		public List<Naf_literalContext> naf_literal() {
			return getRuleContexts(Naf_literalContext.class);
		}
		public Naf_literalContext naf_literal(int i) {
			return getRuleContext(Naf_literalContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(ASPParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(ASPParser.COMMA, i);
		}
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitBody(this);
			else return visitor.visitChildren(this);
		}
	}

	public final BodyContext body() throws RecognitionException {
		BodyContext _localctx = new BodyContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_body);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(58);
			naf_literal();
			setState(63);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(59);
				match(COMMA);
				setState(60);
				naf_literal();
				}
				}
				setState(65);
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
	public static class Naf_literalsContext extends ParserRuleContext {
		public Naf_literalContext naf_literal() {
			return getRuleContext(Naf_literalContext.class,0);
		}
		public TerminalNode COMMA() { return getToken(ASPParser.COMMA, 0); }
		public Naf_literalsContext naf_literals() {
			return getRuleContext(Naf_literalsContext.class,0);
		}
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitNaf_literals(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Naf_literalsContext naf_literals() throws RecognitionException {
		Naf_literalsContext _localctx = new Naf_literalsContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_naf_literals);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(66);
			naf_literal();
			setState(69);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				{
				setState(67);
				match(COMMA);
				setState(68);
				naf_literals();
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
		public Binary_operationContext binary_operation() {
			return getRuleContext(Binary_operationContext.class,0);
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitNaf_literal(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Naf_literalContext naf_literal() throws RecognitionException {
		Naf_literalContext _localctx = new Naf_literalContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_naf_literal);
		int _la;
		try {
			setState(76);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,8,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(72);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==NAF) {
					{
					setState(71);
					match(NAF);
					}
				}

				setState(74);
				classical_literal();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(75);
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitClassical_literal(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Classical_literalContext classical_literal() throws RecognitionException {
		Classical_literalContext _localctx = new Classical_literalContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_classical_literal);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(79);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==MINUS) {
				{
				setState(78);
				match(MINUS);
				}
			}

			setState(81);
			match(ID);
			setState(87);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==PAREN_OPEN) {
				{
				setState(82);
				match(PAREN_OPEN);
				setState(84);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1610752014L) != 0)) {
					{
					setState(83);
					terms();
					}
				}

				setState(86);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterBinary_operation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitBinary_operation(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitBinary_operation(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Binary_operationContext binary_operation() throws RecognitionException {
		Binary_operationContext _localctx = new Binary_operationContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_binary_operation);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(89);
			term(0);
			setState(90);
			binary_operator();
			setState(91);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterLessOrEqOperation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitLessOrEqOperation(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitLessOrEqOperation(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class GreaterOrEqOperationContext extends Binary_operatorContext {
		public TerminalNode GREATER_OR_EQ() { return getToken(ASPParser.GREATER_OR_EQ, 0); }
		public GreaterOrEqOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterGreaterOrEqOperation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitGreaterOrEqOperation(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitGreaterOrEqOperation(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class DisunificationOperationContext extends Binary_operatorContext {
		public TerminalNode DISUNIFICATION() { return getToken(ASPParser.DISUNIFICATION, 0); }
		public DisunificationOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterDisunificationOperation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitDisunificationOperation(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitDisunificationOperation(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class LessOperationContext extends Binary_operatorContext {
		public TerminalNode LESS() { return getToken(ASPParser.LESS, 0); }
		public LessOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterLessOperation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitLessOperation(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitLessOperation(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class GreaterOperationContext extends Binary_operatorContext {
		public TerminalNode GREATER() { return getToken(ASPParser.GREATER, 0); }
		public GreaterOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterGreaterOperation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitGreaterOperation(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitGreaterOperation(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class EqualityOperationContext extends Binary_operatorContext {
		public TerminalNode EQUAL() { return getToken(ASPParser.EQUAL, 0); }
		public EqualityOperationContext(Binary_operatorContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterEqualityOperation(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitEqualityOperation(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitEqualityOperation(this);
			else return visitor.visitChildren(this);
		}
	}

	public final Binary_operatorContext binary_operator() throws RecognitionException {
		Binary_operatorContext _localctx = new Binary_operatorContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_binary_operator);
		try {
			setState(99);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case EQUAL:
				_localctx = new EqualityOperationContext(_localctx);
				enterOuterAlt(_localctx, 1);
				{
				setState(93);
				match(EQUAL);
				}
				break;
			case DISUNIFICATION:
				_localctx = new DisunificationOperationContext(_localctx);
				enterOuterAlt(_localctx, 2);
				{
				setState(94);
				match(DISUNIFICATION);
				}
				break;
			case LESS:
				_localctx = new LessOperationContext(_localctx);
				enterOuterAlt(_localctx, 3);
				{
				setState(95);
				match(LESS);
				}
				break;
			case GREATER:
				_localctx = new GreaterOperationContext(_localctx);
				enterOuterAlt(_localctx, 4);
				{
				setState(96);
				match(GREATER);
				}
				break;
			case LESS_OR_EQ:
				_localctx = new LessOrEqOperationContext(_localctx);
				enterOuterAlt(_localctx, 5);
				{
				setState(97);
				match(LESS_OR_EQ);
				}
				break;
			case GREATER_OR_EQ:
				_localctx = new GreaterOrEqOperationContext(_localctx);
				enterOuterAlt(_localctx, 6);
				{
				setState(98);
				match(GREATER_OR_EQ);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterTerms(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitTerms(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitTerms(this);
			else return visitor.visitChildren(this);
		}
	}

	public final TermsContext terms() throws RecognitionException {
		TermsContext _localctx = new TermsContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_terms);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(101);
			term(0);
			setState(104);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==COMMA) {
				{
				setState(102);
				match(COMMA);
				setState(103);
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterNegatedTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitNegatedTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitNegatedTerm(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class StringTermContext extends TermContext {
		public TerminalNode STRING() { return getToken(ASPParser.STRING, 0); }
		public StringTermContext(TermContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterStringTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitStringTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitStringTerm(this);
			else return visitor.visitChildren(this);
		}
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterBasicTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitBasicTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitBasicTerm(this);
			else return visitor.visitChildren(this);
		}
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
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterArithmeticOperationTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitArithmeticOperationTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitArithmeticOperationTerm(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class ParenthesizedTermContext extends TermContext {
		public TerminalNode PAREN_OPEN() { return getToken(ASPParser.PAREN_OPEN, 0); }
		public TermContext term() {
			return getRuleContext(TermContext.class,0);
		}
		public TerminalNode PAREN_CLOSE() { return getToken(ASPParser.PAREN_CLOSE, 0); }
		public ParenthesizedTermContext(TermContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterParenthesizedTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitParenthesizedTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitParenthesizedTerm(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class AnonymousVariableTermContext extends TermContext {
		public TerminalNode ANONYMOUS_VARIABLE() { return getToken(ASPParser.ANONYMOUS_VARIABLE, 0); }
		public AnonymousVariableTermContext(TermContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterAnonymousVariableTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitAnonymousVariableTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitAnonymousVariableTerm(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class NumberTermContext extends TermContext {
		public TerminalNode NUMBER() { return getToken(ASPParser.NUMBER, 0); }
		public NumberTermContext(TermContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterNumberTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitNumberTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitNumberTerm(this);
			else return visitor.visitChildren(this);
		}
	}
	@SuppressWarnings("CheckReturnValue")
	public static class VariableTermContext extends TermContext {
		public TerminalNode VARIABLE() { return getToken(ASPParser.VARIABLE, 0); }
		public VariableTermContext(TermContext ctx) { copyFrom(ctx); }
		@Override
		public void enterRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).enterVariableTerm(this);
		}
		@Override
		public void exitRule(ParseTreeListener listener) {
			if ( listener instanceof ASPListener ) ((ASPListener)listener).exitVariableTerm(this);
		}
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitVariableTerm(this);
			else return visitor.visitChildren(this);
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
		int _startState = 24;
		enterRecursionRule(_localctx, 24, RULE_term, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(125);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case ID:
				{
				_localctx = new BasicTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;

				setState(107);
				match(ID);
				setState(113);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,15,_ctx) ) {
				case 1:
					{
					setState(108);
					match(PAREN_OPEN);
					setState(110);
					_errHandler.sync(this);
					_la = _input.LA(1);
					if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 1610752014L) != 0)) {
						{
						setState(109);
						terms();
						}
					}

					setState(112);
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
				setState(115);
				match(NUMBER);
				}
				break;
			case STRING:
				{
				_localctx = new StringTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(116);
				match(STRING);
				}
				break;
			case VARIABLE:
				{
				_localctx = new VariableTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(117);
				match(VARIABLE);
				}
				break;
			case ANONYMOUS_VARIABLE:
				{
				_localctx = new AnonymousVariableTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(118);
				match(ANONYMOUS_VARIABLE);
				}
				break;
			case PAREN_OPEN:
				{
				_localctx = new ParenthesizedTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(119);
				match(PAREN_OPEN);
				setState(120);
				term(0);
				setState(121);
				match(PAREN_CLOSE);
				}
				break;
			case MINUS:
				{
				_localctx = new NegatedTermContext(_localctx);
				_ctx = _localctx;
				_prevctx = _localctx;
				setState(123);
				match(MINUS);
				setState(124);
				term(2);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(133);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new ArithmeticOperationTermContext(new TermContext(_parentctx, _parentState));
					pushNewRecursionContext(_localctx, _startState, RULE_term);
					setState(127);
					if (!(precpred(_ctx, 1))) throw new FailedPredicateException(this, "precpred(_ctx, 1)");
					setState(128);
					arithop();
					setState(129);
					term(2);
					}
					} 
				}
				setState(135);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,17,_ctx);
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
		@Override
		public <T> T accept(ParseTreeVisitor<? extends T> visitor) {
			if ( visitor instanceof ASPVisitor ) return ((ASPVisitor<? extends T>)visitor).visitArithop(this);
			else return visitor.visitChildren(this);
		}
	}

	public final ArithopContext arithop() throws RecognitionException {
		ArithopContext _localctx = new ArithopContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_arithop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(136);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 61440L) != 0)) ) {
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
		case 12:
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
		"\u0004\u0001$\u008b\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001\u0002"+
		"\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004\u0002"+
		"\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007\u0002"+
		"\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b\u0002"+
		"\f\u0007\f\u0002\r\u0007\r\u0001\u0000\u0001\u0000\u0001\u0000\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0002\u0005\u0002$\b\u0002\n\u0002\f\u0002"+
		"\'\t\u0002\u0001\u0003\u0001\u0003\u0003\u0003+\b\u0003\u0001\u0003\u0001"+
		"\u0003\u0001\u0003\u0001\u0003\u0003\u00031\b\u0003\u0003\u00033\b\u0003"+
		"\u0001\u0003\u0001\u0003\u0003\u00037\b\u0003\u0001\u0004\u0001\u0004"+
		"\u0001\u0005\u0001\u0005\u0001\u0005\u0005\u0005>\b\u0005\n\u0005\f\u0005"+
		"A\t\u0005\u0001\u0006\u0001\u0006\u0001\u0006\u0003\u0006F\b\u0006\u0001"+
		"\u0007\u0003\u0007I\b\u0007\u0001\u0007\u0001\u0007\u0003\u0007M\b\u0007"+
		"\u0001\b\u0003\bP\b\b\u0001\b\u0001\b\u0001\b\u0003\bU\b\b\u0001\b\u0003"+
		"\bX\b\b\u0001\t\u0001\t\u0001\t\u0001\t\u0001\n\u0001\n\u0001\n\u0001"+
		"\n\u0001\n\u0001\n\u0003\nd\b\n\u0001\u000b\u0001\u000b\u0001\u000b\u0003"+
		"\u000bi\b\u000b\u0001\f\u0001\f\u0001\f\u0001\f\u0003\fo\b\f\u0001\f\u0003"+
		"\fr\b\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001"+
		"\f\u0001\f\u0001\f\u0003\f~\b\f\u0001\f\u0001\f\u0001\f\u0001\f\u0005"+
		"\f\u0084\b\f\n\f\f\f\u0087\t\f\u0001\r\u0001\r\u0001\r\u0000\u0001\u0018"+
		"\u000e\u0000\u0002\u0004\u0006\b\n\f\u000e\u0010\u0012\u0014\u0016\u0018"+
		"\u001a\u0000\u0001\u0001\u0000\f\u000f\u0097\u0000\u001c\u0001\u0000\u0000"+
		"\u0000\u0002\u001f\u0001\u0000\u0000\u0000\u0004%\u0001\u0000\u0000\u0000"+
		"\u00066\u0001\u0000\u0000\u0000\b8\u0001\u0000\u0000\u0000\n:\u0001\u0000"+
		"\u0000\u0000\fB\u0001\u0000\u0000\u0000\u000eL\u0001\u0000\u0000\u0000"+
		"\u0010O\u0001\u0000\u0000\u0000\u0012Y\u0001\u0000\u0000\u0000\u0014c"+
		"\u0001\u0000\u0000\u0000\u0016e\u0001\u0000\u0000\u0000\u0018}\u0001\u0000"+
		"\u0000\u0000\u001a\u0088\u0001\u0000\u0000\u0000\u001c\u001d\u0003\u0004"+
		"\u0002\u0000\u001d\u001e\u0003\u0002\u0001\u0000\u001e\u0001\u0001\u0000"+
		"\u0000\u0000\u001f \u0003\u0010\b\u0000 !\u0005\u0006\u0000\u0000!\u0003"+
		"\u0001\u0000\u0000\u0000\"$\u0003\u0006\u0003\u0000#\"\u0001\u0000\u0000"+
		"\u0000$\'\u0001\u0000\u0000\u0000%#\u0001\u0000\u0000\u0000%&\u0001\u0000"+
		"\u0000\u0000&\u0005\u0001\u0000\u0000\u0000\'%\u0001\u0000\u0000\u0000"+
		"(*\u0005\u000b\u0000\u0000)+\u0003\n\u0005\u0000*)\u0001\u0000\u0000\u0000"+
		"*+\u0001\u0000\u0000\u0000+,\u0001\u0000\u0000\u0000,7\u0005\u0004\u0000"+
		"\u0000-2\u0003\b\u0004\u0000.0\u0005\u000b\u0000\u0000/1\u0003\n\u0005"+
		"\u00000/\u0001\u0000\u0000\u000001\u0001\u0000\u0000\u000013\u0001\u0000"+
		"\u0000\u00002.\u0001\u0000\u0000\u000023\u0001\u0000\u0000\u000034\u0001"+
		"\u0000\u0000\u000045\u0005\u0004\u0000\u000057\u0001\u0000\u0000\u0000"+
		"6(\u0001\u0000\u0000\u00006-\u0001\u0000\u0000\u00007\u0007\u0001\u0000"+
		"\u0000\u000089\u0003\u0010\b\u00009\t\u0001\u0000\u0000\u0000:?\u0003"+
		"\u000e\u0007\u0000;<\u0005\u0005\u0000\u0000<>\u0003\u000e\u0007\u0000"+
		"=;\u0001\u0000\u0000\u0000>A\u0001\u0000\u0000\u0000?=\u0001\u0000\u0000"+
		"\u0000?@\u0001\u0000\u0000\u0000@\u000b\u0001\u0000\u0000\u0000A?\u0001"+
		"\u0000\u0000\u0000BE\u0003\u000e\u0007\u0000CD\u0005\u0005\u0000\u0000"+
		"DF\u0003\f\u0006\u0000EC\u0001\u0000\u0000\u0000EF\u0001\u0000\u0000\u0000"+
		"F\r\u0001\u0000\u0000\u0000GI\u0005\n\u0000\u0000HG\u0001\u0000\u0000"+
		"\u0000HI\u0001\u0000\u0000\u0000IJ\u0001\u0000\u0000\u0000JM\u0003\u0010"+
		"\b\u0000KM\u0003\u0012\t\u0000LH\u0001\u0000\u0000\u0000LK\u0001\u0000"+
		"\u0000\u0000M\u000f\u0001\u0000\u0000\u0000NP\u0005\r\u0000\u0000ON\u0001"+
		"\u0000\u0000\u0000OP\u0001\u0000\u0000\u0000PQ\u0001\u0000\u0000\u0000"+
		"QW\u0005\u001d\u0000\u0000RT\u0005\u0011\u0000\u0000SU\u0003\u0016\u000b"+
		"\u0000TS\u0001\u0000\u0000\u0000TU\u0001\u0000\u0000\u0000UV\u0001\u0000"+
		"\u0000\u0000VX\u0005\u0012\u0000\u0000WR\u0001\u0000\u0000\u0000WX\u0001"+
		"\u0000\u0000\u0000X\u0011\u0001\u0000\u0000\u0000YZ\u0003\u0018\f\u0000"+
		"Z[\u0003\u0014\n\u0000[\\\u0003\u0018\f\u0000\\\u0013\u0001\u0000\u0000"+
		"\u0000]d\u0005\u0017\u0000\u0000^d\u0005\u001c\u0000\u0000_d\u0005\u0018"+
		"\u0000\u0000`d\u0005\u0019\u0000\u0000ad\u0005\u001a\u0000\u0000bd\u0005"+
		"\u001b\u0000\u0000c]\u0001\u0000\u0000\u0000c^\u0001\u0000\u0000\u0000"+
		"c_\u0001\u0000\u0000\u0000c`\u0001\u0000\u0000\u0000ca\u0001\u0000\u0000"+
		"\u0000cb\u0001\u0000\u0000\u0000d\u0015\u0001\u0000\u0000\u0000eh\u0003"+
		"\u0018\f\u0000fg\u0005\u0005\u0000\u0000gi\u0003\u0016\u000b\u0000hf\u0001"+
		"\u0000\u0000\u0000hi\u0001\u0000\u0000\u0000i\u0017\u0001\u0000\u0000"+
		"\u0000jk\u0006\f\uffff\uffff\u0000kq\u0005\u001d\u0000\u0000ln\u0005\u0011"+
		"\u0000\u0000mo\u0003\u0016\u000b\u0000nm\u0001\u0000\u0000\u0000no\u0001"+
		"\u0000\u0000\u0000op\u0001\u0000\u0000\u0000pr\u0005\u0012\u0000\u0000"+
		"ql\u0001\u0000\u0000\u0000qr\u0001\u0000\u0000\u0000r~\u0001\u0000\u0000"+
		"\u0000s~\u0005\u0002\u0000\u0000t~\u0005\u0001\u0000\u0000u~\u0005\u001e"+
		"\u0000\u0000v~\u0005\u0003\u0000\u0000wx\u0005\u0011\u0000\u0000xy\u0003"+
		"\u0018\f\u0000yz\u0005\u0012\u0000\u0000z~\u0001\u0000\u0000\u0000{|\u0005"+
		"\r\u0000\u0000|~\u0003\u0018\f\u0002}j\u0001\u0000\u0000\u0000}s\u0001"+
		"\u0000\u0000\u0000}t\u0001\u0000\u0000\u0000}u\u0001\u0000\u0000\u0000"+
		"}v\u0001\u0000\u0000\u0000}w\u0001\u0000\u0000\u0000}{\u0001\u0000\u0000"+
		"\u0000~\u0085\u0001\u0000\u0000\u0000\u007f\u0080\n\u0001\u0000\u0000"+
		"\u0080\u0081\u0003\u001a\r\u0000\u0081\u0082\u0003\u0018\f\u0002\u0082"+
		"\u0084\u0001\u0000\u0000\u0000\u0083\u007f\u0001\u0000\u0000\u0000\u0084"+
		"\u0087\u0001\u0000\u0000\u0000\u0085\u0083\u0001\u0000\u0000\u0000\u0085"+
		"\u0086\u0001\u0000\u0000\u0000\u0086\u0019\u0001\u0000\u0000\u0000\u0087"+
		"\u0085\u0001\u0000\u0000\u0000\u0088\u0089\u0007\u0000\u0000\u0000\u0089"+
		"\u001b\u0001\u0000\u0000\u0000\u0012%*026?EHLOTWchnq}\u0085";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}