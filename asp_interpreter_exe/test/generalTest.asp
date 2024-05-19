a(X) :- not b(X).
b(X) :- X \= 1, not a(X).

?- a(X).