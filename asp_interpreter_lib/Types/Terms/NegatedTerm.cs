//-----------------------------------------------------------------------
// <copyright file="NegatedTerm.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class NegatedTerm : ITerm
{
    private ITerm term;

    /// <summary>
    /// Initializes a new instance of the <see cref="NegatedTerm"/> class.
    /// </summary>
    /// <param name="term"></param>
    public NegatedTerm(ITerm term)
    {
        this.Term = term;
    }

    public ITerm Term
    {
        get => this.term;
        private set => this.term = value ?? throw new ArgumentNullException(nameof(this.Term), "Term cannot be null!");
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
        return "-" + this.Term.ToString();
    }
}