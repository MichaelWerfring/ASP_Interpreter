append(nil, L, L).
append([H | T], L ,[H|T1]) :- append(T,L, T1).

append(X, Y, [1,2,3,4,5])?.
