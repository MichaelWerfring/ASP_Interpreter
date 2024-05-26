// <copyright file="IBinaryTermCaseVisitor.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;

/// <summary>
/// An interface for a binary term case visitor, basically a double visitor.
/// </summary>
public interface IBinaryTermCaseVisitor
{
    /// <summary>
    /// Visits an integer-integer case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(IntegerIntegerCase binaryCase);

    /// <summary>
    /// Visits an integer-structure case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(IntegerStructureCase binaryCase);

    /// <summary>
    /// Visits an integer-variable case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(IntegerVariableCase binaryCase);

    /// <summary>
    /// Visits an structure-integer case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(StructureIntegerCase binaryCase);

    /// <summary>
    /// Visits an structure-structure case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(StructureStructureCase binaryCase);

    /// <summary>
    /// Visits an structure-variable case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(StructureVariableCase binaryCase);

    /// <summary>
    /// Visits an variable-integer case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(VariableIntegerCase binaryCase);

    /// <summary>
    /// Visits an variable-variable case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(VariableVariableCase binaryCase);

    /// <summary>
    /// Visits an variable-structure case.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    public void Visit(VariableStructureCase binaryCase);
}

/// <summary>
/// An interface for a binary term case visitor, basically a double visitor, that also returns a value.
/// </summary>
public interface IBinaryTermCaseVisitor<T>
{
    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(IntegerIntegerCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(IntegerStructureCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(IntegerVariableCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(StructureIntegerCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(StructureStructureCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(StructureVariableCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(VariableIntegerCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(VariableVariableCase binaryCase);

    /// <summary>
    /// Visits a binary case and returns a value.
    /// </summary>
    /// <param name="binaryCase">The case to visit.</param>
    /// <returns>A value of type <typeparamref name="T"/>.</returns>
    public T Visit(VariableStructureCase binaryCase);
}