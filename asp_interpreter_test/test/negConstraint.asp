a(X) :- b(X).
-a(X, 1).
b(1).
-d(X) :- c(X).
c(42).
d(Y) :- b(Y).

% Expected NMR-Check:
% global_constraint :-
%      not o_chk_1.

% not o_chk_1 :-
%      not o__chk_1_1.

% not o__chk_1_1 :-
%      forall(Var0,not o__chk_1_1(Var0)).

% b and -b cannot be ture at the same time: 

% not o__chk_1_1(Var0) :-
%     -d(Var0),
%      not d(Var0).
% not o__chk_1_1(Var0) :-
%      not -d(Var0).