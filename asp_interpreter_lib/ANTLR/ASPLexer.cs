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

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class ASPLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		STRING=1, NUMBER=2, ANONYMOUS_VARIABLE=3, DOT=4, COMMA=5, QUERY_SYMBOL=6, 
		COLON=7, SEMICOLON=8, OR=9, NAF=10, CONS=11, PLUS=12, MINUS=13, TIMES=14, 
		POW=15, DIV=16, AT=17, PAREN_OPEN=18, PAREN_CLOSE=19, SQUARE_OPEN=20, 
		SQUARE_CLOSE=21, CURLY_OPEN=22, CURLY_CLOSE=23, EQUAL=24, LESS=25, GREATER=26, 
		LESS_OR_EQ=27, GREATER_OR_EQ=28, DISUNIFICATION=29, IS=30, ID=31, VARIABLE=32, 
		COMMENT=33, MULTI_LINE_COMMENT=34, BLANK=35, NEWLINE=36, TAB=37, WS=38;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"STRING", "NUMBER", "ANONYMOUS_VARIABLE", "DOT", "COMMA", "QUERY_SYMBOL", 
		"COLON", "SEMICOLON", "OR", "NAF", "CONS", "PLUS", "MINUS", "TIMES", "POW", 
		"DIV", "AT", "PAREN_OPEN", "PAREN_CLOSE", "SQUARE_OPEN", "SQUARE_CLOSE", 
		"CURLY_OPEN", "CURLY_CLOSE", "EQUAL", "LESS", "GREATER", "LESS_OR_EQ", 
		"GREATER_OR_EQ", "DISUNIFICATION", "IS", "ID", "VARIABLE", "COMMENT", 
		"MULTI_LINE_COMMENT", "BLANK", "NEWLINE", "TAB", "WS"
	};


	public ASPLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ASPLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, "'_'", "'.'", "','", "'?-'", "':'", "';'", "'|'", "'not'", 
		"':-'", "'+'", "'-'", "'*'", "'**'", "'/'", "'@'", "'('", "')'", "'['", 
		"']'", "'{'", "'}'", "'='", "'<'", "'>'", "'<='", "'>='", "'\\='", "'is'", 
		null, null, null, null, null, null, "'\\t'", "' '"
	};
	private static readonly string[] _SymbolicNames = {
		null, "STRING", "NUMBER", "ANONYMOUS_VARIABLE", "DOT", "COMMA", "QUERY_SYMBOL", 
		"COLON", "SEMICOLON", "OR", "NAF", "CONS", "PLUS", "MINUS", "TIMES", "POW", 
		"DIV", "AT", "PAREN_OPEN", "PAREN_CLOSE", "SQUARE_OPEN", "SQUARE_CLOSE", 
		"CURLY_OPEN", "CURLY_CLOSE", "EQUAL", "LESS", "GREATER", "LESS_OR_EQ", 
		"GREATER_OR_EQ", "DISUNIFICATION", "IS", "ID", "VARIABLE", "COMMENT", 
		"MULTI_LINE_COMMENT", "BLANK", "NEWLINE", "TAB", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "ASP.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static ASPLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,38,218,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,2,37,7,37,1,0,1,0,4,0,80,8,0,11,0,12,0,81,1,0,1,0,1,1,1,
		1,1,1,5,1,89,8,1,10,1,12,1,92,9,1,3,1,94,8,1,1,2,1,2,1,3,1,3,1,4,1,4,1,
		5,1,5,1,5,1,6,1,6,1,7,1,7,1,8,1,8,1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,11,
		1,11,1,12,1,12,1,13,1,13,1,14,1,14,1,14,1,15,1,15,1,16,1,16,1,17,1,17,
		1,18,1,18,1,19,1,19,1,20,1,20,1,21,1,21,1,22,1,22,1,23,1,23,1,24,1,24,
		1,25,1,25,1,26,1,26,1,26,1,27,1,27,1,27,1,28,1,28,1,28,1,29,1,29,1,29,
		1,30,1,30,5,30,163,8,30,10,30,12,30,166,9,30,1,31,1,31,5,31,170,8,31,10,
		31,12,31,173,9,31,1,32,1,32,5,32,177,8,32,10,32,12,32,180,9,32,1,32,1,
		32,1,32,1,32,1,33,1,33,1,33,1,33,5,33,190,8,33,10,33,12,33,193,9,33,1,
		33,1,33,1,33,1,33,1,33,1,34,4,34,201,8,34,11,34,12,34,202,1,34,1,34,1,
		35,1,35,1,35,1,35,1,36,1,36,1,36,1,36,1,37,1,37,1,37,1,37,1,191,0,38,1,
		1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,
		15,31,16,33,17,35,18,37,19,39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,
		27,55,28,57,29,59,30,61,31,63,32,65,33,67,34,69,35,71,36,73,37,75,38,1,
		0,9,2,0,34,34,92,92,1,0,48,48,1,0,49,57,1,0,48,57,1,0,97,122,4,0,48,57,
		65,90,95,95,97,122,1,0,65,90,3,0,10,10,13,13,63,63,3,0,9,10,13,13,32,32,
		225,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,
		0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,
		0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,
		1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,
		0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,
		1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,
		0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,1,0,0,0,0,75,1,0,0,0,1,77,
		1,0,0,0,3,93,1,0,0,0,5,95,1,0,0,0,7,97,1,0,0,0,9,99,1,0,0,0,11,101,1,0,
		0,0,13,104,1,0,0,0,15,106,1,0,0,0,17,108,1,0,0,0,19,110,1,0,0,0,21,114,
		1,0,0,0,23,117,1,0,0,0,25,119,1,0,0,0,27,121,1,0,0,0,29,123,1,0,0,0,31,
		126,1,0,0,0,33,128,1,0,0,0,35,130,1,0,0,0,37,132,1,0,0,0,39,134,1,0,0,
		0,41,136,1,0,0,0,43,138,1,0,0,0,45,140,1,0,0,0,47,142,1,0,0,0,49,144,1,
		0,0,0,51,146,1,0,0,0,53,148,1,0,0,0,55,151,1,0,0,0,57,154,1,0,0,0,59,157,
		1,0,0,0,61,160,1,0,0,0,63,167,1,0,0,0,65,174,1,0,0,0,67,185,1,0,0,0,69,
		200,1,0,0,0,71,206,1,0,0,0,73,210,1,0,0,0,75,214,1,0,0,0,77,79,5,34,0,
		0,78,80,8,0,0,0,79,78,1,0,0,0,80,81,1,0,0,0,81,79,1,0,0,0,81,82,1,0,0,
		0,82,83,1,0,0,0,83,84,5,34,0,0,84,2,1,0,0,0,85,94,7,1,0,0,86,90,7,2,0,
		0,87,89,7,3,0,0,88,87,1,0,0,0,89,92,1,0,0,0,90,88,1,0,0,0,90,91,1,0,0,
		0,91,94,1,0,0,0,92,90,1,0,0,0,93,85,1,0,0,0,93,86,1,0,0,0,94,4,1,0,0,0,
		95,96,5,95,0,0,96,6,1,0,0,0,97,98,5,46,0,0,98,8,1,0,0,0,99,100,5,44,0,
		0,100,10,1,0,0,0,101,102,5,63,0,0,102,103,5,45,0,0,103,12,1,0,0,0,104,
		105,5,58,0,0,105,14,1,0,0,0,106,107,5,59,0,0,107,16,1,0,0,0,108,109,5,
		124,0,0,109,18,1,0,0,0,110,111,5,110,0,0,111,112,5,111,0,0,112,113,5,116,
		0,0,113,20,1,0,0,0,114,115,5,58,0,0,115,116,5,45,0,0,116,22,1,0,0,0,117,
		118,5,43,0,0,118,24,1,0,0,0,119,120,5,45,0,0,120,26,1,0,0,0,121,122,5,
		42,0,0,122,28,1,0,0,0,123,124,5,42,0,0,124,125,5,42,0,0,125,30,1,0,0,0,
		126,127,5,47,0,0,127,32,1,0,0,0,128,129,5,64,0,0,129,34,1,0,0,0,130,131,
		5,40,0,0,131,36,1,0,0,0,132,133,5,41,0,0,133,38,1,0,0,0,134,135,5,91,0,
		0,135,40,1,0,0,0,136,137,5,93,0,0,137,42,1,0,0,0,138,139,5,123,0,0,139,
		44,1,0,0,0,140,141,5,125,0,0,141,46,1,0,0,0,142,143,5,61,0,0,143,48,1,
		0,0,0,144,145,5,60,0,0,145,50,1,0,0,0,146,147,5,62,0,0,147,52,1,0,0,0,
		148,149,5,60,0,0,149,150,5,61,0,0,150,54,1,0,0,0,151,152,5,62,0,0,152,
		153,5,61,0,0,153,56,1,0,0,0,154,155,5,92,0,0,155,156,5,61,0,0,156,58,1,
		0,0,0,157,158,5,105,0,0,158,159,5,115,0,0,159,60,1,0,0,0,160,164,7,4,0,
		0,161,163,7,5,0,0,162,161,1,0,0,0,163,166,1,0,0,0,164,162,1,0,0,0,164,
		165,1,0,0,0,165,62,1,0,0,0,166,164,1,0,0,0,167,171,7,6,0,0,168,170,7,5,
		0,0,169,168,1,0,0,0,170,173,1,0,0,0,171,169,1,0,0,0,171,172,1,0,0,0,172,
		64,1,0,0,0,173,171,1,0,0,0,174,178,5,37,0,0,175,177,8,7,0,0,176,175,1,
		0,0,0,177,180,1,0,0,0,178,176,1,0,0,0,178,179,1,0,0,0,179,181,1,0,0,0,
		180,178,1,0,0,0,181,182,7,7,0,0,182,183,1,0,0,0,183,184,6,32,0,0,184,66,
		1,0,0,0,185,186,5,37,0,0,186,187,5,42,0,0,187,191,1,0,0,0,188,190,9,0,
		0,0,189,188,1,0,0,0,190,193,1,0,0,0,191,192,1,0,0,0,191,189,1,0,0,0,192,
		194,1,0,0,0,193,191,1,0,0,0,194,195,5,42,0,0,195,196,5,37,0,0,196,197,
		1,0,0,0,197,198,6,33,0,0,198,68,1,0,0,0,199,201,7,8,0,0,200,199,1,0,0,
		0,201,202,1,0,0,0,202,200,1,0,0,0,202,203,1,0,0,0,203,204,1,0,0,0,204,
		205,6,34,0,0,205,70,1,0,0,0,206,207,7,7,0,0,207,208,1,0,0,0,208,209,6,
		35,0,0,209,72,1,0,0,0,210,211,5,9,0,0,211,212,1,0,0,0,212,213,6,36,0,0,
		213,74,1,0,0,0,214,215,5,32,0,0,215,216,1,0,0,0,216,217,6,37,0,0,217,76,
		1,0,0,0,9,0,81,90,93,164,171,178,191,202,1,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
