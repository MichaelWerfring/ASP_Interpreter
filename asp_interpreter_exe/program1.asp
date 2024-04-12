nat(0).
nat(s(X)) :- nat(X).

calculate(X, Y, add, Z) :- nat(X), nat(Y), add(X,Y,Z).
calculate(X, Y, sub, Z) :- nat(X), nat(Y), sub(X,Y,Z).
calculate(X, Y, mult, Z) :- nat(X), nat(Y), mult(X,Y,Z).
calculate(X, Y, div, Z) :- nat(X), nat(Y), div(X,Y,0,Z).

add(Y, 0, Y).
add(0, Y, Y).
add(s(X),Y, s(Z1)) :- add(X, Y, Z1). 

sub(X,0,X).
sub(s(X), s(Y), Z) :- sub(X, Y, Z).

mult(X,s(0),X).
mult(X,s(N), Z) :- mult(X,N, Z1), add(Z1, X, Z).

div(0, Y, Acc, Acc).
div(X, Y, Acc, Z) :- sub(X, Y, Z1), add(Acc, s(0), Acc1),  div(Z1, Y, Acc1, Z).

c(a,b).
c(b,c).
c(c,d).
c(d,e).

con(X,Y) :- c(X,Y).
con(X, Y) :- c(X,Z), con(Z,Y).

calculate(s(s(s(0))), Y, add, Z)?.
