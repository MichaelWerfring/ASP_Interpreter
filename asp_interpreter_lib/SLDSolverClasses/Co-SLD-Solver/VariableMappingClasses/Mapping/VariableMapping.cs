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
    private readonly ImmutableDictionary<Variable, IVariableBinding> _mapping;

    public VariableMapping()
    {
        _mapping = ImmutableDictionary.Create<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
    }

    public VariableMapping(ImmutableDictionary<Variable, IVariableBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if(mapping.KeyComparer is not VariableComparer)
        {
            throw new ArgumentException("Must contain the correct comparer.",nameof(mapping));
        }

        _mapping = mapping;
    }

    public VariableMapping(ImmutableDictionary<Variable, ProhibitedValuesBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if (mapping.KeyComparer is not VariableComparer)
        {
            throw new ArgumentException("Must contain the correct comparer.", nameof(mapping));
        }
    
        var immutableBuilder = 
            ImmutableDictionary.CreateBuilder<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
        foreach (var pair in mapping)
        {
            immutableBuilder.Add(pair.Key, pair.Value);
        }

       _mapping = immutableBuilder.ToImmutable();
    }

    public VariableMapping(ImmutableDictionary<Variable, TermBinding> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping, nameof(mapping));
        if (mapping.KeyComparer is not VariableComparer)
        {
            throw new ArgumentException("Must contain the correct comparer.", nameof(mapping));
        }

        var immutableBuilder = 
            ImmutableDictionary.CreateBuilder<Variable, IVariableBinding>(TermFuncs.GetSingletonVariableComparer());
        foreach (var pair in mapping)
        {
            immutableBuilder.Add(pair.Key, pair.Value);
        }

        _mapping = immutableBuilder.ToImmutable();
    }

    public VariableMapping Add(Variable key, IVariableBinding value) => new(_mapping.Add(key, value));

    public VariableMapping AddRange(IEnumerable<KeyValuePair<Variable, IVariableBinding>> pairs) => new(_mapping.AddRange(pairs));

    public VariableMapping Clear() => new(_mapping.Clear());

    public VariableMapping Remove(Variable key) => new(_mapping.Remove(key));

    public VariableMapping RemoveRange(IEnumerable<Variable> keys) => new(_mapping.RemoveRange(keys));

    public VariableMapping SetItem(Variable key, IVariableBinding value) => new(_mapping.SetItem(key, value));

    public VariableMapping SetItems(IEnumerable<KeyValuePair<Variable, IVariableBinding>> items) => new(_mapping.SetItems(items));

    public IVariableBinding this[Variable key] => 
        _mapping[key];

    public IEnumerable<Variable> Keys => 
        _mapping.Keys;

    public IEnumerable<IVariableBinding> Values =>
        _mapping.Values;

    public int Count => 
        _mapping.Count;

    public bool Contains(KeyValuePair<Variable, IVariableBinding> pair) => 
        _mapping.Contains(pair);

    public bool ContainsKey(Variable key) => 
        _mapping.ContainsKey(key);

    public bool TryGetKey(Variable equalKey, out Variable actualKey) => _mapping.TryGetKey(equalKey, out actualKey);

    public bool TryGetValue(Variable key, [MaybeNullWhen(false)] out IVariableBinding value) => _mapping.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<Variable, IVariableBinding>> GetEnumerator() => 
        _mapping.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _mapping.GetEnumerator();
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Add(Variable key, IVariableBinding value)
    {
       return Add(key, value);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.AddRange(IEnumerable<KeyValuePair<Variable, IVariableBinding>> pairs)
    {
        return AddRange(pairs);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Clear()
    {
        return Clear();
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.Remove(Variable key)
    {
        return Remove(key);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.RemoveRange(IEnumerable<Variable> keys)
    {
        return RemoveRange(keys);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.SetItem(Variable key, IVariableBinding value)
    {
       return SetItem(key, value);
    }

    IImmutableDictionary<Variable, IVariableBinding> IImmutableDictionary<Variable, IVariableBinding>.SetItems(IEnumerable<KeyValuePair<Variable, IVariableBinding>> items)
    {
        return SetItems(items);
    }

    public override string ToString() 
    {
        return _mapping.Count > 0 ? $"{{{_mapping.ToList().ListToString()}}}" : "Empty Mapping";
    }
}
