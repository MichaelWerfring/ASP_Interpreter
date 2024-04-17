// Generated from c://Users//micha//Desktop//4_Semester//Logikprogrammierung//ASP_Interpreter//asp_interpreter_lib//ANTLR//ASP.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue", "this-escape"})
public class ASPLexer extends Lexer {
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
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"STRING", "NUMBER", "ANONYMOUS_VARIABLE", "DOT", "COMMA", "QUERY_MARK", 
			"COLON", "SEMICOLON", "OR", "NAF", "CONS", "PLUS", "MINUS", "TIMES", 
			"DIV", "AT", "PAREN_OPEN", "PAREN_CLOSE", "SQUARE_OPEN", "SQUARE_CLOSE", 
			"CURLY_OPEN", "CURLY_CLOSE", "EQUAL", "LESS", "GREATER", "LESS_OR_EQ", 
			"GREATER_OR_EQ", "DISUNIFICATION", "ID", "VARIABLE", "COMMENT", "MULTI_LINE_COMMENT", 
			"BLANK", "NEWLINE", "TAB", "WS"
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


	public ASPLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "ASP.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\u0004\u0000$\u00ce\u0006\uffff\uffff\u0002\u0000\u0007\u0000\u0002\u0001"+
		"\u0007\u0001\u0002\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004"+
		"\u0007\u0004\u0002\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007"+
		"\u0007\u0007\u0002\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b"+
		"\u0007\u000b\u0002\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002"+
		"\u000f\u0007\u000f\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002"+
		"\u0012\u0007\u0012\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002"+
		"\u0015\u0007\u0015\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002"+
		"\u0018\u0007\u0018\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a\u0002"+
		"\u001b\u0007\u001b\u0002\u001c\u0007\u001c\u0002\u001d\u0007\u001d\u0002"+
		"\u001e\u0007\u001e\u0002\u001f\u0007\u001f\u0002 \u0007 \u0002!\u0007"+
		"!\u0002\"\u0007\"\u0002#\u0007#\u0001\u0000\u0001\u0000\u0005\u0000L\b"+
		"\u0000\n\u0000\f\u0000O\t\u0000\u0001\u0000\u0001\u0000\u0001\u0001\u0001"+
		"\u0001\u0004\u0001U\b\u0001\u000b\u0001\f\u0001V\u0003\u0001Y\b\u0001"+
		"\u0001\u0002\u0001\u0002\u0001\u0003\u0001\u0003\u0001\u0004\u0001\u0004"+
		"\u0001\u0005\u0001\u0005\u0001\u0006\u0001\u0006\u0001\u0007\u0001\u0007"+
		"\u0001\b\u0001\b\u0001\t\u0001\t\u0001\t\u0001\t\u0001\n\u0001\n\u0001"+
		"\n\u0001\u000b\u0001\u000b\u0001\f\u0001\f\u0001\r\u0001\r\u0001\u000e"+
		"\u0001\u000e\u0001\u000f\u0001\u000f\u0001\u0010\u0001\u0010\u0001\u0011"+
		"\u0001\u0011\u0001\u0012\u0001\u0012\u0001\u0013\u0001\u0013\u0001\u0014"+
		"\u0001\u0014\u0001\u0015\u0001\u0015\u0001\u0016\u0001\u0016\u0001\u0017"+
		"\u0001\u0017\u0001\u0018\u0001\u0018\u0001\u0019\u0001\u0019\u0001\u0019"+
		"\u0001\u001a\u0001\u001a\u0001\u001a\u0001\u001b\u0001\u001b\u0001\u001b"+
		"\u0001\u001c\u0001\u001c\u0005\u001c\u0097\b\u001c\n\u001c\f\u001c\u009a"+
		"\t\u001c\u0001\u001d\u0001\u001d\u0005\u001d\u009e\b\u001d\n\u001d\f\u001d"+
		"\u00a1\t\u001d\u0001\u001e\u0001\u001e\u0005\u001e\u00a5\b\u001e\n\u001e"+
		"\f\u001e\u00a8\t\u001e\u0001\u001e\u0001\u001e\u0001\u001e\u0001\u001e"+
		"\u0001\u001f\u0001\u001f\u0001\u001f\u0001\u001f\u0005\u001f\u00b2\b\u001f"+
		"\n\u001f\f\u001f\u00b5\t\u001f\u0001\u001f\u0001\u001f\u0001\u001f\u0001"+
		"\u001f\u0001\u001f\u0001 \u0004 \u00bd\b \u000b \f \u00be\u0001 \u0001"+
		" \u0001!\u0001!\u0001!\u0001!\u0001\"\u0001\"\u0001\"\u0001\"\u0001#\u0001"+
		"#\u0001#\u0001#\u0001\u00b3\u0000$\u0001\u0001\u0003\u0002\u0005\u0003"+
		"\u0007\u0004\t\u0005\u000b\u0006\r\u0007\u000f\b\u0011\t\u0013\n\u0015"+
		"\u000b\u0017\f\u0019\r\u001b\u000e\u001d\u000f\u001f\u0010!\u0011#\u0012"+
		"%\u0013\'\u0014)\u0015+\u0016-\u0017/\u00181\u00193\u001a5\u001b7\u001c"+
		"9\u001d;\u001e=\u001f? A!C\"E#G$\u0001\u0000\b\u0002\u0000\"\"^^\u0001"+
		"\u000000\u0001\u000019\u0001\u0000az\u0004\u000009AZ__az\u0001\u0000A"+
		"Z\u0003\u0000\n\n\r\r??\u0003\u0000\t\n\r\r  \u00d5\u0000\u0001\u0001"+
		"\u0000\u0000\u0000\u0000\u0003\u0001\u0000\u0000\u0000\u0000\u0005\u0001"+
		"\u0000\u0000\u0000\u0000\u0007\u0001\u0000\u0000\u0000\u0000\t\u0001\u0000"+
		"\u0000\u0000\u0000\u000b\u0001\u0000\u0000\u0000\u0000\r\u0001\u0000\u0000"+
		"\u0000\u0000\u000f\u0001\u0000\u0000\u0000\u0000\u0011\u0001\u0000\u0000"+
		"\u0000\u0000\u0013\u0001\u0000\u0000\u0000\u0000\u0015\u0001\u0000\u0000"+
		"\u0000\u0000\u0017\u0001\u0000\u0000\u0000\u0000\u0019\u0001\u0000\u0000"+
		"\u0000\u0000\u001b\u0001\u0000\u0000\u0000\u0000\u001d\u0001\u0000\u0000"+
		"\u0000\u0000\u001f\u0001\u0000\u0000\u0000\u0000!\u0001\u0000\u0000\u0000"+
		"\u0000#\u0001\u0000\u0000\u0000\u0000%\u0001\u0000\u0000\u0000\u0000\'"+
		"\u0001\u0000\u0000\u0000\u0000)\u0001\u0000\u0000\u0000\u0000+\u0001\u0000"+
		"\u0000\u0000\u0000-\u0001\u0000\u0000\u0000\u0000/\u0001\u0000\u0000\u0000"+
		"\u00001\u0001\u0000\u0000\u0000\u00003\u0001\u0000\u0000\u0000\u00005"+
		"\u0001\u0000\u0000\u0000\u00007\u0001\u0000\u0000\u0000\u00009\u0001\u0000"+
		"\u0000\u0000\u0000;\u0001\u0000\u0000\u0000\u0000=\u0001\u0000\u0000\u0000"+
		"\u0000?\u0001\u0000\u0000\u0000\u0000A\u0001\u0000\u0000\u0000\u0000C"+
		"\u0001\u0000\u0000\u0000\u0000E\u0001\u0000\u0000\u0000\u0000G\u0001\u0000"+
		"\u0000\u0000\u0001I\u0001\u0000\u0000\u0000\u0003X\u0001\u0000\u0000\u0000"+
		"\u0005Z\u0001\u0000\u0000\u0000\u0007\\\u0001\u0000\u0000\u0000\t^\u0001"+
		"\u0000\u0000\u0000\u000b`\u0001\u0000\u0000\u0000\rb\u0001\u0000\u0000"+
		"\u0000\u000fd\u0001\u0000\u0000\u0000\u0011f\u0001\u0000\u0000\u0000\u0013"+
		"h\u0001\u0000\u0000\u0000\u0015l\u0001\u0000\u0000\u0000\u0017o\u0001"+
		"\u0000\u0000\u0000\u0019q\u0001\u0000\u0000\u0000\u001bs\u0001\u0000\u0000"+
		"\u0000\u001du\u0001\u0000\u0000\u0000\u001fw\u0001\u0000\u0000\u0000!"+
		"y\u0001\u0000\u0000\u0000#{\u0001\u0000\u0000\u0000%}\u0001\u0000\u0000"+
		"\u0000\'\u007f\u0001\u0000\u0000\u0000)\u0081\u0001\u0000\u0000\u0000"+
		"+\u0083\u0001\u0000\u0000\u0000-\u0085\u0001\u0000\u0000\u0000/\u0087"+
		"\u0001\u0000\u0000\u00001\u0089\u0001\u0000\u0000\u00003\u008b\u0001\u0000"+
		"\u0000\u00005\u008e\u0001\u0000\u0000\u00007\u0091\u0001\u0000\u0000\u0000"+
		"9\u0094\u0001\u0000\u0000\u0000;\u009b\u0001\u0000\u0000\u0000=\u00a2"+
		"\u0001\u0000\u0000\u0000?\u00ad\u0001\u0000\u0000\u0000A\u00bc\u0001\u0000"+
		"\u0000\u0000C\u00c2\u0001\u0000\u0000\u0000E\u00c6\u0001\u0000\u0000\u0000"+
		"G\u00ca\u0001\u0000\u0000\u0000IM\u0005\"\u0000\u0000JL\u0007\u0000\u0000"+
		"\u0000KJ\u0001\u0000\u0000\u0000LO\u0001\u0000\u0000\u0000MK\u0001\u0000"+
		"\u0000\u0000MN\u0001\u0000\u0000\u0000NP\u0001\u0000\u0000\u0000OM\u0001"+
		"\u0000\u0000\u0000PQ\u0005\"\u0000\u0000Q\u0002\u0001\u0000\u0000\u0000"+
		"RY\u0007\u0001\u0000\u0000SU\u0007\u0002\u0000\u0000TS\u0001\u0000\u0000"+
		"\u0000UV\u0001\u0000\u0000\u0000VT\u0001\u0000\u0000\u0000VW\u0001\u0000"+
		"\u0000\u0000WY\u0001\u0000\u0000\u0000XR\u0001\u0000\u0000\u0000XT\u0001"+
		"\u0000\u0000\u0000Y\u0004\u0001\u0000\u0000\u0000Z[\u0005_\u0000\u0000"+
		"[\u0006\u0001\u0000\u0000\u0000\\]\u0005.\u0000\u0000]\b\u0001\u0000\u0000"+
		"\u0000^_\u0005,\u0000\u0000_\n\u0001\u0000\u0000\u0000`a\u0005?\u0000"+
		"\u0000a\f\u0001\u0000\u0000\u0000bc\u0005:\u0000\u0000c\u000e\u0001\u0000"+
		"\u0000\u0000de\u0005;\u0000\u0000e\u0010\u0001\u0000\u0000\u0000fg\u0005"+
		"|\u0000\u0000g\u0012\u0001\u0000\u0000\u0000hi\u0005n\u0000\u0000ij\u0005"+
		"o\u0000\u0000jk\u0005t\u0000\u0000k\u0014\u0001\u0000\u0000\u0000lm\u0005"+
		":\u0000\u0000mn\u0005-\u0000\u0000n\u0016\u0001\u0000\u0000\u0000op\u0005"+
		"+\u0000\u0000p\u0018\u0001\u0000\u0000\u0000qr\u0005-\u0000\u0000r\u001a"+
		"\u0001\u0000\u0000\u0000st\u0005*\u0000\u0000t\u001c\u0001\u0000\u0000"+
		"\u0000uv\u0005/\u0000\u0000v\u001e\u0001\u0000\u0000\u0000wx\u0005@\u0000"+
		"\u0000x \u0001\u0000\u0000\u0000yz\u0005(\u0000\u0000z\"\u0001\u0000\u0000"+
		"\u0000{|\u0005)\u0000\u0000|$\u0001\u0000\u0000\u0000}~\u0005[\u0000\u0000"+
		"~&\u0001\u0000\u0000\u0000\u007f\u0080\u0005]\u0000\u0000\u0080(\u0001"+
		"\u0000\u0000\u0000\u0081\u0082\u0005{\u0000\u0000\u0082*\u0001\u0000\u0000"+
		"\u0000\u0083\u0084\u0005}\u0000\u0000\u0084,\u0001\u0000\u0000\u0000\u0085"+
		"\u0086\u0005=\u0000\u0000\u0086.\u0001\u0000\u0000\u0000\u0087\u0088\u0005"+
		"<\u0000\u0000\u00880\u0001\u0000\u0000\u0000\u0089\u008a\u0005>\u0000"+
		"\u0000\u008a2\u0001\u0000\u0000\u0000\u008b\u008c\u0005<\u0000\u0000\u008c"+
		"\u008d\u0005=\u0000\u0000\u008d4\u0001\u0000\u0000\u0000\u008e\u008f\u0005"+
		">\u0000\u0000\u008f\u0090\u0005=\u0000\u0000\u00906\u0001\u0000\u0000"+
		"\u0000\u0091\u0092\u0005\\\u0000\u0000\u0092\u0093\u0005=\u0000\u0000"+
		"\u00938\u0001\u0000\u0000\u0000\u0094\u0098\u0007\u0003\u0000\u0000\u0095"+
		"\u0097\u0007\u0004\u0000\u0000\u0096\u0095\u0001\u0000\u0000\u0000\u0097"+
		"\u009a\u0001\u0000\u0000\u0000\u0098\u0096\u0001\u0000\u0000\u0000\u0098"+
		"\u0099\u0001\u0000\u0000\u0000\u0099:\u0001\u0000\u0000\u0000\u009a\u0098"+
		"\u0001\u0000\u0000\u0000\u009b\u009f\u0007\u0005\u0000\u0000\u009c\u009e"+
		"\u0007\u0004\u0000\u0000\u009d\u009c\u0001\u0000\u0000\u0000\u009e\u00a1"+
		"\u0001\u0000\u0000\u0000\u009f\u009d\u0001\u0000\u0000\u0000\u009f\u00a0"+
		"\u0001\u0000\u0000\u0000\u00a0<\u0001\u0000\u0000\u0000\u00a1\u009f\u0001"+
		"\u0000\u0000\u0000\u00a2\u00a6\u0005%\u0000\u0000\u00a3\u00a5\b\u0006"+
		"\u0000\u0000\u00a4\u00a3\u0001\u0000\u0000\u0000\u00a5\u00a8\u0001\u0000"+
		"\u0000\u0000\u00a6\u00a4\u0001\u0000\u0000\u0000\u00a6\u00a7\u0001\u0000"+
		"\u0000\u0000\u00a7\u00a9\u0001\u0000\u0000\u0000\u00a8\u00a6\u0001\u0000"+
		"\u0000\u0000\u00a9\u00aa\u0007\u0006\u0000\u0000\u00aa\u00ab\u0001\u0000"+
		"\u0000\u0000\u00ab\u00ac\u0006\u001e\u0000\u0000\u00ac>\u0001\u0000\u0000"+
		"\u0000\u00ad\u00ae\u0005%\u0000\u0000\u00ae\u00af\u0005*\u0000\u0000\u00af"+
		"\u00b3\u0001\u0000\u0000\u0000\u00b0\u00b2\t\u0000\u0000\u0000\u00b1\u00b0"+
		"\u0001\u0000\u0000\u0000\u00b2\u00b5\u0001\u0000\u0000\u0000\u00b3\u00b4"+
		"\u0001\u0000\u0000\u0000\u00b3\u00b1\u0001\u0000\u0000\u0000\u00b4\u00b6"+
		"\u0001\u0000\u0000\u0000\u00b5\u00b3\u0001\u0000\u0000\u0000\u00b6\u00b7"+
		"\u0005*\u0000\u0000\u00b7\u00b8\u0005%\u0000\u0000\u00b8\u00b9\u0001\u0000"+
		"\u0000\u0000\u00b9\u00ba\u0006\u001f\u0000\u0000\u00ba@\u0001\u0000\u0000"+
		"\u0000\u00bb\u00bd\u0007\u0007\u0000\u0000\u00bc\u00bb\u0001\u0000\u0000"+
		"\u0000\u00bd\u00be\u0001\u0000\u0000\u0000\u00be\u00bc\u0001\u0000\u0000"+
		"\u0000\u00be\u00bf\u0001\u0000\u0000\u0000\u00bf\u00c0\u0001\u0000\u0000"+
		"\u0000\u00c0\u00c1\u0006 \u0000\u0000\u00c1B\u0001\u0000\u0000\u0000\u00c2"+
		"\u00c3\u0007\u0006\u0000\u0000\u00c3\u00c4\u0001\u0000\u0000\u0000\u00c4"+
		"\u00c5\u0006!\u0000\u0000\u00c5D\u0001\u0000\u0000\u0000\u00c6\u00c7\u0005"+
		"\t\u0000\u0000\u00c7\u00c8\u0001\u0000\u0000\u0000\u00c8\u00c9\u0006\""+
		"\u0000\u0000\u00c9F\u0001\u0000\u0000\u0000\u00ca\u00cb\u0005 \u0000\u0000"+
		"\u00cb\u00cc\u0001\u0000\u0000\u0000\u00cc\u00cd\u0006#\u0000\u0000\u00cd"+
		"H\u0001\u0000\u0000\u0000\n\u0000KMVX\u0098\u009f\u00a6\u00b3\u00be\u0001"+
		"\u0006\u0000\u0000";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}