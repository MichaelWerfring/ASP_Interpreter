using Antlr4.Runtime;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Visitors;

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
var visitor = new ProgramVisitor();
var program = visitor.VisitProgram(context);
Console.ReadLine();