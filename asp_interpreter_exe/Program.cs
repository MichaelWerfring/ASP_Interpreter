using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Antlr4.Runtime;
using asp_interpreter_lib;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.OLONDetection.CallGraph;
using asp_interpreter_lib.OLONDetection;
using asp_interpreter_lib.InternalProgramClasses.InternalProgram;
using asp_interpreter_lib.SLDSolverClasses.SLDNFSolver;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types.TypeVisitors;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using Antlr4.Runtime.Misc;
using asp_interpreter_exe;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<IErrorLogger, ConsoleErrorLogger>();
builder.Services.AddTransient<ProgramVisitor>();
builder.Services.AddSingleton<PrefixOptions>(new PrefixOptions(
    "rwh_", "fa_", "eh", "chk_", "not_", "V"));
using var host = builder.Build();

ConsoleApplication app = new ConsoleApplication(
    null ,null);
app.Run(args);