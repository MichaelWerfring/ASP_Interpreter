opera(D) :- not home(D).
home(D):- not opera(D).

home(monday).

:- baby(D), opera(D).

baby(tuesday).

%global_constraint :-
%     not o_chk_1.

%not o_chk_1 :-
%     not o__chk_1_1.

%not o__chk_1_1 :-
%     forall(Var0,not o__chk_1_1(Var0)).

%not o__chk_1_1(Var0) :-
     baby(Var0),
     not opera(Var0).

%not o__chk_1_1(Var0) :-
     not baby(Var0).