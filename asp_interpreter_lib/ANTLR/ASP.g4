grammar ASP;
program : statements query?;
query : QUERY_SYMBOL goal(COMMA goal)* DOT;

statements : statement*;

statement 
    : CONS goal (COMMA goal)* DOT 
    | literal (CONS (goal (COMMA goal)*))? DOT;

goal : 
    literal
    | binary_operation;

binary_operation : term binary_operator term;
literal : NAF? MINUS? ID (PAREN_OPEN terms? PAREN_CLOSE)?;

binary_operator
    : EQUAL             #equalityOperation
    | DISUNIFICATION    #disunificationOperation
    | LESS              #lessOperation
    | GREATER           #greaterOperation
    | LESS_OR_EQ        #lessOrEqOperation
    | GREATER_OR_EQ     #greaterOrEqOperation
    | IS                #isOperation;

terms 
    : term (COMMA terms)?;


term 
    : ID (PAREN_OPEN terms? PAREN_CLOSE)?   #basicTerm
    | NUMBER                                #numberTerm
    | STRING                                #stringTerm
    | VARIABLE                              #variableTerm
    | ANONYMOUS_VARIABLE                    #anonymousVariableTerm
    | PAREN_OPEN term PAREN_CLOSE           #parenthesizedTerm
    | MINUS term                            #negatedTerm
    | list                                  #listTerm
    | term arithop term                     #arithmeticOperationTerm;

list: SQUARE_OPEN terms? SQUARE_CLOSE       #conventionalList
    | SQUARE_OPEN term OR term SQUARE_CLOSE #recursiveList;

arithop 
    : PLUS                                  #plusOperation
    | MINUS                                 #minusOperation
    | TIMES                                 #timesOperation
    | DIV                                   #divOperation
    | POW                                   #powerOperation;


//escaping the " and then match everything except " 
STRING : '"' ~[\\"]+ '"';
NUMBER :  [0] | [1-9][0-9]*;
ANONYMOUS_VARIABLE : '_';
DOT : '.';
COMMA : ',';
QUERY_SYMBOL : '?-';
COLON : ':';
SEMICOLON : ';';
OR : '|';
NAF : 'not';
CONS : ':-';
PLUS : '+';
MINUS : '-';
TIMES : '*';
POW : '**';
DIV : '/';
AT : '@';
PAREN_OPEN : '(';
PAREN_CLOSE : ')';
SQUARE_OPEN : '[';
SQUARE_CLOSE : ']';
CURLY_OPEN : '{';
CURLY_CLOSE : '}';
EQUAL : '=';
LESS : '<';
GREATER : '>';
LESS_OR_EQ : '<=';
GREATER_OR_EQ : '>=';
DISUNIFICATION : '\\=';
IS : 'is';

//put this down so it does not match the not and is token
ID : [a-z][a-zA-Z0-9_]*;
VARIABLE : [A-Z][a-zA-Z0-9_]*;

// ~ means match evereything except ...
COMMENT		: '%' ~[\r?\n]* [\r?\n] -> skip;
MULTI_LINE_COMMENT : '%*' .*? '*%' -> skip;
BLANK : [ \t\n\r]+ -> skip;
NEWLINE		: [\r?\n] -> skip ;
TAB			: '\t' -> skip ;
WS			: ' ' -> skip ;