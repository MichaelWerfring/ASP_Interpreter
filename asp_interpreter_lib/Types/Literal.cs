//-----------------------------------------------------------------------
// <copyright file="Literal.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Text;

public class Literal : Goal
{
    private List<ITerm> terms;
    private string identifier;

    public Literal(string identifier, bool hasNafNegation, bool hasStrongNegation, List<ITerm> terms)
    {
        this.Identifier = identifier;
        this.Terms = terms;
        this.HasStrongNegation = hasStrongNegation;
        this.HasNafNegation = hasNafNegation;
    }

    public List<ITerm> Terms
    {
        get => this.terms;
        set => this.terms = value ?? throw new ArgumentNullException(nameof(this.Terms));
    }

    // Negated in this context means classical negation
    public bool HasStrongNegation { get; set; }

    public bool HasNafNegation { get; set; }

    public string Identifier
    {
        get => this.identifier;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || value == string.Empty)
            {
                throw new ArgumentException(
                    "The given Identifier must not be null, whitespace or empty!",
                    nameof(this.Identifier));
            }

            this.identifier = value;
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = new StringBuilder();

        if (this.HasNafNegation)
        {
            builder.Append("not ");
        }

        if (this.HasStrongNegation)
        {
            builder.Append('-');
        }

        builder.Append(this.Identifier);

        if (this.Terms.Count > 0)
        {
            builder.Append('(');
            builder.Append(this.Terms.ListToString());
            builder.Append(')');
        }

        return builder.ToString();
    }

    /// <inheritdoc/>
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}