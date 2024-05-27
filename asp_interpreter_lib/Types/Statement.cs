//-----------------------------------------------------------------------
// <copyright file="Statement.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Text;

public class Statement : IVisitableType
{
    public bool HasBody => this.Body.Count != 0;

    public bool HasHead => this.Head.HasValue;

    // Empty per default
    public IOption<Literal> Head { get; set; } = new None<Literal>();

    public List<Goal> Body { get; private set; } = new([]);

    public void AddHead(Literal head)
    {
        ArgumentNullException.ThrowIfNull(head);
        if (this.HasHead)
        {
            throw new ArgumentException("A statement can only have one head");
        }

        this.Head = new Some<Literal>(head);
    }

    public void AddBody(List<Goal> body)
    {
        ArgumentNullException.ThrowIfNull(body);
        if (this.HasBody)
        {
            throw new ArgumentException("A statement can only have one body");
        }

        this.Body = body;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        if (this.HasBody && this.HasHead)
        {
            return $"{this.Head.GetValueOrThrow().ToString()} :- {this.GetBodyAsString()}.";
        }

        if (this.HasBody && !this.HasHead)
        {
            return ":- " + this.GetBodyAsString() + ".";
        }

        if (!this.HasBody && !this.HasHead)
        {
            return string.Empty;
        }

        return $"{this.Head.GetValueOrThrow().ToString()}.";
    }

    private string GetBodyAsString()
    {
        var builder = new StringBuilder();

        if (this.Body.Count < 1)
        {
            return string.Empty;
        }

        builder.Append(this.Body[0].ToString());
        for (var index = 1; index < this.Body.Count; index++)
        {
            var goal = this.Body[index];
            builder.Append(", ");
            builder.Append(goal.ToString());
        }

        return builder.ToString();
    }

    /// <inheritdoc/>
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}