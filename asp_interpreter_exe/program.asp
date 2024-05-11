faster(bike, skateboard).
faster(toyota_corolla, bike).
faster(bmw_z3, toyota_corolla).
faster(enterprise, bmw_z3).

is_faster(X, Y) :- faster(X, Y).
is_faster(X, Y) :- faster(X, Z), is_faster(Z, Y).

fastest(X) :- not is_faster(Z, X).