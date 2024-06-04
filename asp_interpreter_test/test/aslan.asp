animal(lion).
animal(snake).
animal(monkey).
animal(rat).

live(monkey, forest).
live(snake, zoo).
live(rat, apartment).

live(X, forest) :-
    animal(X),
    not ab(live_animal(X)),
    not -live(X, forest).

-live(X, Z) :-
    live(X, Y),
    Y \= Z.



?- live(lion, X).