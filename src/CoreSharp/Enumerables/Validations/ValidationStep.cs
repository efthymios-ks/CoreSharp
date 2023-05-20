using System;
using System.Diagnostics;

namespace CoreSharp.Enumerables.Validations;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class ValidationStep
{
    // Fields  
    private readonly Func<bool> _validationFunction;
    private readonly Func<string> _validationMessageFunction;

    // Constructors
    public ValidationStep(int stepNumber, Func<bool> validationFunction)
        : this(stepNumber, validationFunction, () => string.Empty)
    {
    }

    public ValidationStep(int stepNumber, Func<bool> validationFunction, Func<string> validationMessageFunction)
    {
        ValidateNumber(stepNumber);

        Number = stepNumber;
        _validationFunction = validationFunction ?? throw new ArgumentNullException(nameof(validationFunction));
        _validationMessageFunction = validationMessageFunction ?? throw new ArgumentNullException(nameof(validationMessageFunction));
    }

    // Properties
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay => ToString();
    public int Number { get; }
    internal bool IsValid => _validationFunction();
    internal string ValidationMessage => _validationMessageFunction();

    // Methods
    public override string ToString() => $"Step={Number}";

    internal static void ValidateNumber(int number)
    {
        if (number < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number), $"{nameof(ValidationStep)}.{nameof(Number)} ({number}) cannot be at least 0.");
        }
    }
}
