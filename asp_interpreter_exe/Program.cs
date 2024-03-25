using Antlr4.Runtime;
using asp_interpreter_lib.CallGraph;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;
using System.IO;

Test();



/*
if (args.Length != 1)
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
*/

void Test()
{
    var result = FileReader.ReadFile("D:\\FH\\Semester 4\\Logikprogrammierung ILV\\SASP-Projekt\\ASP_Interpreter\\asp_interpreter_exe\\program1.asp");

    var inputStream = new AntlrInputStream(result.Content);
    var lexer = new ASPLexer(inputStream);
    var commonTokenStream = new CommonTokenStream(lexer);
    var parser = new ASPParser(commonTokenStream);
    var context = parser.program();
    var visitor = new ProgramVisitor();
    var program = visitor.VisitProgram(context);

    CallGraphBuilder builder = new CallGraphBuilder();
    var graph = builder.BuildCallGraph(program);

    string graphString = graph.ToGraphviz();

    using var stream = File.Open("D:\\FH\\Semester 4\\Logikprogrammierung ILV\\SASP-Projekt\\ASP_Interpreter\\callGraphTest", FileMode.OpenOrCreate);
    using StreamWriter writer = new StreamWriter(stream);
    writer.Write(graphString);

    Console.ReadLine();
}