%Funktion fibo(n) welche die Fibonacci Zahl an der Stelle n berechnet
fibo(1, 1).
fibo(2, 2).
fibo(N, R) :-
    N > 1,
    F is N - 1,
    S is N - 2,
    fibo(F, FF),
    fibo(S, SF),
    R is FF + SF.

%Schreiben Sie eine Funktion sum(l1,l2) welche jedes Element miteinander addiert --> l1[0] + l2[0]....
sum([], [], []).
sum([X|XS], [Y|YS], [R|RS]) :- 
    R is X + Y,
    sum(XS, YS, RS).

%compact(l) (strings) welche duplikate aus der liste entfernt --> compact(["abc", "abc", "d", "d", "e"]) -> ["abc", "d", "e"]

% compact(L, R).
compact(L, R) :- 
    comp(L, [], R).
    
comp([], I, O) :-
    O = I.
comp([X|XS], I, O) :-
    try_add(I, X, RT),
    comp(XS, RT, R),
    O = R.
    
% If it is in the list just return the list again
copy([], []).
copy([X|XS], [C|CS]) :- 
    C = X,
    copy(XS, CS).

try_add([], Y, [Y]).
try_add([X|XS], X, [R|RS]) :- 
    R = X,
    copy(XS, RS).
    
try_add([X|XS], Y, [R|RS]) :-
    R = X,
    X \= Y,
    try_add(XS, Y, RS).

%middle(l,m,r) welches ermittelt ob die zahl m zwischen l und r liegt --> middle(3,5,7) --> 1
middle(N, N, N).
middle(L, M, U) :- 
    L < U,
    U > 0,
    LT is L + 1,
    UT is U - 1,
    middle(LT, M, UT).

%maximize(l1) welches jedes element der liste durch das größte vorkommniss ersetzt --> maximize([1,3,7]) -> [7, 7, 7]

maximize([X|XS], R) :- 
    largest([X|XS], X, L),
    replaceAll([X|XS],L, R).

replaceAll([], L, []).
replaceAll([X|XS], L, [L|LS]) :- 
    replaceAll(XS, L, LS).


largest([], ACC, R) :-
    R = ACC.

largest([X|XS], ACC, R) :- 
    X <= ACC,
    largest(XS, ACC, RT),
    R = RT.

largest([X|XS], ACC, R) :- 
    X > ACC,
    largest(XS, X, RT),
    R = RT.