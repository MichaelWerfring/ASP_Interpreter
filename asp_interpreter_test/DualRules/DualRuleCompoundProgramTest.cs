﻿using asp_interpreter_lib.Solving;
using asp_interpreter_lib.Solving.DualRules;
using asp_interpreter_lib.Util;
using asp_interpreter_lib.Util.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asp_interpreter_test.DualRules
{
    internal class DualRuleCompoundProgramTest
    {
        private readonly PrefixOptions _prefixes = AspExtensions.CommonPrefixes;

        private readonly TestingLogger _logger = new TestingLogger(LogLevel.Error);

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

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 14);
                Assert.That(duals[0].ToString(), Is.EqualTo("not_penguin(V1) :- not_penguin1(V1)."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not_penguin1(V1) :- V1 \\= sam."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not_wounded_bird(V1) :- not_wounded_bird1(V1)."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not_wounded_bird1(V1) :- V1 \\= john."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not_bird(V1) :- not_bird1(V1), not_bird2(V1), not_bird3(V1)."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not_bird1(V1) :- V1 \\= tweety."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not_bird2(X) :- not penguin(X)."));
                Assert.That(duals[7].ToString(), Is.EqualTo("not_bird3(X) :- not wounded_bird(X)."));
                Assert.That(duals[8].ToString(), Is.EqualTo("not_ab(V1) :- not_ab1(V1), not_ab2(V1)."));
                Assert.That(duals[9].ToString(), Is.EqualTo("not_ab1(X) :- not penguin(X)."));
                Assert.That(duals[10].ToString(), Is.EqualTo("not_ab2(X) :- not wounded_bird(X)."));
                Assert.That(duals[11].ToString(), Is.EqualTo("not_flies(V1) :- not_flies1(V1)."));
                Assert.That(duals[12].ToString(), Is.EqualTo("not_flies1(X) :- not bird(X)."));
                Assert.That(duals[13].ToString(), Is.EqualTo("not_flies1(X) :- bird(X), ab(X)."));
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


            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);
            var duals = dualRuleConverter.GetDualRules(program.Statements);
            
            //The output of this test has is based 
            //on the original s(CASP) implementation
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 25);
                Assert.That(duals[0].ToString(), Is.EqualTo("not_penguin(V1) :- not_penguin1(V1)."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not_penguin1(V1) :- V1 \\= sam."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not_wounded_bird(V1) :- not_wounded_bird1(V1)."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not_wounded_bird1(V1) :- V1 \\= john."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not_bird(V1) :- not_bird1(V1), not_bird2(V1), not_bird3(V1)."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not_bird1(V1) :- V1 \\= tweety."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not_bird2(X) :- not penguin(X)."));
                Assert.That(duals[7].ToString(), Is.EqualTo("not_bird3(X) :- not wounded_bird(X)."));
                Assert.That(duals[8].ToString(), Is.EqualTo("not_ab(V1) :- not_ab1(V1), not_ab2(V1)."));
                Assert.That(duals[9].ToString(), Is.EqualTo("not_ab1(X) :- not penguin(X)."));
                Assert.That(duals[10].ToString(), Is.EqualTo("not_ab2(X) :- not wounded_bird(X)."));
                Assert.That(duals[11].ToString(), Is.EqualTo("not_flies(V1) :- not_flies1(V1)."));
                Assert.That(duals[12].ToString(), Is.EqualTo("not_flies1(X) :- not bird(X)."));
                Assert.That(duals[13].ToString(), Is.EqualTo("not_flies1(X) :- bird(X), ab(X)."));
                Assert.That(duals[14].ToString(), Is.EqualTo("-not_flies(V1) :- -not_flies1(V1), -not_flies2(V1)."));
                Assert.That(duals[15].ToString(), Is.EqualTo("-not_flies1(X) :- not ab(X)."));
                Assert.That(duals[16].ToString(), Is.EqualTo("-not_flies2(X) :- not -bird(X)."));
                Assert.That(duals[17].ToString(), Is.EqualTo("-not_wounded_bird(V1) :- -not_wounded_bird1(V1)."));
                Assert.That(duals[18].ToString(), Is.EqualTo("-not_wounded_bird1(X) :- wounded_bird(X)."));
                Assert.That(duals[19].ToString(), Is.EqualTo("-not_penguin(V1) :- -not_penguin1(V1)."));
                Assert.That(duals[20].ToString(), Is.EqualTo("-not_penguin1(X) :- penguin(X)."));
                Assert.That(duals[21].ToString(), Is.EqualTo("-not_ab(V1) :- -not_ab1(V1)."));
                Assert.That(duals[22].ToString(), Is.EqualTo("-not_ab1(X) :- ab(X)."));
                Assert.That(duals[23].ToString(), Is.EqualTo("-not_bird(V1) :- -not_bird1(V1)."));
                Assert.That(duals[24].ToString(), Is.EqualTo("-not_bird1(X) :- bird(X)."));
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

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);
            
            //Verified by s(CASP)
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 7);
                Assert.That(duals[0].ToString(), Is.EqualTo("not_member(V1, V2) :- not_member1(V1, V2), not_member2(V1, V2)."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not_member1(X, V1) :- forall(T, fa_member1(X, V1, T))."));
                Assert.That(duals[2].ToString(), Is.EqualTo("fa_member1(X, V1, T) :- V1 \\= [X| T]."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not_member2(X, V1) :- forall(Y, forall(T, fa_member2(X, V1, Y, T)))."));
                Assert.That(duals[4].ToString(), Is.EqualTo("fa_member2(X, V1, Y, T) :- V1 \\= [Y| T]."));
                Assert.That(duals[5].ToString(), Is.EqualTo("fa_member2(X, V1, Y, T) :- V1 = [Y| T], X = Y."));
                Assert.That(duals[6].ToString(), Is.EqualTo("fa_member2(X, V1, Y, T) :- V1 = [Y| T], X \\= Y, not member(X, T)."));
            });
        }
    }
}
