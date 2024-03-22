using Antlr4.Runtime;
using asp_interpreter_lib.ANTLR;

var program = @"node(a).
                node(b).
                node(c).
                edge(a, b).
                edge(b, c).";

var inputStream = new AntlrInputStream(program);
var lexer = new ASPLexer(inputStream);
var commonTokenStream = new CommonTokenStream(lexer);
var parser = new ASPParser(commonTokenStream);

var context = parser.program();
var visitor = new BasicAspVisitor();
visitor.Visit(context);