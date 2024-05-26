// <copyright file="VariableDisunificationException.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Disunification.Exceptions;

/// <summary>
/// A class that represents an exception during disunification
/// due to both arguments of a disunifcation being variables.
/// </summary>
public class VariableDisunificationException : DisunificationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VariableDisunificationException"/> class.
    /// </summary>
    /// <param name="message">The specific message to this exception instance.</param>
    public VariableDisunificationException(string message)
        : base(message)
    {
    }
}