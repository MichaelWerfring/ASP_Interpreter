// <copyright file="CannotDisunifyException.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

/// <summary>
/// A class that represents an exception that indicates that disunification failed.
/// </summary>
public class CannotDisunifyException : DisunificationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CannotDisunifyException"/> class.
    /// </summary>
    /// <param name="message">The specific message to this exception instance.</param>
    public CannotDisunifyException(string message)
        : base(message)
    {
    }
}