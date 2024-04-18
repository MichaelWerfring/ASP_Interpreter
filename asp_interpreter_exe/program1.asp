test(X) :- X is 10 + 2.
test(X) :- X is 10 - 2.
test(X) :- X is 10 * 2.
test(X) :- X is 10 / 2.
test(X) :- X is 10 ** 2.

test(X) :- X = 10.
test(X) :- X = 10 + 2.

test(X) :- X \= 11.
test(X) :- X \= 11 + 2.

test(X) :- X = 1, X < 2.
test(X) :- X = 1, X <= 1.
test(X) :- X = 2, X > 1.
test(X) :- X = 2, X >= 2.

append([], L, L).
append([H | T], L, [H | T1]) :- append(T, L, T1).

eliza(Stimuli, Response) :-
    template(InternalStimuli, InternalResponse),
    match(InternalStimuli, Stimuli),
    match(InternalResponse, Response).

template([s([i,am]),s(X)], [s([why,are,you]),s(X),w(questionMark)]).
template([w(i),s(X),w(you)], [s([why,do,you]),s(X),w(me),w(questionMark)]).

match([],[]).
match([Item|Items],[Word|Words]) :-
    match(Item, Items, Word, Words).

match(w(Word), Items, Word, Words) :-
    match(Items, Words).
match(s([Word|Seg]), Items, Word, Words0) :-
    append(Seg, Words1, Words0),
    match(Items, Words1).

eliza([i, am, very, hungry], Response)?.
