//-----------------------------------------------------------------------
// <copyright file="BasicTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Text;

public class BasicTerm : ITerm
{
    private string identifier;
    private List<ITerm> terms;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicTerm"/> class.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="terms"></param>
    public BasicTerm(string identifier, List<ITerm> terms)
    {
        this.Identifier = identifier;
        this.Terms = terms;
    }

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

    public List<ITerm> Terms
    {
        get => this.terms;
        set => this.terms = value ?? throw new ArgumentNullException(nameof(this.Terms));
    }

    /// <inheritdoc/>
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(this.Identifier);

        if (this.Terms.Count > 0)
        {
            builder.Append('(');
            builder.Append(this.Terms.ListToString());
            builder.Append(')');
        }

        return builder.ToString();
    }
}