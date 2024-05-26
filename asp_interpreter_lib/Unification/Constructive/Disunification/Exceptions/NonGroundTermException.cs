// <copyright file="NonGroundTermException.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

/// <summary>
/// A class that represents an exception during disunification due to a term being non-ground.
/// </summary>
public class NonGroundTermException : DisunificationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonGroundTermException"/> class.
    /// </summary>
    /// <param name="message">The specific message to this exception instance.</param>
    public NonGroundTermException(string message)
        : base(message)
    {
    }
}