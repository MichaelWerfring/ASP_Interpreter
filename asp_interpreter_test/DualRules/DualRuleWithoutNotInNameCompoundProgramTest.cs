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
    internal class DualRuleWithoutNameCompoundProgramTest
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
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger, false);

            var duals = dualRuleConverter.GetDualRules(program.Statements, false);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count, Is.EqualTo(11));
                Assert.That(duals[0].ToString(), Is.EqualTo("not penguin(V0) :- V0 \\= sam."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not wounded_bird(V0) :- V0 \\= john."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not bird(V1) :- not bird1(V1), not bird2(V1), not bird3(V1)."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not bird1(V0) :- V0 \\= tweety."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not bird2(X) :- not penguin(X)."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not bird3(X) :- not wounded_bird(X)."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not ab(V1) :- not ab1(V1), not ab2(V1)."));
                Assert.That(duals[7].ToString(), Is.EqualTo("not ab1(X) :- not penguin(X)."));
                Assert.That(duals[8].ToString(), Is.EqualTo("not ab2(X) :- not wounded_bird(X)."));
                Assert.That(duals[9].ToString(), Is.EqualTo("not flies(X) :- not bird(X)."));
                Assert.That(duals[10].ToString(), Is.EqualTo("not flies(X) :- bird(X), ab(X)."));
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
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger, false);
            var duals = dualRuleConverter.GetDualRules(program.Statements, false);
            
            //The output of this test has is based 
            //on the original s(CASP) implementation
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals, Has.Count.EqualTo(18));
                Assert.That(duals[0].ToString(), Is.EqualTo("not penguin(V0) :- V0 \\= sam."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not wounded_bird(V0) :- V0 \\= john."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not bird(V1) :- not bird1(V1), not bird2(V1), not bird3(V1)."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not bird1(V0) :- V0 \\= tweety."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not bird2(X) :- not penguin(X)."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not bird3(X) :- not wounded_bird(X)."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not ab(V1) :- not ab1(V1), not ab2(V1)."));
                Assert.That(duals[7].ToString(), Is.EqualTo("not ab1(X) :- not penguin(X)."));
                Assert.That(duals[8].ToString(), Is.EqualTo("not ab2(X) :- not wounded_bird(X)."));
                Assert.That(duals[9].ToString(), Is.EqualTo("not flies(X) :- not bird(X)."));
                Assert.That(duals[10].ToString(), Is.EqualTo("not flies(X) :- bird(X), ab(X)."));
                Assert.That(duals[11].ToString(), Is.EqualTo("not -flies(V1) :- not -flies1(V1), not -flies2(V1)."));
                Assert.That(duals[12].ToString(), Is.EqualTo("not -flies1(X) :- not ab(X)."));
                Assert.That(duals[13].ToString(), Is.EqualTo("not -flies2(X) :- not -bird(X)."));
                Assert.That(duals[14].ToString(), Is.EqualTo("not -wounded_bird(X) :- wounded_bird(X)."));
                Assert.That(duals[15].ToString(), Is.EqualTo("not -penguin(X) :- penguin(X)."));
                Assert.That(duals[16].ToString(), Is.EqualTo("not -ab(X) :- ab(X)."));
                Assert.That(duals[17].ToString(), Is.EqualTo("not -bird(X) :- bird(X)."));
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
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger, false);

            var duals = dualRuleConverter.GetDualRules(program.Statements , false);

            
            //The output of this test has is based 
            //on the original s(CASP) implementation
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 13);
                Assert.That(duals[0].ToString() == "not eligible(V1) :- not eligible1(V1), not eligible2(V1).");
                Assert.That(duals[1].ToString() == "not eligible1(X) :- not highGPA(X).");
                Assert.That(duals[2].ToString() == "not eligible2(X) :- not special(X).");
                Assert.That(duals[3].ToString() == "not eligible2(X) :- special(X), not fairGPA(X).");
                Assert.That(duals[4].ToString() == "not -eligible(X) :- not -special(X).");
                Assert.That(duals[5].ToString() == "not -eligible(X) :- -special(X), not -highGPA(X).");
                Assert.That(duals[6].ToString() == "not interview(X) :- eligible(X).");
                Assert.That(duals[7].ToString() == "not interview(X) :- not eligible(X), -eligible(X).");
                Assert.That(duals[8].ToString() == "not fairGPA(V0) :- V0 \\= john.");
                Assert.That(duals[9].ToString() == "not -highGPA(V0) :- V0 \\= john.");
                Assert.That(duals[10].ToString() == "not highGPA(X).");
                Assert.That(duals[11].ToString() == "not special(X).");
                Assert.That(duals[12].ToString() == "not -special(X).");
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
                      ?-chosen(1,2).
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger, false);

            var duals = dualRuleConverter.GetDualRules(program.Statements, false);

            // Verified by original s(CASP) implementation
            // 5-10 is switched in Order
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals, Has.Count.EqualTo(27));
                Assert.That(duals[0].ToString(), Is.EqualTo("not vertex(V1) :- not vertex1(V1), not vertex2(V1), not vertex3(V1)."));
                Assert.That(duals[1].ToString(), Is.EqualTo("not vertex1(V0) :- V0 \\= 0."));
                Assert.That(duals[2].ToString(), Is.EqualTo("not vertex2(V0) :- V0 \\= 1."));
                Assert.That(duals[3].ToString(), Is.EqualTo("not vertex3(V0) :- V0 \\= 2."));
                Assert.That(duals[4].ToString(), Is.EqualTo("not edge(V1, V2) :- not edge1(V1, V2), not edge2(V1, V2), not edge3(V1, V2)."));
                Assert.That(duals[5].ToString(), Is.EqualTo("not edge1(V0, V1) :- V1 \\= 1."));
                Assert.That(duals[6].ToString(), Is.EqualTo("not edge1(V0, V1) :- V1 = 1, V0 \\= 0."));
                Assert.That(duals[7].ToString(), Is.EqualTo("not edge2(V0, V1) :- V1 \\= 2."));
                Assert.That(duals[8].ToString(), Is.EqualTo("not edge2(V0, V1) :- V1 = 2, V0 \\= 1."));
                Assert.That(duals[9].ToString(), Is.EqualTo("not edge3(V0, V1) :- V1 \\= 0."));
                Assert.That(duals[10].ToString(), Is.EqualTo("not edge3(V0, V1) :- V1 = 0, V0 \\= 2."));
                Assert.That(duals[11].ToString(), Is.EqualTo("not reachable(V1) :- not reachable1(V1), not reachable2(V1)."));
                Assert.That(duals[12].ToString(), Is.EqualTo("not reachable1(V) :- forall(U, not fa_reachable1(V, U))."));
                Assert.That(duals[13].ToString(), Is.EqualTo("not fa_reachable1(V, U) :- not chosen(U, V)."));
                Assert.That(duals[14].ToString(), Is.EqualTo("not fa_reachable1(V, U) :- chosen(U, V), not reachable(U)."));
                Assert.That(duals[15].ToString(), Is.EqualTo("not reachable2(V0) :- forall(V, not fa_reachable2(V0, V))."));
                Assert.That(duals[16].ToString(), Is.EqualTo("not fa_reachable2(V0, V) :- V0 \\= 0."));
                Assert.That(duals[17].ToString(), Is.EqualTo("not fa_reachable2(V0, V) :- V0 = 0, not chosen(V, 0)."));
                Assert.That(duals[18].ToString(), Is.EqualTo("not other(U, V) :- forall(W, not fa_other(U, V, W))."));
                Assert.That(duals[19].ToString(), Is.EqualTo("not fa_other(U, V, W) :- not vertex(U)."));
                Assert.That(duals[20].ToString(), Is.EqualTo("not fa_other(U, V, W) :- vertex(U), not vertex(V)."));
                Assert.That(duals[21].ToString(), Is.EqualTo("not fa_other(U, V, W) :- vertex(U), vertex(V), not vertex(W)."));
                Assert.That(duals[22].ToString(), Is.EqualTo("not fa_other(U, V, W) :- vertex(U), vertex(V), vertex(W), not edge(U, W)."));
                Assert.That(duals[23].ToString(), Is.EqualTo("not fa_other(U, V, W) :- vertex(U), vertex(V), vertex(W), edge(U, W), V = W."));
                Assert.That(duals[24].ToString(), Is.EqualTo("not fa_other(U, V, W) :- vertex(U), vertex(V), vertex(W), edge(U, W), V \\= W, not chosen(U, W)."));
                Assert.That(duals[25].ToString(), Is.EqualTo("not chosen(U, V) :- not edge(U, V)."));
                Assert.That(duals[26].ToString(), Is.EqualTo("not chosen(U, V) :- edge(U, V), other(U, V)."));
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
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger, false);

            var duals = dualRuleConverter.GetDualRules(program.Statements, false);

            //Verified by s(CASP) implementation
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 5);
                Assert.That(duals[0].ToString() == "not p(X) :- q(X).");
                Assert.That(duals[1].ToString() == "not q(X) :- p(X).");
                Assert.That(duals[2].ToString() == "not r(X) :- X = 3.");
                Assert.That(duals[3].ToString() == "not r(X) :- X \\= 3, X = 4.");
                Assert.That(duals[4].ToString() == "not r(X) :- X \\= 3, X \\= 4, not q(X).");
            });
        }

        [Test]
        public void MemberTest()
        {
            string code = """
                      member(X,[X|T]).
                      member(X,[Y|T]) :- X\=Y, member(X,T).

                      ?-member(1, [1, 2, 3]).
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger, false);

            var duals = dualRuleConverter.GetDualRules(program.Statements, false);
            
            //Verified by s(CASP)
            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 7);
                Assert.That(duals[0].ToString() == "not member(V1, V2) :- not member1(V1, V2), not member2(V1, V2).");
                Assert.That(duals[1].ToString() == "not member1(X, V0) :- forall(T, not fa_member1(X, V0, T)).");
                Assert.That(duals[2].ToString() == "not fa_member1(X, V0, T) :- V0 \\= [X| T].");
                Assert.That(duals[3].ToString() == "not member2(X, V0) :- forall(Y, forall(T, not fa_member2(X, V0, Y, T))).");
                Assert.That(duals[4].ToString() == "not fa_member2(X, V0, Y, T) :- V0 \\= [Y| T].");
                Assert.That(duals[5].ToString() == "not fa_member2(X, V0, Y, T) :- V0 = [Y| T], X = Y.");
                Assert.That(duals[6].ToString() == "not fa_member2(X, V0, Y, T) :- V0 = [Y| T], X \\= Y, not member(X, T).");
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
                              n_queens(N, N, [], Q).

                      n_queens(X, N, Qi, Qo) :-
                              X > 0,
                              pick_queen(X, Y, N),
                              not _attack(X, Y, Qi),
                              X1 is X - 1,
                              n_queens(X1, N, [q(X, Y) | Qi], Qo).
                      n_queens(0, _, Q, Q).

                      pick_queen(X, Y, Y) :-
                              Y > 0,
                              q(X, Y).
                      pick_queen(X, Y, N) :-
                              N > 1,
                              N1 is N - 1,
                              pick_queen(X, Y, N1).

                      attack(X, _, [q(X, _) | _]). % same row
                      attack(_, Y, [q(_, Y) | _]). % same col
                      attack(X, Y, [q(X2, Y2) | _]) :- % same diagonal
                              Xd is X2 - X,
                              abs(Xd, Xd2),
                              Yd is Y2 - Y,
                              abs(Yd, Yd2),
                              Xd2 = Yd2.
                      attack(X, Y, [_ | T]) :-
                              attack(X, Y, T).

                      q(X, Y) :- not negg(X, Y).
                      negg(X, Y) :- not q(X, Y).

                      abs(X, X) :- X >= 0.
                      abs(X, Y) :- X < 0, Y is X * -1.

                      ?-nqueens(4, Q).
                      """;

            var program = AspExtensions.GetProgram(code, _logger);
            var dualRuleConverter = new DualRuleConverter(_prefixes, _logger);

            var duals = dualRuleConverter.GetDualRules(program.Statements);

            Assert.Multiple(() =>
            {
                Assert.That(_logger.ErrorMessages.Count == 0);
                Assert.That(duals.Count == 1);
                Assert.That(duals[0].ToString(), Is.EqualTo(""));
            });
        }
    }
}
