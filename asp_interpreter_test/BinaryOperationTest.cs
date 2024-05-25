namespace Asp_interpreter_test
{
    using Antlr4.Runtime;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using Asp_interpreter_lib.Visitors;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class BinaryOperationTest
    {
        [TestCase("a :- 1 + 1 is X.")]
        [TestCase("a :- -1 is a(X).")]
        [TestCase("a :- 11 is X.")]
        [TestCase("a :- X is (1 + b(1)) * 2.")]
        [TestCase("a :- X is 1 + a(X).")]
        [TestCase("a :- kmn(lelele) is 1 + 1")]
        [TestCase("a :- X is \"t\"")]
        public void IsFailsOnInvalidTerms(string code)
        {
            TestingLogger logger = new TestingLogger(LogLevel.Error);

            var inputStream = new AntlrInputStream(code);
            var lexer = new ASPLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new ASPParser(commonTokenStream);
            var context = parser.program();
            var visitor = new ProgramVisitor(logger);
            var program = visitor.VisitProgram(context);

            Assert.That(logger.Errors.Count > 0 && !program.HasValue);
        }
    }
}