// <copyright file="TargetBuildingException.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Constructive.Target.Builder;

/// <summary>
/// An exception class that represents an exception during target building.
/// </summary>
/// <param name="message">The message that explains the cause of the exception.</param>
public class TargetBuildingException(string message) : Exception(message)
{
}