lexer grammar ASPLexer;
//escaping the " and then match everything except " 
STRING : '"' ~[\\"]+ '"';
SHOW: '#show';
EXP_OPEN : EXP -> pushMode(EXP_MODE);
EXP: '::';
NUMBER :  [0] | [1-9][0-9]*;
ANONYMOUS_VARIABLE : '_';
DOT : '.';
COMMA : ',';
QUERY_SYMBOL : '?-';
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

// ~ means match everything except ...
COMMENT		: '%' ~[\r?\n]* [\r?\n] -> skip;
MULTI_LINE_COMMENT : '%*' .*? '*%' -> skip;
NEWLINE		: [\r?\n] -> skip ;
//TAB			: '\t' -> skip ;
WS : [ \t\r\n]+ -> skip ;

mode EXP_MODE;
EXP_VAR: [A-Z][a-zA-Z0-9_]*;
EXP_TEXT : ~[:@().\n]+;
EXP_CLOSE : DOT -> popMode;
EXP_VAR_OPEN : '@(';
EXP_VAR_CLOSE : ')';