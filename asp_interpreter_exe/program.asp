node(a).
node(b).
node(c).
edge(a, b).
edge(b, c).

edge(X, Y) :- node(X), node(Y), edge(Y, X).
separate(X, Y) :- node(X), node(Y), not edge(X, Y).

a(X) :- 2 + 2 = X.

edge(X, b)?