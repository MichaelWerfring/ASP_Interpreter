using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Antlr4.Runtime;
using asp_interpreter_lib.ErrorHandling;
using asp_interpreter_lib.FileIO;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;
using System.IO;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<IErrorLogger, ConsoleErrorLogger>();
builder.Services.AddTransient<ProgramVisitor>();
using var host = builder.Build();


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
var context = parser.program();
var visitor = host.Services.GetRequiredService<ProgramVisitor>();
var program = visitor.VisitProgram(context);
Console.ReadLine();