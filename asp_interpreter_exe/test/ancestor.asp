man(granddad).
man(father).
man(son).
woman(grandmother).
woman(mother).
woman(daughter).

parentOf(granddad, father).
parentOf(grandmother, father).
parentOf(father, son).
parentOf(father, daughter).
parentOf(mother, son).
parentOf(mother, daughter).

siblings(X, Y) :- X \= Y, parentOf(P, X), parentOf(P, Y).

ancestor(X, Y) :- parentOf(X, Y).
ancestor(X, Y) :- parentOf(X, Z), ancestor(Z, Y).

childOf(C, P) :- parentOF(P, C).

?- ancestor(granddad, son).