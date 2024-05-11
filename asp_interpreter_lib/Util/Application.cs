using Antlr4.Runtime;
using asp_interpreter_lib.InternalProgramClasses.Database;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using asp_interpreter_lib.Preprocessing.OLONDetection;
using asp_interpreter_lib.Preprocessing.OLONDetection.CallGraph;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.Conversion;
using asp_interpreter_lib.ProgramConversion.ASPProgramToInternalProgram.FunctorTable;
using asp_interpreter_lib.SLDSolverClasses.Basic.SLDNFSolver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.Solver;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Postprocessing;
using asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver;
using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Solving.NMRCheck;
using asp_interpreter_lib.Types;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors.Copy;
using asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;
using asp_interpreter_lib.Util.ErrorHandling;
using asp_interpreter_lib.Visitors;
using QuikGraph;
using QuikGraph.Algorithms;
using System.Diagnostics;

namespace asp_interpreter_lib.Util;

public class Application(
    ILogger logger,
    ProgramVisitor programVisitor, 
    ProgramConfig config)
{
    private readonly ILogger _logger = logger;

    private readonly ProgramVisitor _programVisitor = programVisitor;

    private readonly ProgramConfig _config = config;

    private readonly PrefixOptions _prefixes = new("fa_", "eh", "chk_", "not_", "V");

    private ProgramConverter _converter = new ProgramConverter(new FunctorTableRecord(), logger);

    public void Run()
    {
        //Read
        var code = FileReader.ReadFile(_config.Path, _logger);
        if (code == null) return;

        //Program
        var program = GetProgram(code);

        //Dual
        var dualGenerator = new DualRuleConverter(_prefixes, _logger);
        var dual = dualGenerator.GetDualRules(program.Duplicate().Statements);

        //OLON
        List<Statement> olonRules = new OLONRulesFilterer(_logger).FilterOlonRules(program.Statements);

        //NMR 
        var nmrChecker = new NmrChecker(_prefixes, _logger);
        var subcheck = nmrChecker.GetSubCheckRules(olonRules);

        //Interactive if needed else just solve
        if (!_config.Interactive)
        {
            if (!program.Query.HasValue)
            {
                _logger.LogError("The program has no query given, either specify one in the file or use interactive mode.");
            }

            
            return;
        }

        //If a query is specified in the file it will be ignored in this case
        while (true)
        {
            Console.Write("?-");
            string input = Console.ReadLine();

            if (input == "exit") return;

            // 1) parse query 
            // 2) solve existing program with new query 
            // 3) show answer
        }
    }

    private void SolveAutomatic()
    {

        ////converted program
        //var convertedProgram = program.Statements.Concat(dual)
        //                                         .Concat(subcheck)
        //                                         .Select(_converter.ConvertStatement)
        //                                         .ToList();

        //// database
        //var database = new DualClauseDatabase(convertedProgram, new FunctorTableRecord());


        //var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord(), _logger);



        //foreach (var solution in solver.Solve(query))
        //{

        //}
        
    }


    private void InteractiveSolve(IDatabase database)
    {
        var solver = new CoinductiveSLDSolver(database, new FunctorTableRecord(), _logger);
    }

    private AspProgram GetProgram(string code)
    {
        var inputStream = new AntlrInputStream(code);
        var lexer = new ASPLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(lexer);
        var parser = new ASPParser(commonTokenStream);
        var context = parser.program();
        var program = _programVisitor.VisitProgram(context);

        if (!program.HasValue)
        {
            throw new ArgumentException("Failed to parse program!");
        }

        return program.GetValueOrThrow();
    } 
    

}