grammar Lambda;

input
   : value EOF
   ;

tuple
   : '(' value (',' value)* ')'
   ;

arr
   : '[' value (',' value)* ']'
   | '[' ']'
   ;

value
   : STRING
   | CHAR
   | REAL
   | INTEGER
   | tuple
   | arr
   | 'true'
   | 'false'
   | 'null'
   ;


STRING
   : '"' (ESC | SAFECODEPOINT)* '"'
   ;

CHAR
   : '\'' (ESC | SAFECODEPOINT)* '\''
   ;


fragment ESC
   : '\\' (["\\/bfnrt] | UNICODE)
   ;
fragment UNICODE
   : 'u' HEX HEX HEX HEX
   ;
fragment HEX
   : [0-9a-fA-F]
   ;
fragment SAFECODEPOINT
   : ~ ["\\\u0000-\u001F]
   ;

INTEGER
   : '-'? INT
   ;

REAL
   : INTEGER ('.' [0-9] +)?
   ;


fragment INT
   : '0' | [1-9] [0-9]*
   ;

WS
   : [ \t\n\r] + -> skip
   ;