faster(bike, skateboard).
faster(bmw_z3, toyota_corolla).
faster(enterprise, toyota_corolla).

is_faster(X, Y) :- faster(X, Y).
is_faster(X, Y) :- faster(X, Z), is_faster(Z, Y).

fastest(X) :- not is_faster(Z, X).