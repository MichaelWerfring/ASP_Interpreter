﻿using asp_interpreter_lib.ListExtensions;
using asp_interpreter_lib.Types.Terms;
using asp_interpreter_lib.Types.TypeVisitors;
using System.Text;

namespace asp_interpreter_lib.Types;

public class ClassicalLiteral
{
    private List<Term> _terms;
    private string _identifier;

    public ClassicalLiteral(string identifier, bool negated, List<Term> terms) 
    {
        Identifier = identifier;
        Terms = terms;
        Negated = negated;
    }

    public List<Term> Terms
    {
        get => _terms;
        private set => _terms = value ?? throw new ArgumentNullException(nameof(Terms));
    }

    //Negated in this context means classical negation 
    public bool Negated { get; private set; }

    public string Identifier
    {
        get => _identifier;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty )
            {
                throw new ArgumentException("The given Identifier must not be null, whitespace or empty!",
                    nameof(Identifier));
            }

            _identifier = value;
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        if(Negated)
        {
            builder.Append('-');
        }

        builder.Append(Identifier);

        if (Terms.Count > 0)
        {
            builder.Append('(');
            builder.Append(Terms.ListToString());
            builder.Append(')');
        }

        return builder.ToString();
    }
}