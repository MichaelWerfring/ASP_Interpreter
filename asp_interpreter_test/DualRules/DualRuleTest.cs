﻿//-----------------------------------------------------------------------
// <copyright file="DualRuleTest.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_test.DualRules;
using Asp_interpreter_lib.Preprocessing;
using Asp_interpreter_lib.Preprocessing.DualRules;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;
using NUnit.Framework;
using System.Linq;

public class DualRuleTest
{
    private readonly PrefixOptions prefixes = AspExtensions.CommonPrefixes;

    private readonly ILogger logger = new TestingLogger(LogLevels.Error);

    [Test]
    public void ForallSkipsEmptyHeads()
    {
        string code = """
                      :- not b(X), Y = 4, c(X, Y, Z).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var dual = dualRuleConverter.AddForall(program.Statements[0]).ToList();

        Assert.That(dual.Count == 1);
        Assert.Multiple(() => { Assert.That(dual[0].ToString() == ":- not b(X), Y = 4, c(X, Y, Z)."); });
    }

    [Test]
    public void ForallSkipsFacts()
    {
        string code = """
                      b(X).
                      ?- b(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var dual = dualRuleConverter.AddForall(program.Statements[0]).ToList();

        Assert.Multiple(() =>
        {
            Assert.That(dual.Count == 1);
            Assert.That(dual[0].ToString() == "b(X).");
        });
    }

    [Test]
    public void TestComplexConversion()
    {
        string code = """
                      p(0).
                      p(X) :- q(X), not t(X, Y).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 7);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1) :- not p1(V1), not p2(V1)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(V1) :- V1 \\= 0."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not p2(X) :- forall(Y, not fa_p2(X, Y))."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not fa_p2(X, Y) :- not q(X)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not fa_p2(X, Y) :- q(X), t(X, Y)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not q(X)."));
            Assert.That(duals[6].ToString(), Is.EqualTo("not t(X, Y)."));
        });
    }

    [Test]
    public void ComplexConversionDoesNotAlterClassicalNegation()
    {
        string code = """
                      p(0).
                      p(X) :- -q(X), not -t(X, Y).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 7);
            Assert.That(duals[0].ToString() == "not p(V1) :- not p1(V1), not p2(V1).");
            Assert.That(duals[1].ToString() == "not p1(V1) :- V1 \\= 0.");
            Assert.That(duals[2].ToString() == "not p2(X) :- forall(Y, not fa_p2(X, Y)).");
            Assert.That(duals[3].ToString() == "not fa_p2(X, Y) :- not -q(X).");
            Assert.That(duals[4].ToString() == "not fa_p2(X, Y) :- -q(X), -t(X, Y).");
            Assert.That(duals[5].ToString() == "not -q(X).");
            Assert.That(duals[6].ToString() == "not -t(X, Y).");
        });
    }

    [Test]
    public void GeneratesDualsForClassicalNegationSeparately()
    {
        string code = """
                      p(X) :- not q(X), r(X).
                      -p(X) :- s(X), not t(X).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 10);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1) :- not p1(V1)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(X) :- q(X)."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not p1(X) :- not q(X), not r(X)."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not -p(V1) :- not -p1(V1)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not -p1(X) :- not s(X)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not -p1(X) :- s(X), t(X)."));
            Assert.That(duals[6].ToString(), Is.EqualTo("not q(X)."));
            Assert.That(duals[7].ToString(), Is.EqualTo("not r(X)."));
            Assert.That(duals[8].ToString(), Is.EqualTo("not s(X)."));
            Assert.That(duals[9].ToString(), Is.EqualTo("not t(X)."));
        });
    }

    [Test]
    public void TreatsRecursiveListInHeadWithForall()
    {
        string code = """
                      p([X|T]) :- q(X), p(T).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Checked with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count, Is.EqualTo(6));
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1) :- not p1(V1)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(V1) :- forall(X, forall(T, not fa_p1(V1, X, T)))."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not fa_p1(V1, X, T) :- V1 \\= [X| T]."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not fa_p1(V1, X, T) :- V1 = [X| T], not q(X)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not fa_p1(V1, X, T) :- V1 = [X| T], q(X), not p(T)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not q(X)."));
        });
    }

    [Test]
    public void TreatsConventionalListInHeadWithForall()
    {
        string code = """
                      p([X, Y, Z]) :- q(X), r(Y), s(Z).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);

        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);
        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count, Is.EqualTo(9));
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1) :- not p1(V1)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(V1) :- forall(X, forall(Y, forall(Z, not fa_p1(V1, X, Y, Z))))."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not fa_p1(V1, X, Y, Z) :- V1 \\= [X, Y, Z]."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not fa_p1(V1, X, Y, Z) :- V1 = [X, Y, Z], not q(X)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not fa_p1(V1, X, Y, Z) :- V1 = [X, Y, Z], q(X), not r(Y)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not fa_p1(V1, X, Y, Z) :- V1 = [X, Y, Z], q(X), r(Y), not s(Z)."));
            Assert.That(duals[6].ToString(), Is.EqualTo("not q(X)."));
            Assert.That(duals[7].ToString(), Is.EqualTo("not r(Y)."));
            Assert.That(duals[8].ToString(), Is.EqualTo("not s(Z)."));
        });
    }

    [Test]
    public void DualRuleConversionHandlesAtoms()
    {
        string code = """
                      -p.
                      p :- -p.
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Tested with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 3);
            Assert.That(duals[0].ToString(), Is.EqualTo("not -p :- not -p1."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p :- not p1."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not p1 :- not -p."));
        });
    }

    [Test]
    public void DualRuleConverterPutsGoalsOnlyInBodyIntoFacts()
    {
        string code = """
                      p :- s.
                      p :- not q.
                      q :- not p.
                      r :- p.
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 8);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p :- not p1, not p2."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1 :- not s."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not p2 :- q."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not q :- not q1."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not q1 :- p."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not r :- not r1."));
            Assert.That(duals[6].ToString(), Is.EqualTo("not r1 :- not p."));
            Assert.That(duals[7].ToString(), Is.EqualTo("not s."));
        });
    }

    [Test]
    public void ConversionRemovesAnonymousVariablesInBinaryOperation()
    {
        string code = """
                      p(X) :- _ > X.
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 3);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1) :- not p1(V1)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(X) :- forall(V0_, not fa_p1(X, V0_))."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not fa_p1(X, V0_) :- V0_ <= X."));
        });
    }

    [Test]
    public void ConversionRemovesAnonymousVariablesInLiteral()
    {
        string code = """
                      p(X) :- q(_), s(X).
                      q(3).
                      s(4).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 8);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1) :- not p1(V1)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(X) :- forall(V0_, not fa_p1(X, V0_))."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not fa_p1(X, V0_) :- not q(V0_)."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not fa_p1(X, V0_) :- q(V0_), not s(X)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not q(V1) :- not q1(V1)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not q1(V1) :- V1 \\= 3."));
            Assert.That(duals[6].ToString(), Is.EqualTo("not s(V1) :- not s1(V1)."));
            Assert.That(duals[7].ToString(), Is.EqualTo("not s1(V1) :- V1 \\= 4."));
        });
    }

    [Test]
    public void ConversionRemovesAnonymousVariablesInHead()
    {
        string code = """
                      p(X, _) :- q(X).
                      q(3).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 4);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1, V2) :- not p1(V1, V2)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(X, V0_) :- not q(X)."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not q(V1) :- not q1(V1)."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not q1(V1) :- V1 \\= 3."));
        });
    }

    [Test]
    public void ConversionRemovesAnonymousVariablesInRecursiveListHead()
    {
        string code = """
                      p(X, [X|_]) :- q(X).
                      q(3).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 6);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1, V2) :- not p1(V1, V2)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(X, V1) :- forall(V0_, not fa_p1(X, V1, V0_))."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not fa_p1(X, V1, V0_) :- V1 \\= [X| V0_]."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not fa_p1(X, V1, V0_) :- V1 = [X| V0_], not q(X)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not q(V1) :- not q1(V1)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not q1(V1) :- V1 \\= 3."));
        });
    }

    [Test]
    public void ConversionRemovesAnonymousVariablesInConventionalListHead()
    {
        string code = """
                      p(X, [X, _]) :- q(X).
                      q(3).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 6);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1, V2) :- not p1(V1, V2)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(X, V1) :- forall(V0_, not fa_p1(X, V1, V0_))."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not fa_p1(X, V1, V0_) :- V1 \\= [X, V0_]."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not fa_p1(X, V1, V0_) :- V1 = [X, V0_], not q(X)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not q(V1) :- not q1(V1)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not q1(V1) :- V1 \\= 3."));
        });
    }

    [Test]
    public void ConversionHandlesMultipleAnonymousVariablesInBody()
    {
        string code = """
                      p(X) :- q(X), s(_, _), t(_).
                      s(1, 2).
                      t(4).
                      q(3).
                      ?- p(X).
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 12);
            Assert.That(duals[0].ToString(), Is.EqualTo("not p(V1) :- not p1(V1)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not p1(X) :- forall(V0_, forall(V1_, forall(V2_, not fa_p1(X, V0_, V1_, V2_))))."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not fa_p1(X, V0_, V1_, V2_) :- not q(X)."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not fa_p1(X, V0_, V1_, V2_) :- q(X), not s(V0_, V1_)."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not fa_p1(X, V0_, V1_, V2_) :- q(X), s(V0_, V1_), not t(V2_)."));
            Assert.That(duals[5].ToString(), Is.EqualTo("not s(V1, V2) :- not s1(V1, V2)."));
            Assert.That(duals[6].ToString(), Is.EqualTo("not s1(V1, V2) :- V1 \\= 1."));
            Assert.That(duals[7].ToString(), Is.EqualTo("not s1(V1, V2) :- V1 = 1, V2 \\= 2."));
            Assert.That(duals[8].ToString(), Is.EqualTo("not t(V1) :- not t1(V1)."));
            Assert.That(duals[9].ToString(), Is.EqualTo("not t1(V1) :- V1 \\= 4."));
            Assert.That(duals[10].ToString(), Is.EqualTo("not q(V1) :- not q1(V1)."));
            Assert.That(duals[11].ToString(), Is.EqualTo("not q1(V1) :- V1 \\= 3."));
        });
    }

    [Test]
    public void DualConverterAddsWrapperOnEveryGoal()
    {
        string code = """
                      a :- b.
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 3);
            Assert.That(duals[0].ToString() == "not a :- not a1.");
            Assert.That(duals[1].ToString() == "not a1 :- not b.");
            Assert.That(duals[2].ToString() == "not b.");
        });
    }

    [Test]
    public void DualConverterAddsWrapperForAtoms()
    {
        string code = """
                      a.
                      a.
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 1);
            Assert.That(duals[0].ToString() == "not a :- not a1, not a2.");
        });
    }

    [Test]
    public void DualConverterAddsWrapperForAtomsAndGeneratesDualsIfNeeded()
    {
        string code = """
                      a.
                      a :- b.
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        // Solution was verified with s(CASP)
        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 3);
            Assert.That(duals[0].ToString() == "not a :- not a1, not a2.");
            Assert.That(duals[1].ToString() == "not a2 :- not b.");
            Assert.That(duals[2].ToString() == "not b.");
        });
    }

    [Test]
    public void DualConverterConvertsIsToIsNot()
    {
        string code = """
                      abs(X, X) :- X >= 0.
                      abs(X, Y) :- X < 0, Y is X * -1.
                      """;

        var program = AspExtensions.GetProgram(code, this.logger);
        var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

        var duals = dualRuleConverter.GetDualRules(program.Statements);

        Assert.Multiple(() =>
        {
            Assert.That(duals.Count == 5);
            Assert.That(duals[0].ToString(), Is.EqualTo("not abs(V1, V2) :- not abs1(V1, V2), not abs2(V1, V2)."));
            Assert.That(duals[1].ToString(), Is.EqualTo("not abs1(X, V1) :- V1 \\= X."));
            Assert.That(duals[2].ToString(), Is.EqualTo("not abs1(X, V1) :- V1 = X, X < 0."));
            Assert.That(duals[3].ToString(), Is.EqualTo("not abs2(X, Y) :- X >= 0."));
            Assert.That(duals[4].ToString(), Is.EqualTo("not abs2(X, Y) :- X < 0, Y is not X * -1."));
        });
    }
}