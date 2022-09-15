using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Enumerables.Validations;

public abstract class ValidationStepCollectionBase : ICollection<ValidationStep>
{
    // Fields 
    private readonly SortedList<int, ValidationStep> _source = new();

    // Properties 
    public int Count => _source.Count;

    public bool IsReadOnly => false;

    // Methods
    public void Add(ValidationStep item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        ValidationStep.ValidateNumber(item.Number);

        if (!Contains(item))
            _source.Add(item.Number, item);
    }

    public bool Remove(ValidationStep item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        ValidationStep.ValidateNumber(item.Number);

        if (!Contains(item))
            return false;
        return _source.Remove(item.Number);
    }

    public void Clear()
        => _source.Clear();

    public bool Contains(ValidationStep item)
    {
        _ = item ?? throw new ArgumentNullException(nameof(item));
        ValidationStep.ValidateNumber(item.Number);

        return _source.ContainsKey(item.Number);
    }

    public void CopyTo(ValidationStep[] array, int arrayIndex)
    {
        _ = array ?? throw new ArgumentNullException(nameof(array));
        _source
            .Select(s => s.Value)
            .ToArray()
            .CopyTo(array, arrayIndex);
    }

    public IEnumerator<ValidationStep> GetEnumerator()
        => _source
            .Select(s => s.Value)
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _source.GetEnumerator();
}
