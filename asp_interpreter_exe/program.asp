man(granddad).
man(father).
man(son).
woman(grandmother).
woman(mother).
woman(daugther).

parentOf(granddad, father).
parentOf(grandmother, father).
parentOf(father, son).
parentOf(father, daugther).
parentOf(mother, son).
parentOf(mother, daugther).

siblings(X, Y) :- parentOf(P, X), parentOf(P, Y).

ancestor(X, Y) :- parentOf(X, Y).
ancestor(X, Y) :- parentOf(X, Z), ancestor(Z, Y).

?- ancestor(granddad, son).