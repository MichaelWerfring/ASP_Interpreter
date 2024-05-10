using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace asp_interpreter_test.DualRules
{
    internal class ComputeHeadsTest
    {
        private readonly PrefixOptions _prefixes = AspExtensions.CommonPrefixes;

        private readonly ILogger _logger = new TestingLogger(LogLevel.Error);

        [Test]
        public void ComputeHeadHandlesMultipleVariables()
        {
            string code = """
                      a(X, Y, X, X) :- c(X), b(Y).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(X, Y, V1, V2) :- V2 = X, V1 = X, c(X), b(Y).");
        }

        [Test]
        public void ComputeHeadHandlesIntegerValues()
        {
            string code = """
                      a(X, Y, 12) :- c(X), b(Y).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(X, Y, V1) :- V1 = 12, c(X), b(Y).");
        }

        [Test]
        public void ComputeHeadHandlesAtom()
        {
            string code = """
                      a(b).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(V1) :- V1 = b.");
        }

        [Test]
        public void ComputeHeadHandlesHandlesNegatedTerm()
        {
            string code = """
                      a(-4).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(V1) :- V1 = -4.");
        }

        [Test]
        public void ComputeHeadHandlesHandlesCompoundTerm()
        {
            string code = """
                      a(b(c(d))).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(V1) :- V1 = b(c(d)).");
        }

        [Test]
        public void ComputeHeadHandlesHandlesParenthesisedTerm()
        {
            string code = """
                      a(((4))).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(V1) :- V1 = 4.");
        }

        [Test]
        public void ComputeHeadHandlesRecursiveLists()
        {
            string code = """
                      p([X|T]) :- q(X), p(T).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "p(V1) :- V1 = [X| T], q(X), p(T).");
        }

        [Test]
        public void ComputeHeadHandlesConventionalLists()
        {
            string code = """
                      p([X,Y,Z]) :- q(X), r(Y), s(Z).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "p(V1) :- V1 = [X, Y, Z], q(X), r(Y), s(Z).");
        }
    }
}
