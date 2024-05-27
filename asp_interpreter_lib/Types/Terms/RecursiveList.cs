//-----------------------------------------------------------------------
// <copyright file="RecursiveList.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;

public class RecursiveList : ListTerm
{
    private readonly ITerm head;
    private readonly ITerm tail;

    public RecursiveList(ITerm head, ITerm tail)
    {
        ArgumentNullException.ThrowIfNull(head);
        ArgumentNullException.ThrowIfNull(tail);

        this.head = head;
        this.tail = tail;
    }

    public ITerm Head => this.head;

    public ITerm Tail => this.tail;

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{this.head.ToString()}| {this.tail.ToString()}]";
    }

    /// <inheritdoc/>
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}