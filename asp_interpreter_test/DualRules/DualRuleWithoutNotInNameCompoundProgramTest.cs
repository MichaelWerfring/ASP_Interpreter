//-----------------------------------------------------------------------
// <copyright file="DualRuleWithoutNotInNameCompoundProgramTest.cs" company="FHWN">
// Copyright (c) PlaceholderCompany. All rights reserved.
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

    internal class DualRuleWithoutNameCompoundProgramTest
    {
        private readonly PrefixOptions prefixes = AspExtensions.CommonPrefixes;

        private readonly TestingLogger logger = new TestingLogger(LogLevels.Error);

        [Test]
        public void BirdsTest()
        {
            string code = """
                      penguin(sam).
                      wounded_bird(john).
                      bird(tweety).
                      
                      bird(X) :- penguin(X).
                      bird(X) :- wounded_bird(X).
                      
                      ab(X) :- penguin(X).
                      ab(X) :- wounded_bird(X).
                      
                      flies(X) :- bird(X), not ab(X).
                      ?- flies(sam).
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(this.logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 14);
                Assert.That(duals[0].ToString(), Is.EqualTo("not penguin(V1) :- not penguin1(V1)."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not penguin1(V1) :- V1 \\= sam."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not wounded_bird(V1) :- not wounded_bird1(V1)."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not wounded_bird1(V1) :- V1 \\= john."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not bird(V1) :- not bird1(V1), not bird2(V1), not bird3(V1)."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not bird1(V1) :- V1 \\= tweety."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not bird2(X) :- not penguin(X)."));
                Assert.That(duals[7].ToString(), Is.EqualTo("not bird3(X) :- not wounded_bird(X)."));
                Assert.That(duals[8].ToString(), Is.EqualTo("not ab(V1) :- not ab1(V1), not ab2(V1)."));
                Assert.That(duals[9].ToString(), Is.EqualTo("not ab1(X) :- not penguin(X)."));
                Assert.That(duals[10].ToString(), Is.EqualTo("not ab2(X) :- not wounded_bird(X)."));
                Assert.That(duals[11].ToString(), Is.EqualTo("not flies(V1) :- not flies1(V1)."));
                Assert.That(duals[12].ToString(), Is.EqualTo("not flies1(X) :- not bird(X)."));
                Assert.That(duals[13].ToString(), Is.EqualTo("not flies1(X) :- bird(X), ab(X)."));
            });
        }

        [Test]
        public void BirdsWithClassicalNegationTest()
        {
            string code = """
                      penguin(sam).
                      wounded_bird(john).
                      bird(tweety).
                      
                      bird(X) :- penguin(X).
                      bird(X) :- wounded_bird(X).
                      
                      ab(X) :- penguin(X).
                      ab(X) :- wounded_bird(X).
                      
                      flies(X) :- bird(X), not ab(X).
                      
                      -flies(X) :- ab(X).
                      -flies(X) :- -bird(X).
                      
                      -wounded_bird(X) :- not wounded_bird(X).
                      -penguin(X) :- not penguin(X).
                      -ab(X) :- not ab(X).
                      -bird(X) :- not bird(X).
                      
                      ?-flies(sam).
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);
            var duals = dualRuleConverter.GetDualRules(program.Statements);

            // The output of this test has is based
            // on the original s(CASP) implementation
            Assert.Multiple(() =>
            {
                Assert.That(this.logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 25);
                Assert.That(duals[0].ToString(), Is.EqualTo("not penguin(V1) :- not penguin1(V1)."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not penguin1(V1) :- V1 \\= sam."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not wounded_bird(V1) :- not wounded_bird1(V1)."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not wounded_bird1(V1) :- V1 \\= john."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not bird(V1) :- not bird1(V1), not bird2(V1), not bird3(V1)."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not bird1(V1) :- V1 \\= tweety."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not bird2(X) :- not penguin(X)."));
                Assert.That(duals[7].ToString(), Is.EqualTo("not bird3(X) :- not wounded_bird(X)."));
                Assert.That(duals[8].ToString(), Is.EqualTo("not ab(V1) :- not ab1(V1), not ab2(V1)."));
                Assert.That(duals[9].ToString(), Is.EqualTo("not ab1(X) :- not penguin(X)."));
                Assert.That(duals[10].ToString(), Is.EqualTo("not ab2(X) :- not wounded_bird(X)."));
                Assert.That(duals[11].ToString(), Is.EqualTo("not flies(V1) :- not flies1(V1)."));
                Assert.That(duals[12].ToString(), Is.EqualTo("not flies1(X) :- not bird(X)."));
                Assert.That(duals[13].ToString(), Is.EqualTo("not flies1(X) :- bird(X), ab(X)."));
                Assert.That(duals[14].ToString(), Is.EqualTo("not -flies(V1) :- not -flies1(V1), not -flies2(V1)."));
                Assert.That(duals[15].ToString(), Is.EqualTo("not -flies1(X) :- not ab(X)."));
                Assert.That(duals[16].ToString(), Is.EqualTo("not -flies2(X) :- not -bird(X)."));
                Assert.That(duals[17].ToString(), Is.EqualTo("not -wounded_bird(V1) :- not -wounded_bird1(V1)."));
                Assert.That(duals[18].ToString(), Is.EqualTo("not -wounded_bird1(X) :- wounded_bird(X)."));
                Assert.That(duals[19].ToString(), Is.EqualTo("not -penguin(V1) :- not -penguin1(V1)."));
                Assert.That(duals[20].ToString(), Is.EqualTo("not -penguin1(X) :- penguin(X)."));
                Assert.That(duals[21].ToString(), Is.EqualTo("not -ab(V1) :- not -ab1(V1)."));
                Assert.That(duals[22].ToString(), Is.EqualTo("not -ab1(X) :- ab(X)."));
                Assert.That(duals[23].ToString(), Is.EqualTo("not -bird(V1) :- not -bird1(V1)."));
                Assert.That(duals[24].ToString(), Is.EqualTo("not -bird1(X) :- bird(X)."));
            });
        }

        [Test]
        public void MemberTest()
        {
            string code = """
                      member(X,[X|T]).
                      member(X,[Y|T]) :- X\=Y, member(X,T).

                      ?- p(X).member(1, [1, 2, 3]).
                      """;

            var program = AspExtensions.GetProgram(code, this.logger);
            var dualRuleConverter = new DualRuleConverter(this.prefixes, this.logger, false);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            // Verified by s(CASP)
            Assert.Multiple(() =>
            {
                Assert.That(this.logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 7);
                Assert.That(duals[0].ToString(), Is.EqualTo("not member(V1, V2) :- not member1(V1, V2), not member2(V1, V2)."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not member1(X, V1) :- forall(T, not fa_member1(X, V1, T))."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not fa_member1(X, V1, T) :- V1 \\= [X| T]."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not member2(X, V1) :- forall(Y, forall(T, not fa_member2(X, V1, Y, T)))."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not fa_member2(X, V1, Y, T) :- V1 \\= [Y| T]."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not fa_member2(X, V1, Y, T) :- V1 = [Y| T], X = Y."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not fa_member2(X, V1, Y, T) :- V1 = [Y| T], X \\= Y, not member(X, T)."));
            });
        }
    }
}