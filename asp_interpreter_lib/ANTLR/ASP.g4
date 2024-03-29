grammar ASP;
program : statements query;
query : classical_literal QUERY_MARK;

//also allow empty programs
statements : statement*;

statement 
    : CONS body? DOT 
    | head (CONS body?)? DOT;

head: classical_literal;
body : naf_literal (COMMA naf_literal)*;

naf_literals :  naf_literal (COMMA naf_literals)?;
naf_literal : NAF? classical_literal | binary_operation;

classical_literal : MINUS? ID (PAREN_OPEN terms? PAREN_CLOSE)?;
binary_operation : term binary_operator term;

binary_operator
    : EQUAL             #equalityOperation
    | DISUNIFICATION    #disunificationOperation
    | LESS              #lessOperation
    | GREATER           #greaterOperation
    | LESS_OR_EQ        #lessOrEqOperation
    | GREATER_OR_EQ     #greaterOrEqOperation;

terms : term (COMMA terms)?;
//terms : term (COMMA term)*;
term 
    : ID (PAREN_OPEN terms? PAREN_CLOSE)?   #basicTerm
    | NUMBER                                #numberTerm
    | STRING                                #stringTerm
    | VARIABLE                              #variableTerm
    | ANONYMOUS_VARIABLE                    #anonymousVariableTerm
    | PAREN_OPEN term PAREN_CLOSE           #parenthesizedTerm
    | MINUS term                            #negatedTerm
    | term arithop term                     #arithmeticOperationTerm;

arithop 
    : PLUS
    | MINUS
    | TIMES
    | DIV;


//escaping the " and then match everything except " 
STRING : '"'([^"]|'"')*'"';
NUMBER : [0] | [1-9]+;
ANONYMOUS_VARIABLE : '_';
DOT : '.';
COMMA : ',';
QUERY_MARK : '?';
COLON : ':';
SEMICOLON : ';';
OR : '|';
NAF : 'not';
CONS : ':-';
PLUS : '+';
MINUS : '-';
TIMES : '*';
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

//put this down so it does not match the not token
ID : [a-z][a-zA-Z0-9_]*;
VARIABLE : [A-Z][a-zA-Z0-9_]*;

// ~ means match evereything except ...
COMMENT		: '%' ~[\r?\n]* [\r?\n] -> skip;
MULTI_LINE_COMMENT : '%*' .*? '*%' -> skip;
BLANK : [ \t\n\r]+ -> skip;
NEWLINE		: [\r?\n] -> skip ;
TAB			: '\t' -> skip ;
WS			: ' ' -> skip ;