using asp_interpreter_lib.Solving;
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
                      flies(sam)?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 11);
                Assert.That(duals[0].ToString() == "not_penguin(V0) :- V0 \\= sam.");
                Assert.That(duals[1].ToString() == "not_wounded_bird(V0) :- V0 \\= john.");
                Assert.That(duals[2].ToString() == "not_bird(V1) :- not_bird1(V1), not_bird2(V1), not_bird3(V1).");
                Assert.That(duals[3].ToString() == "not_bird1(V0) :- V0 \\= tweety.");
                Assert.That(duals[4].ToString() == "not_bird2(X) :- not penguin(X).");
                Assert.That(duals[5].ToString() == "not_bird3(X) :- not wounded_bird(X).");
                Assert.That(duals[6].ToString() == "not_ab(V1) :- not_ab1(V1), not_ab2(V1).");
                Assert.That(duals[7].ToString() == "not_ab1(X) :- not penguin(X).");
                Assert.That(duals[8].ToString() == "not_ab2(X) :- not wounded_bird(X).");
                Assert.That(duals[9].ToString() == "not_flies(X) :- not bird(X).");
                Assert.That(duals[10].ToString() == "not_flies(X) :- bird(X), ab(X).");
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
                      
                      flies(sam)?
                      """;


            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);
            var duals = dualRuleConverter.GetDualRules(program.Statements);

            //Solution was verified with s(CASP)
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 18);
                Assert.That(duals[0].ToString() == "not_penguin(V0) :- V0 \\= sam.");
                Assert.That(duals[1].ToString() == "not_wounded_bird(V0) :- V0 \\= john.");
                Assert.That(duals[2].ToString() == "not_bird(V1) :- not_bird1(V1), not_bird2(V1), not_bird3(V1).");
                Assert.That(duals[3].ToString() == "not_bird1(V0) :- V0 \\= tweety.");
                Assert.That(duals[4].ToString() == "not_bird2(X) :- not penguin(X).");
                Assert.That(duals[5].ToString() == "not_bird3(X) :- not wounded_bird(X).");
                Assert.That(duals[6].ToString() == "not_ab(V1) :- not_ab1(V1), not_ab2(V1).");
                Assert.That(duals[7].ToString() == "not_ab1(X) :- not penguin(X).");
                Assert.That(duals[8].ToString() == "not_ab2(X) :- not wounded_bird(X).");
                Assert.That(duals[9].ToString() == "not_flies(X) :- not bird(X).");
                Assert.That(duals[10].ToString() == "not_flies(X) :- bird(X), ab(X).");
                Assert.That(duals[11].ToString() == "-not_flies(V1) :- -not_flies1(V1), -not_flies2(V1).");
                Assert.That(duals[12].ToString() == "-not_flies1(X) :- not ab(X).");
                Assert.That(duals[13].ToString() == "-not_flies2(X) :- not -bird(X).");
                Assert.That(duals[14].ToString() == "-not_wounded_bird(X) :- wounded_bird(X).");
                Assert.That(duals[15].ToString() == "-not_penguin(X) :- penguin(X).");
                Assert.That(duals[16].ToString() == "-not_ab(X) :- ab(X).");
                Assert.That(duals[17].ToString() == "-not_bird(X) :- bird(X).");
            });
        }

        [Test]
        public void GPATest()
        {
            string code = """
                      eligible(X) :- highGPA(X).
                      eligible(X) :- special(X), fairGPA(X).
                      -eligible(X) :- -special(X), -highGPA(X).

                      interview(X) :- not eligible(X), not -eligible(X).
                      fairGPA(john).
                      -highGPA(john).
                      ?- interview(john).
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 13);
                Assert.That(duals[0].ToString() == "not_eligible(V1) :- not_eligible1(V1), not_eligible2(V1).");
                Assert.That(duals[1].ToString() == "not_eligible1(X) :- not highGPA(X).");
                Assert.That(duals[2].ToString() == "not_eligible2(X) :- not special(X).");
                Assert.That(duals[3].ToString() == "not_eligible2(X) :- special(X), not fairGPA(X).");
                Assert.That(duals[4].ToString() == "-not_eligible(X) :- not -special(X).");
                Assert.That(duals[5].ToString() == "-not_eligible(X) :- -special(X), not -highGPA(X).");
                Assert.That(duals[6].ToString() == "not_interview(X) :- eligible(X).");
                Assert.That(duals[7].ToString() == "not_interview(X) :- not eligible(X), -eligible(X).");
                Assert.That(duals[8].ToString() == "not_fairGPA(V0) :- V0 \\= john.");
                Assert.That(duals[9].ToString() == "-not_highGPA(V0) :- V0 \\= john.");
                Assert.That(duals[10].ToString() == "not_highGPA(X).");
                Assert.That(duals[11].ToString() == "not_special(X).");
                Assert.That(duals[12].ToString() == "-not_special(X).");
            });
        }

        [Test]
        public void HamcycleTest()
        {
            string code = """
                      % fact for each vertex(N).
                      vertex(0).
                      vertex(1).
                      vertex(2).

                      % fact for each edge edge(U, V).
                      edge(0, 1).
                      edge(1, 2).
                      edge(2, 0).

                      reachable(V) :- chosen(U, V), reachable(U).
                      reachable(0) :- chosen(V, 0).

                      % Every vertex must be reachable.
                      :- vertex(U), not reachable(U).

                      % Choose exactly one edge from each vertex.
                      other(U, V) :-
                          vertex(U), vertex(V), vertex(W),
                          edge(U, W), V \= W, chosen(U, W).
                      chosen(U, V) :-
                          edge(U, V), not other(U, V).

                      % You cannot choose two edges to the same vertex
                      :- chosen(U, W), chosen(V, W), U \= V.
                      chosen(1,2)?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 1);
                Assert.That(duals[0].ToString() == "");
            });
        }

        [Test]
        public void HanoiTest()
        {
            string code = """
                      % Move N disks in T moves.
                      hanoi(N, T) :-
                              moven(N, 0, T, 1, 2, 3).

                      % Move N disks from peg A to peg B using peg C. Assign move numbers.
                      moven(N, Ti, To, A, B, C) :-
                              N > 1,
                              N1 is N - 1,
                              moven(N1, Ti, T2, A, C, B),
                              T3 is T2 + 1,
                              move(T3, A, B),
                              moven(N1, T3, To, C, B, A).
                      moven(1, Ti, To, A, B, _) :-
                              To is Ti + 1,
                              move(To, A, B).

                      % move T: move disk from P1 to P2.
                      % any move may or may not be selected.
                      move(T, P1, P2) :-
                              not negmove(T, P1, P2).

                      negmove(T, P1, P2) :-
                              not move(T, P1, P2).

                      hanoi(3, T)?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 1);
                Assert.That(duals[0].ToString() == "");
            });
        }

        [Test]
        public void LoopTest()
        {
            string code = """
                      p(X) :- not q(X).
                      q(X) :- not p(X).
                      r(X) :- X \= 3, X \= 4, q(X).
                      p(X), r(Y)?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 1);
                Assert.That(duals[0].ToString() == "");
            });
        }

        [Test]
        public void MemberTest()
        {
            string code = """
                      member(X,[X|T]).
                      member(X,[Y|T]) :- X\=Y, member(X,T).

                      member(1, [1, 2, 3])?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 1);
                Assert.That(duals[0].ToString() == "");
            });
        }

        [Test]
        public void NatnumTest()
        {
            string code = """
                      s(0).
                      s(s(X)) :-
                      	s(X).

                      sum(s(0), X, s(X)).
                      sum(X, s(0), s(X)).
                      sum(s(X), Y, s(Z)) :-
                      	X \= 0,
                      	sum(X, Y, Z).

                      sumlist([X | T], [Y | T2], [Z | T3]) :-
                              sum(X, Y, Z),
                              sumlist(T, T2, T3).
                      sumlist([], [], []).

                      sumlist([s(s(0)), s(s(s(0)))], [s(s(s(0))), s(s(s(0)))], X)?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 1);
                Assert.That(duals[0].ToString() == "");
            });
        }

        [Test]
        public void QueensTest()
        {
            string code = """
                      nqueens(N, Q) :-
                              _nqueens(N, N, [], Q).

                      _nqueens(X, N, Qi, Qo) :-
                              X > 0,
                              _pickqueen(X, Y, N),
                              not _attack(X, Y, Qi),
                              X1 is X - 1,
                              _nqueens(X1, N, [q(X, Y) | Qi], Qo).
                      _nqueens(0, _, Q, Q).

                      _pickqueen(X, Y, Y) :-
                              Y > 0,
                              q(X, Y).
                      _pickqueen(X, Y, N) :-
                              N > 1,
                              N1 is N - 1,
                              _pickqueen(X, Y, N1).

                      _attack(X, _, [q(X, _) | _]). % same row
                      _attack(_, Y, [q(_, Y) | _]). % same col
                      _attack(X, Y, [q(X2, Y2) | _]) :- % same diagonal
                              Xd is X2 - X,
                              _abs(Xd, Xd2),
                              Yd is Y2 - Y,
                              _abs(Yd, Yd2),
                              Xd2 = Yd2.
                      _attack(X, Y, [_ | T]) :-
                              _attack(X, Y, T).

                      q(X, Y) :- not _negq(X, Y).
                      _negq(X, Y) :- not q(X, Y).

                      _abs(X, X) :- X >= 0.
                      _abs(X, Y) :- X < 0, Y is X * -1.

                      nqueens(4, Q)?
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 1);
                Assert.That(duals[0].ToString() == "");
            });
        }
    }
}
