parser grammar ASPParser;
options { tokenVocab=ASPLexer; }
program : statements query?;
query : QUERY_SYMBOL goal(COMMA goal)* DOT;

statements : (explaination? statement)*;
statement 
    : CONS goal (COMMA goal)* DOT 
    | literal (CONS (goal (COMMA goal)*))? DOT;

explaination : literal EXP_OPEN exp EXP_CLOSE ;
exp : EXP_TEXT (EXP_VAR_OPEN EXP_VAR EXP_VAR_CLOSE|EXP_TEXT)* ;

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