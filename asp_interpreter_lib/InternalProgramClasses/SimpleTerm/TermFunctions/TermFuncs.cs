// <copyright file="TermFuncs.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.CaseDetermination.Cases;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.ClauseRenamer;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances.Typechecking;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Interface;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Structures;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.Util.ErrorHandling;

/// <summary>
/// A static class containing frequently used functions related to terms.
/// </summary>
public static class TermFuncs
{
    private static readonly ClauseVariableRenamer Renamer = new(new());
    private static readonly StructureReducer Reducer = new();
    private static readonly VariableComparer VariableComparer = new();
    private static readonly SimpleTermComparer TermComparer = new();
    private static readonly TermCaseDeterminer CaseDeterminer = new();
    private static readonly IntegerChecker IntChecker = new();
    private static readonly VariableChecker VarChecker = new();
    private static readonly StructureChecker StructChecker = new();

    /// <summary>
    /// Tries to reduce the term and returns a zipping of their children, or none.
    /// </summary>
    /// <param name="structure">The left term.</param>
    /// <param name="other">The right term.</param>
    /// <returns>A zipping of the children of the input terms, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public static IOption<IEnumerable<(ISimpleTerm LeftChild, ISimpleTerm RightChild)>> Reduce(IStructure structure, IStructure other)
    {
        ArgumentNullException.ThrowIfNull(structure);
        ArgumentNullException.ThrowIfNull(other);

        return Reducer.TryReduce(structure, other);
    }

    /// <summary>
    /// Renames the variables inside a clause.
    /// </summary>
    /// <param name="clause">The clause to rename.</param>
    /// <param name="nextInternalIndex">The next variable index to use.</param>
    /// <returns>A renaming result that contains the renamed clause and the next variable index.</returns>
    /// <exception cref="ArgumentNullException">Thrown if clause is null.</exception>
    public static RenamingResult RenameClause(IEnumerable<Structure> clause, int nextInternalIndex)
    {
        ArgumentNullException.ThrowIfNull(clause);

        return Renamer.RenameVariables(clause, nextInternalIndex);
    }

    /// <summary>
    /// Determines the concrete type case of two input terms.
    /// </summary>
    /// <param name="left">The left term.</param>
    /// <param name="right">The right term.</param>
    /// <returns>The binary case of the two types.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// .. left is null.
    /// .. right is null.</exception>
    public static IBinaryTermCase DetermineCase(ISimpleTerm left, ISimpleTerm right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return CaseDeterminer.DetermineCase(left, right);
    }

    /// <summary>
    /// Returns the term as a variable, or none.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The term as a variable, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public static IOption<Variable> ReturnVariableOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return VarChecker.ReturnVariableOrNone(term);
    }

    /// <summary>
    /// Returns the term as an integer, or none.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The term as an integer, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public static IOption<Integer> ReturnIntegerOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return IntChecker.ReturnIntegerOrNone(term);
    }

    /// <summary>
    /// Returns the term as a structure, or none.
    /// </summary>
    /// <param name="term">The term to convert.</param>
    /// <returns>The term as a structure, or none.</returns>
    /// <exception cref="ArgumentNullException">Thrown if term is null.</exception>
    public static IOption<Structure> ReturnStructureOrNone(ISimpleTerm term)
    {
        ArgumentNullException.ThrowIfNull(term);

        return StructChecker.ReturnStructureOrNone(term);
    }

    /// <summary>
    /// Gets a singleton variable comparer instance.
    /// VariableComparer works by hashing so its faster than regular Comparer.
    /// </summary>
    /// <returns>A singleton variable comparer instance.</returns>
    public static VariableComparer GetSingletonVariableComparer()
    {
        return VariableComparer;
    }

    /// <summary>
    /// Gets a singleton term comparer instance.
    /// </summary>
    /// <returns>A singleton term comparer instance.</returns>
    public static SimpleTermComparer GetSingletonTermComparer()
    {
        return TermComparer;
    }
}