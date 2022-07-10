using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CoreSharp.Enumerables.Validations;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ValidationSteps : ValidationStepCollectionBase
{
    //Constructors
    public ValidationSteps(IEnumerable<ValidationStep> steps, bool sequentialValidation = true)
        : this(sequentialValidation)
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
    public bool SequentialValidation { get; set; }

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
            return previousStep is null ? currentStep.IsValid : IsStepValid(previousStep.Number) && currentStep.IsValid;
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
        else
            return !step.IsValid ? step.ValidationMessage : string.Empty;
    }

    public ValidationStep GetStep(int number)
    {
        ValidationStep.ValidateNumber(number);

        var step = this.FirstOrDefault(s => s.Number == number);
        return step is null
            ? throw new ArgumentException($"{nameof(ValidationStep)} with {nameof(ValidationStep.Number)}=`{number}` not found.", nameof(number))
            : step;
    }

    private ValidationStep GetPreviousStep(int number)
    {
        ValidationStep.ValidateNumber(number);

        return this.OrderByDescending(s => s.Number)
                   .FirstOrDefault(s => s.Number < number);
    }
}
