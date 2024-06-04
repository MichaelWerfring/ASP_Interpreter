append([],L,L).
append([H | T], L, [H | T1]) :-  append(T, L, T1).

filterSmaller([], _, []).
filterSmaller([H | T], E, [H | T1]) :- H < E, filterSmaller(T, E, T1).
filterSmaller([H | T], E, T1) :- H >= E, filterSmaller(T, E, T1).

filterLarger([], _, []).
filterLarger([H | T], E, [H | T1]) :- H > E, filterLarger(T, E, T1).
filterLarger([H | T], E, T1) :- H <= E, filterLarger(T, E, T1).

filterEqual([], _, []).
filterEqual([H | T], E, [H | T1]) :- H = E, filterEqual(T, E, T1).
filterEqual([H | T], E, T1) :- H \= E, filterEqual(T, E, T1).

quickSort([],[]).
quickSort([H | T], S) :- 
    L = [H | T],
    filterSmaller(L, H, Left),
    filterLarger(L, H, Right),
    filterEqual(L, H, Middle),

    quickSort(Left, SLeft),
    quickSort(Right, SRight),
    append(SLeft, Middle, LM),
    append(LM, SRight, S).

?- quickSort([2, 1, 1, 1, 2, 2, 3, 5, 0], S).