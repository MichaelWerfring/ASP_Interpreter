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

    public VariableMapping(ImmutableDictionary<Variable, IVariableBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if (mapping.KeyComparer is not VariableComparer)
        {
            throw new ArgumentException("Must contain the correct comparer.",nameof(mapping));
        }

        this.mapping = mapping;
    }

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
    public IVariableBinding this[Variable key] => this.mapping[key];

    public int Count => this.mapping.Count;

    public IEnumerable<Variable> Keys => this.mapping.Keys;

    public IEnumerable<IVariableBinding> Values => this.mapping.Values;

    public VariableMapping Add(Variable key, IVariableBinding value) => new(this.mapping.Add(key, value));

    public VariableMapping AddRange(IEnumerable<KeyValuePair<Variable, IVariableBinding>> pairs) => new(this.mapping.AddRange(pairs));

    public VariableMapping Clear() => new(this.mapping.Clear());

    public VariableMapping Remove(Variable key) => new(this.mapping.Remove(key));

    public VariableMapping RemoveRange(IEnumerable<Variable> keys) => new(this.mapping.RemoveRange(keys));

    public VariableMapping SetItem(Variable key, IVariableBinding value) => new(this.mapping.SetItem(key, value));

    public VariableMapping SetItems(IEnumerable<KeyValuePair<Variable, IVariableBinding>> items) => new(this.mapping.SetItems(items));

    public bool Contains(KeyValuePair<Variable, IVariableBinding> pair) => this.mapping.Contains(pair);

    public bool ContainsKey(Variable key) => this.mapping.ContainsKey(key);

    public bool TryGetKey(Variable equalKey, out Variable actualKey) => this.mapping.TryGetKey(equalKey, out actualKey);

    public bool TryGetValue(Variable key, [MaybeNullWhen(false)] out IVariableBinding value) => this.mapping.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<Variable, IVariableBinding>> GetEnumerator() => this.mapping.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.mapping.GetEnumerator();
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Add(Variable key, IVariableBinding value)
    {
       return this.Add(key, value);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.AddRange(IEnumerable<KeyValuePair<Variable, IVariableBinding>> pairs)
    {
        return this.AddRange(pairs);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Clear()
    {
        return this.Clear();
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Remove(Variable key)
    {
        return this.Remove(key);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.RemoveRange(IEnumerable<Variable> keys)
    {
        return this.RemoveRange(keys);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.SetItem(Variable key, IVariableBinding value)
    {
       return this.SetItem(key, value);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.SetItems(IEnumerable<KeyValuePair<Variable, IVariableBinding>> items)
    {
        return this.SetItems(items);
    }

    public override string ToString()
    {
        return this.mapping.Count > 0 ? $"{{{this.mapping.ToList().ListToString()}}}" : "Empty Mapping";
    }
}