
fib(0, 0).
fib(1, 1).
fib(N, F) :- N1 is N - 1, N2 is N - 2, fib(N1, F1), fib(N2, F2), F is F1 + F2.

% rectangle stuff.
isInRectangle(vector(X, Y), rectangle(vector(LX, LY), vector(RX, RY))) :- 
    isInRange(X, LX, RX), 
    isInRange(Y, LY, RY).

isInRange(X, Low, High) :- Low <= High, X <= High, X >= Low.

mod(A, B, C) :- 
    Div is A / B, 
    Real is Div * B,
    C is A - Real.

isPrime(N) :- N1 is N - 1, not hasDivisor(N, N1).

hasDivisor(N, Div) :- Div > 1, mod(N, Div, 0).
hasDivisor(N, Div) :- Div > 1, mod(N,Div, D), D \= 0, Div1 is Div - 1, hasDivisor(N, Div1).


% shit with lists
append([], L, L).
append([H | T], L, [H | T1]) :- append(T, L, T1).

reverse(L, Rev) :- reverse_acc(L, [], Rev).
reverse_acc([], Acc, Acc).
reverse_acc([H | T], Acc, Rev) :- Acc1 = [H | Acc], reverse_acc(T, Acc1, Rev).

flatten([], []).
flatten([H | T], Fl) :- flatten(T, Fl1), append(H, Fl1, Fl).

sum([H | []], H).
sum([H | T], Sum) :- sum(T, TSum), Sum is H + TSum.

larger(A, B, A) :- A >= B.
larger(A, B, B) :- B > A.

max([H | []], H).
max([H | T], Max) :- max(T, TMax), larger(H, TMax, Max).

?- fib(5, F).