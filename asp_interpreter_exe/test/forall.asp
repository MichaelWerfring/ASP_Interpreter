% fact for each vertex(N).
vertex(0).
vertex(1).

% fact for each edge edge(U, V).
edge(0, 1).

reachable(V) :- chosen(U, V), reachable(U).
reachable(0) :- chosen(V, 0).

% Choose exactly one edge from each vertex.
other(U, V) :-
    vertex(U), vertex(V), vertex(W),
    edge(U, W), V \= W, chosen(U, W).
chosen(U, V) :-
    edge(U, V), not other(U, V).

% You cannot choose two edges to the same vertex
:- chosen(U, W), chosen(V, W), U \= V.


?- chosen(X, Y).