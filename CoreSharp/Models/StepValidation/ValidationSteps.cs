using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.Models.StepValidation
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ValidationSteps : ValidationStepCollectionBase
    {
        //Constructors
        public ValidationSteps(IEnumerable<ValidationStep> steps, bool sequentialValidation = true) : this(sequentialValidation)
        {
            _ = steps ?? throw new ArgumentNullException(nameof(steps));

            foreach (var step in steps)
                Add(step);
        }

        public ValidationSteps(bool sequentialValidation = true)
            => SequentialValidation = sequentialValidation;

        //Properties 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => ToString();

        /// <summary>
        /// Steps have to be sequantially valid.
        /// If Step-1 is not valid, then Step-2 cannot be valid as well.
        /// </summary>
        public bool SequentialValidation { get; set; } = true;

        //Methods 
        public override string ToString() => $"{Count} steps";

        public void Add(int number, Func<bool> validationFunction)
            => Add(number, validationFunction, () => string.Empty);

        public void Add(int number, Func<bool> validationFunction, Func<string> validationMessageFunction)
            => Add(new(number, validationFunction, validationMessageFunction));

        public bool Remove(int number)
            => Remove(GetStep(number));

        public bool IsStepValid(int number)
        {
            var currentStep = GetStep(number);
            if (SequentialValidation)
            {
                var previousStep = GetPreviousStep(number);

                //If there is no previous step, run current step validation 
                if (previousStep is null)
                    return currentStep.IsValid;

                //Else run recursively previous step validation and then the current step 
                else
                    return IsStepValid(previousStep.Number) && currentStep.IsValid;
            }
            else
            {
                return currentStep.IsValid;
            }
        }

        public string GetStepValidationMessage(int number, bool bypassValidation = false)
        {
            var step = GetStep(number);

            if (bypassValidation)
                return step.ValidationMessage;
            else if (!step.IsValid)
                return step.ValidationMessage;
            else
                return string.Empty;
        }

        public ValidationStep GetStep(int number)
        {
            ValidationStep.ValidateNumber(number);

            var step = this.FirstOrDefault(s => s.Number == number);
            if (step is null)
                throw new ArgumentException($"{nameof(ValidationStep)} with {nameof(ValidationStep.Number)}=`{number}` not found.", nameof(number));
            else
                return step;
        }

        private ValidationStep GetPreviousStep(int number)
        {
            ValidationStep.ValidateNumber(number);

            return this.OrderByDescending(s => s.Number)
                       .FirstOrDefault(s => s.Number < number);
        }
    }
}
