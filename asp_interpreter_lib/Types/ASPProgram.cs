//-----------------------------------------------------------------------
// <copyright file="ASPProgram.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Text;

public class AspProgram : IVisitableType
{
    private List<Statement> statements;
    private IOption<Query> query;
    private Dictionary<(string, int), Explanation> explainations;

    public AspProgram(List<Statement> statements, IOption<Query> query, Dictionary<(string, int), Explanation> explanations)
    {
        this.Statements = statements;
        this.Query = query;
        this.Explanations = explanations;
    }

    public List<Statement> Statements
    {
        get => this.statements;
        private set => this.statements = value ?? throw new ArgumentNullException(nameof(this.Statements));
    }

    public IOption<Query> Query
    {
        get => this.query;
        private set => this.query = value ?? throw new ArgumentNullException(nameof(this.Query));
    }

    public Dictionary<(string, int), Explanation> Explanations
    {
        get => this.explainations;
        private set => this.explainations = value ?? throw new ArgumentNullException(nameof(this.Explanations));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = new StringBuilder();
        foreach (var statement in this.Statements)
        {
            builder.Append(statement.ToString());
            builder.AppendLine();
        }

        if (this.Query.HasValue)
        {
            builder.Append(this.Query.GetValueOrThrow().ToString());
        }
        builder.AppendLine();

        return builder.ToString();
    }

    /// <inheritdoc/>
    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}