//-----------------------------------------------------------------------
// <copyright file="Query.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util;
using Asp_interpreter_lib.Util.ErrorHandling;

public class Query
{
    private List<Goal> goals;

    /// <summary>
    /// Initializes a new instance of the <see cref="Query"/> class.
    /// </summary>
    /// <param name="goals"></param>
    public Query(List<Goal> goals)
    {
        this.goals = goals;
    }

    public List<Goal> Goals
    {
        get => this.goals;
        private set => this.goals = value ?? throw new ArgumentNullException(nameof(this.Goals));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"?- {AspExtensions.ListToString(this.Goals)}.";
    }

    public IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));
        return visitor.Visit(this);
    }
}