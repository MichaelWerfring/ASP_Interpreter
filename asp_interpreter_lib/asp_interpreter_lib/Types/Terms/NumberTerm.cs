﻿namespace asp_interpreter_lib.Types.Terms;

public class NumberTerm: Term
{
    public NumberTerm(int value)
    {
        Value = value;
    }
    
    //according to the grammar, the value
    //of a number term allows only integer
    public int Value { get; set; }
}