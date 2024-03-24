grammar ASP;
program : statements query;
query : classical_literal QUERY_MARK;

//maybe use plus 
statements : statement*;

statement 
    : CONS body? DOT 
    | head (CONS body?)? DOT;

head : disjunction | choice;
//body : naf_literal (COMMA body)?;
body : naf_literal (COMMA naf_literal)*;

disjunction : classical_literal (OR classical_literal)*;
choice: (term binop)? CURLY_OPEN choice_elements? CURLY_CLOSE (binop term)?;

choice_elements : choice_element (SEMICOLON choice_elements)?;
choice_element : classical_literal (COLON naf_literals?)?;

naf_literals :  naf_literal (COMMA naf_literals)?;
naf_literal : NAF? classical_literal | builtin_atom;

classical_literal : MINUS? ID (PAREN_OPEN terms? PAREN_CLOSE)?;
builtin_atom : term binop term;

binop 
    : EQUAL             #equalityOperation
    | UNEQUAL           #unequalityOperation
    | LESS              #lessOperation
    | GREATER           #greaterOperation
    | LESS_OR_EQ        #lessOrEqOperation
    | GREATER_OR_EQ     #greaterOrEqOperation
    // Find proper place for disunification
    | DISUNIFICATION    #disunificationOperation;

//terms : (terms COMMA)? term;
terms : term (COMMA terms)?;
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
//Might be obsolete due to DISUNIFICATION
UNEQUAL : '!='|'<>';
LESS : '<';
GREATER : '>';
LESS_OR_EQ : '<=';
GREATER_OR_EQ : '>=';
DISUNIFICATION : '\\=';

//had to put this down, because id matched instead of naf
ID : [a-z][a-zA-Z0-9_]*;
VARIABLE : [A-Z][a-zA-Z0-9_]*;

// ~ means match evereything except ...
COMMENT		: '%' ~[\r?\n]* [\r?\n] -> skip;
MULTI_LINE_COMMENT : '%*' .*? '*%' -> skip;
BLANK : [ \t\n\r]+ -> skip;
NEWLINE		: [\r?\n] -> skip ;
TAB			: '\t' -> skip ;
WS			: ' ' -> skip ;