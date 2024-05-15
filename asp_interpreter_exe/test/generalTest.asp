opera(D) :- not home(D).
home(D):- not opera(D).

home(monday).

:- baby(D), opera(D).

baby(tuesday).

?- opera(D).