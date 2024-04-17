f(X) :- X is 1.

f(X) :- X = 2.
f(X) :- X is 3, X \= 5.

f(X) :- X is 4, X < 5.
f(X) :- X is 5, X <= 5.
f(X) :- X is 6, X > 5.
f(X) :- X is 7, X >= 5.

f(X) :- not g(X).

f(X)?.