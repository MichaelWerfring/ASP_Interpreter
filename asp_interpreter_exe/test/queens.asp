abs(X, X) :- X >= 0.
abs(X, Y) :- X < 0, Y is X * -1.

?- nqueens(4, Q).