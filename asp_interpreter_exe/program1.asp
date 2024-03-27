a :- not b.
a :- c.
c :- not e.
e:- not a.


g :- s, not b.
a :- f, g.

s :- not a, c.
s :- not e, g.
g :- not b.

b :- a.

edge(X, b)?