using System.Collections.Generic;

namespace CoreSharp.Models.StepValidation
{
    internal class ValidationStepComparer : Comparer<ValidationStep>
    {
        //Methods 
        public override int Compare(ValidationStep current, ValidationStep previous)
            => Comparer<int?>.Default.Compare(current?.Number, previous?.Number);
    }
}
