using Asp_interpreter_lib.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asp_interpreter_lib.Util.ErrorHandling;
using Asp_interpreter_lib.Preprocessing;
using Asp_interpreter_lib.Preprocessing.DualRules;

namespace Asp_interpreter_test.DualRules
{
    internal class ToDisjunctionTest
    {
        private readonly PrefixOptions _prefixes = AspExtensions.CommonPrefixes;

        private readonly ILogger _logger = new TestingLogger(LogLevel.Error);

        [Test]
        public void ToDisjunctionHandlesTwoGoals()
        {
            string code = """
                      a(X, Y) :- c(X), not b(X, Y).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X, Y) :- not c(X).");
                Assert.That(dual[1].ToString() == "not a(X, Y) :- c(X), b(X, Y).");
            });
        }

        [Test]
        public void ToDisjunctionHandlesLiteralsAndBinaryOperations()
        {
            string code = """
                      a(X, Y) :- not b(X), Y = 4.
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 2);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X, Y) :- b(X).");
                Assert.That(dual[1].ToString() == "not a(X, Y) :- not b(X), Y \\= 4.");
            });
        }

        [Test]
        public void ToDisjunctionDoesNotAlterClassicalNegation()
        {
            string code = """
                      a(X, Y) :- not -b(X), Y = 4, -c(Y), d(X, Y).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 4);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X, Y) :- -b(X).");
                Assert.That(dual[1].ToString() == "not a(X, Y) :- not -b(X), Y \\= 4.");
                Assert.That(dual[2].ToString() == "not a(X, Y) :- not -b(X), Y = 4, not -c(Y).");
                Assert.That(dual[3].ToString() == "not a(X, Y) :- not -b(X), Y = 4, -c(Y), not d(X, Y).");
            });
        }

        [Test]
        public void ToDisjunctionIgnoresEmptyHeads()
        {
            string code = """
                      :- not b(X), c(Y), not d(X, Y).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 1);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == ":- not b(X), c(Y), not d(X, Y).");
            });
        }

        [Test]
        public void ToDisjunctionAppliesForallOnPositiveLiteral()
        {
            string code = """
                      a(X) :- not b(X), d(X, Y).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 3);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X) :- forall(Y, not fa_a(X, Y)).");
                Assert.That(dual[1].ToString() == "not fa_a(X, Y) :- b(X).");
                Assert.That(dual[2].ToString() == "not fa_a(X, Y) :- not b(X), not d(X, Y).");
            });
        }

        [Test]
        public void ToDisjunctionAppliesForallOnNegativeLiteral()
        {
            string code = """
                      a(X) :- not b(X), not d(X, Y).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 3);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X) :- forall(Y, not fa_a(X, Y)).");
                Assert.That(dual[1].ToString() == "not fa_a(X, Y) :- b(X).");
                Assert.That(dual[2].ToString() == "not fa_a(X, Y) :- not b(X), d(X, Y).");
            });
        }

        [Test]
        public void ToDisjunctionAppliesForallOnBinaryOperation()
        {
            string code = """
                      a(X) :- not b(X), Y = 4.
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 3);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X) :- forall(Y, not fa_a(X, Y)).");
                Assert.That(dual[1].ToString() == "not fa_a(X, Y) :- b(X).");
                Assert.That(dual[2].ToString() == "not fa_a(X, Y) :- not b(X), Y \\= 4.");
            });
        }
        [Test]
        public void ToDisjunctionWithBodyVariablesDoesNotAlterClassicalNegation()
        {
            string code = """
                      a(X) :- not b(X), -c(Y), not -d(X, Y).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 4);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X) :- forall(Y, not fa_a(X, Y)).");
                Assert.That(dual[1].ToString() == "not fa_a(X, Y) :- b(X).");
                Assert.That(dual[2].ToString() == "not fa_a(X, Y) :- not b(X), not -c(Y).");
                Assert.That(dual[3].ToString() == "not fa_a(X, Y) :- not b(X), -c(Y), -d(X, Y).");
            });
        }

        [Test]
        public void ToDisjunctionHandlesMultipleBodyVariables()
        {
            string code = """
                      a(X) :- not b(X), Y = 4, c(X, Y, Z).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var dual = dualRuleConverter.ToDisjunction(program.Statements[0]).ToList();

            Assert.That(dual.Count == 4);
            Assert.Multiple(() =>
            {
                Assert.That(dual[0].ToString() == "not a(X) :- forall(Y, forall(Z, not fa_a(X, Y, Z))).");
                Assert.That(dual[1].ToString() == "not fa_a(X, Y, Z) :- b(X).");
                Assert.That(dual[2].ToString() == "not fa_a(X, Y, Z) :- not b(X), Y \\= 4.");
                Assert.That(dual[3].ToString() == "not fa_a(X, Y, Z) :- not b(X), Y = 4, not c(X, Y, Z).");
            });
        }
    }
}
