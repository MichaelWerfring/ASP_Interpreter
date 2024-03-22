using Antlr4.Runtime;
using asp_interpreter_lib.ANTLR;
using asp_interpreter_lib.FileIO;

if(args.Length != 1)
{
    throw new ArgumentException("Please provide a source file!");
}

var result = FileReader.ReadFile(args[0]);

if (!result.Success)
{
    throw new ArgumentException(result.Message);
}

var inputStream = new AntlrInputStream(result.Content);
var lexer = new ASPLexer(inputStream);
var commonTokenStream = new CommonTokenStream(lexer);
var parser = new ASPParser(commonTokenStream);
//for handling errors: parser.AddErrorListener();
var context = parser.program();
var visitor = new BasicAspVisitor();
visitor.Visit(context);