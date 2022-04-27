using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CoreSharp.Enumerables.Validations
{
    public abstract class ValidationStepCollectionBase : ICollection<ValidationStep>
    {
        //Fields 
        private readonly SortedList<int, ValidationStep> _source = new();

        //Properties 
        public int Count => _source.Count;

        public bool IsReadOnly => false;

        //Methods
        public void Add(ValidationStep validationStep)
        {
            _ = validationStep ?? throw new ArgumentNullException(nameof(validationStep));
            ValidationStep.ValidateNumber(validationStep.Number);

            if (!Contains(validationStep))
                _source.Add(validationStep.Number, validationStep);
        }

        public bool Remove(ValidationStep validationStep)
        {
            _ = validationStep ?? throw new ArgumentNullException(nameof(validationStep));
            ValidationStep.ValidateNumber(validationStep.Number);

            if (!Contains(validationStep))
                return false;
            return _source.Remove(validationStep.Number);
        }

        public void Clear()
            => _source.Clear();

        public bool Contains(ValidationStep validationStep)
        {
            _ = validationStep ?? throw new ArgumentNullException(nameof(validationStep));
            ValidationStep.ValidateNumber(validationStep.Number);

            return _source.ContainsKey(validationStep.Number);
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
}
