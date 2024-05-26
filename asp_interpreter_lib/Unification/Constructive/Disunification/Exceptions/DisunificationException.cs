// <copyright file="DisunificationException.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

/// <summary>
/// A class that represents an exception that occurred during disunification.
/// </summary>
public abstract class DisunificationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisunificationException"/> class.
    /// </summary>
    /// <param name="message">The specific message to this exception instance.</param>
    public DisunificationException(string message)
        : base(message)
    {
    }
}