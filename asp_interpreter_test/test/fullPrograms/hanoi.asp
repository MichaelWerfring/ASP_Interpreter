member(H, [H | _]).
member(H, [X | T]) :- H \= X, member(H, T).

hanoi(1, Start, Goal, InputMoveNum, NextMoveNum) :- 
    move(InputMoveNum, Start, Goal),
    NextMoveNum is InputMoveNum + 1.

hanoi(N, Start, Goal, InputMoveNum, NextMoveNum) :- 
    N > 1,
    N1 is N - 1,
    getOther(Start, Goal, Other),
    hanoi(N1, Start, Other, InputMoveNum, NextMoveNum1), 
    hanoi(1, Start, Goal, NextMoveNum1, NextMoveNum2),
    hanoi(N1, Other, Goal, NextMoveNum2, NextMoveNum).

move(MoveN, Start, Goal).

getOther(Start, Goal, Other) :- 
    member(Other, [1, 2, 3]),
    Other \= Start,
    Other \= Goal.


hanoi_main(TowerCount, StartTower, GoalTower) :- hanoi(TowerCount, StartTower, GoalTower, 1, _).


?- hanoi_main(5, 1, 3).