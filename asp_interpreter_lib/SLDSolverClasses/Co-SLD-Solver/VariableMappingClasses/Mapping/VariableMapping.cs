// <copyright file="VariableMapping.cs" company="FHWN">
// Copyright (c) FHWN. All rights reserved.
// </copyright>

namespace Asp_interpreter_lib.Unification.Co_SLD.Binding.VariableMappingClasses;

using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.TermFunctions.Instances;
using Asp_interpreter_lib.InternalProgramClasses.SimpleTerm.Terms.Variables;
using Asp_interpreter_lib.SLDSolverClasses.Co_SLD_Solver.VariableMappingClasses.Binding;
using Asp_interpreter_lib.Util;
using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents a mapping for the co-sld-resolution:
/// Variables are mapped to either a prohibited value set or a single term.
/// This is a concretization of ImmutableDictionary to make sure that it always has the right Comparer:
/// All the methods just call the inner dictionary and wrap them in a NewMapping before returning.
/// VariableMapping implements IImmutableDictionary instead of wrapping one simply to make the calling code easier to read.
/// </summary>
public class VariableMapping : IImmutableDictionary<Variable, IVariableBinding>
{
    private readonly ImmutableDictionary<Variable, IVariableBinding> mapping;

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableMapping"/> class.
    /// </summary>
    public VariableMapping()
    {
        this.mapping = ImmutableDictionary.Create<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableMapping"/> class.
    /// </summary>
    /// <param name="mapping">The mapping to use.</param>
    /// <exception cref="ArgumentException">Thrown if comparer of <paramref name="mapping"/>
    /// is not of type <see cref="VariableComparer"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public VariableMapping(ImmutableDictionary<Variable, IVariableBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if (mapping.KeyComparer is not VariableComparer)
        {
            throw new ArgumentException("Must contain the correct comparer.", nameof(mapping));
        }

        this.mapping = mapping;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableMapping"/> class.
    /// </summary>
    /// <param name="mapping">The mapping to use.</param>
    /// <exception cref="ArgumentException">Thrown if comparer of <paramref name="mapping"/>
    /// is not of type <see cref="VariableComparer"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public VariableMapping(ImmutableDictionary<Variable, ProhibitedValuesBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if (mapping.KeyComparer is not VariableComparer)
        {
            throw new ArgumentException("Must contain the correct comparer.", nameof(mapping));
        }

        var immutableBuilder = ImmutableDictionary.CreateBuilder<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
        foreach (var pair in mapping)
        {
            immutableBuilder.Add(pair.Key, pair.Value);
        }

        this.mapping = immutableBuilder.ToImmutable();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariableMapping"/> class.
    /// </summary>
    /// <param name="mapping">The mapping to use.</param>
    /// <exception cref="ArgumentException">Thrown if comparer of <paramref name="mapping"/>
    /// is not of type <see cref="VariableComparer"/>.</exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="mapping"/> is null.</exception>
    public VariableMapping(ImmutableDictionary<Variable, TermBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if (mapping.KeyComparer is not VariableComparer)
        {
            throw new ArgumentException("Must contain the correct comparer.", nameof(mapping));
        }

        var immutableBuilder = ImmutableDictionary.CreateBuilder<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
        foreach (var pair in mapping)
        {
            immutableBuilder.Add(pair.Key, pair.Value);
        }

        this.mapping = immutableBuilder.ToImmutable();
    }

    /// <summary>
    /// Gets the key-value-pair count.
    /// </summary>
    public int Count => this.mapping.Count;

    /// <summary>
    /// Gets the keys.
    /// </summary>
    public IEnumerable<Variable> Keys => this.mapping.Keys;

    /// <summary>
    /// Gets the values.
    /// </summary>
    public IEnumerable<IVariableBinding> Values => this.mapping.Values;

    /// <summary>
    /// Gets the value at key <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The value that the key maps towards.</returns>
    /// <exception cref="Exception">Thrown if <paramref name="key"/> maps to no value.</exception>
    public IVariableBinding this[Variable key] => this.mapping[key];

    /// <summary>
    /// Adds a key and value to the mapping.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>A new mapping with the pair added.</returns>
    /// <exception cref="ArgumentException">Thrown if key already maps to a value.</exception>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="key"/> is null,
    /// ..<paramref name="value"/> is null.</exception>
    public VariableMapping Add(Variable key, IVariableBinding value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        return new(this.mapping.Add(key, value));
    }

    /// <summary>
    /// Adds a range of key-value-pairs.
    /// </summary>
    /// <param name="pairs">The pairs to add.</param>
    /// <returns>A mapping with the pair added to it.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pairs"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if any key already maps to a value.</exception>
    public VariableMapping AddRange(IEnumerable<KeyValuePair<Variable, IVariableBinding>> pairs)
    {
        ArgumentNullException.ThrowIfNull(pairs, nameof(pairs));

        return new(this.mapping.AddRange(pairs));
    }

    /// <summary>
    /// Returns an empty mapping.
    /// </summary>
    /// <returns>An empty mapping.</returns>
    public VariableMapping Clear()
    {
        return new(this.mapping.Clear());
    }

    /// <summary>
    /// Removes a key and its value.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <returns>A new mapping with the key removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
    public VariableMapping Remove(Variable key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        return new(this.mapping.Remove(key));
    }

    /// <summary>
    /// Removes a range of keys.
    /// </summary>
    /// <param name="keys">The keys to remove.</param>
    /// <returns>A new mapping with the keys removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="keys"/> is null.</exception>
    public VariableMapping RemoveRange(IEnumerable<Variable> keys)
    {
        ArgumentNullException.ThrowIfNull(keys, nameof(keys));

        return new(this.mapping.RemoveRange(keys));
    }

    /// <summary>
    /// Sets the value of a key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>A new mapping with the key and value added.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="key"/> is null,
    /// ..<paramref name="value"/> is null.</exception>
    public VariableMapping SetItem(Variable key, IVariableBinding value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        return new(this.mapping.SetItem(key, value));
    }

    /// <summary>
    /// Sets the specified key-value-pairs in the mapping.
    /// </summary>
    /// <param name="items">The items to set.</param>
    /// <returns>A new mapping with the items set.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is null.</exception>
    public VariableMapping SetItems(IEnumerable<KeyValuePair<Variable, IVariableBinding>> items)
    {
        ArgumentNullException.ThrowIfNull(items, nameof(items));

        return new(this.mapping.SetItems(items));
    }

    /// <summary>
    /// Checks if mapping contains a pair.
    /// </summary>
    /// <param name="pair">The pair to check for.</param>
    /// <returns>A value indicating whether the mapping contains <paramref name="pair"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pair"/> is null.</exception>
    public bool Contains(KeyValuePair<Variable, IVariableBinding> pair)
    {
        ArgumentNullException.ThrowIfNull(pair, nameof(pair));

        return this.mapping.Contains(pair);
    }

    /// <summary>
    /// Checks if mapping contains a key.
    /// </summary>
    /// <param name="key">The key to check for.</param>
    /// <returns>A value indicating whether the mapping contains <paramref name="key"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
    public bool ContainsKey(Variable key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        return this.mapping.ContainsKey(key);
    }

    /// <summary>
    /// Determines whether the mapping contains the specified key.
    /// </summary>
    /// <param name="equalKey">The key to check for.</param>
    /// <param name="actualKey">The actual key.</param>
    /// <returns>A value indicating whether the mapping has <paramref name="equalKey"/>.</returns>
    public bool TryGetKey(Variable equalKey, out Variable actualKey)
    {
        ArgumentNullException.ThrowIfNull(equalKey, nameof(equalKey));

        return this.mapping.TryGetKey(equalKey, out actualKey);
    }

    /// <summary>
    /// Determines whether the mapping a value for the specified key.
    /// </summary>
    /// <param name="key">The key to check for.</param>
    /// <param name="value">The value.</param>
    /// <returns>A value indicating whether the mapping has a value for <paramref name="key"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
    public bool TryGetValue(Variable key, [MaybeNullWhen(false)] out IVariableBinding value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        return this.mapping.TryGetValue(key, out value);
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<KeyValuePair<Variable, IVariableBinding>> GetEnumerator()
    {
        return this.mapping.GetEnumerator();
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.mapping.GetEnumerator();
    }

    /// <summary>
    /// Adds a key and value to the mapping.
    /// </summary>
    /// <param name="key">The key to add.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>A new mapping with the pair added.</returns>
    /// <exception cref="ArgumentException">Thrown if key already maps to a value.</exception>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="key"/> is null,
    /// ..<paramref name="value"/> is null.</exception>
    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Add(Variable key, IVariableBinding value)
    {
       return this.Add(key, value);
    }

    /// <summary>
    /// Adds a range of key-value-pairs.
    /// </summary>
    /// <param name="pairs">The pairs to add.</param>
    /// <returns>A mapping with the pair added to it.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pairs"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if any key already maps to a value.</exception>
    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.AddRange(IEnumerable<KeyValuePair<Variable, IVariableBinding>> pairs)
    {
        return this.AddRange(pairs);
    }

    /// <summary>
    /// Returns an empty mapping.
    /// </summary>
    /// <returns>An empty mapping.</returns>
    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Clear()
    {
        return this.Clear();
    }

    /// <summary>
    /// Removes a key and its value.
    /// </summary>
    /// <param name="key">The key to remove.</param>
    /// <returns>A new mapping with the key removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="key"/> is null.</exception>
    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Remove(Variable key)
    {
        return this.Remove(key);
    }

    /// <summary>
    /// Removes a range of keys.
    /// </summary>
    /// <param name="keys">The keys to remove.</param>
    /// <returns>A new mapping with the keys removed.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="keys"/> is null.</exception>
    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.RemoveRange(IEnumerable<Variable> keys)
    {
        return this.RemoveRange(keys);
    }

    /// <summary>
    /// Sets the value of a key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>A new mapping with the key and value added.</returns>
    /// <exception cref="ArgumentNullException">Thrown if..
    /// ..<paramref name="key"/> is null,
    /// ..<paramref name="value"/> is null.</exception>
    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.SetItem(Variable key, IVariableBinding value)
    {
       return this.SetItem(key, value);
    }

    /// <summary>
    /// Sets the specified key-value-pairs in the mapping.
    /// </summary>
    /// <param name="items">The items to set.</param>
    /// <returns>A new mapping with the items set.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is null.</exception>
    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.SetItems(IEnumerable<KeyValuePair<Variable, IVariableBinding>> items)
    {
        return this.SetItems(items);
    }

    /// <summary>
    /// Converts the mapping to a string representation.
    /// </summary>
    /// <returns>The string representation.</returns>
    public override string ToString()
    {
        return this.mapping.Count > 0 ? $"{{{this.mapping.ToList().ListToString()}}}" : "Empty Mapping";
    }
}