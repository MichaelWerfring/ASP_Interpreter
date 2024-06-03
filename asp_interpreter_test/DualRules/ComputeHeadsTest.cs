//-----------------------------------------------------------------------
// <copyright file="ComputeHeadsTest.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_test.DualRules
{
    using Asp_interpreter_lib.Preprocessing;
    using Asp_interpreter_lib.Preprocessing.DualRules;
    using Asp_interpreter_lib.Util;
    using Asp_interpreter_lib.Util.ErrorHandling;
    using NUnit.Framework;

    internal class ComputeHeadsTest
    {
        private readonly PrefixOptions prefixes = AspExtensions.CommonPrefixes;

        private readonly ILogger logger = new TestingLogger(LogLevels.Error);

        [Test]
        public void ComputeHeadHandlesMultipleVariables()
        {
            string code = """
                      a(X, Y, X, X) :- c(X), b(Y).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(X, Y, V1, V2) :- V1 = X, V2 = X, c(X), b(Y).");
        }

        [Test]
        public void ComputeHeadHandlesIntegerValues()
        {
            string code = """
                      a(X, Y, 12) :- c(X), b(Y).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

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

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

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

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

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

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "a(V1) :- V1 = b(c(d)).");
        }

        [Test]
        public void ComputeHeadHandlesHandlesParenthesizedTerm()
        {
            string code = """
                      a(((4))).
                      a?
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

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

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

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

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "p(V1) :- V1 = [X, Y, Z], q(X), r(Y), s(Z).");
        }

        [Test]
        public void ComputeHeadPutsAtomsInCorrectOrder()
        {
            string code = """
                      b(1, 2).
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);

            Assert.That(statement.ToString() == "b(V1, V2) :- V1 = 1, V2 = 2.");
        }

        [Test]
        public void ComputeHeadPutsAtomsAndVariablesInCorrectOrder()
        {
            string code = """
                      b(1, X, 3) :- c(X).
                      c(3)
                      p?
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

            var statement = dualRuleConverter.ComputeHead(program.Statements[0]);
            Assert.That(statement.ToString(), Is.EqualTo("b(V1, X, V2) :- V1 = 1, V2 = 3, c(X)."));
        }
    }
}