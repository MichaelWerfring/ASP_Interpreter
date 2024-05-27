//-----------------------------------------------------------------------
// <copyright file="ConventionalList.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Michael Werfring</author>
// <author>Clemens Niklos</author>
//-----------------------------------------------------------------------

namespace Asp_interpreter_lib.Types.Terms;
using Asp_interpreter_lib.Types.TypeVisitors;
using Asp_interpreter_lib.Util.ErrorHandling;
using System.Text;

public class ConventionalList : ListTerm
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConventionalList"/> class.
    /// </summary>
    public ConventionalList()
    {
        this.Terms =[];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConventionalList"/> class.
    /// </summary>
    /// <param name="terms"></param>
    public ConventionalList(List<ITerm> terms)
    {
        this.Terms = terms;
    }

    public List<ITerm> Terms { get; private set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append("[");

        for (int i = 0; i < this.Terms.Count; i++)
        {
            sb.Append(this.Terms[i].ToString());
            if (i < this.Terms.Count - 1)
            {
                sb.Append(", ");
            }
        }

        sb.Append("]");

        return sb.ToString();
    }

    /// <inheritdoc/>
    public override IOption<T> Accept<T>(TypeBaseVisitor<T> visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}