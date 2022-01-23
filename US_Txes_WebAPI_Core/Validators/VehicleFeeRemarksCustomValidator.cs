using System.ComponentModel.DataAnnotations;

namespace US_Txes_WebAPI_Core.Validators
{
    public class VehicleFeeKoefficientRemarksValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isAvailableProperty = validationContext.ObjectInstance.GetType().GetProperty("IsAvailable");
            var isAvailablePropertyValue = isAvailableProperty.GetValue(validationContext.ObjectInstance, null);

            if (isAvailablePropertyValue == null)
            {
                return new ValidationResult("Error while validating VehicleFee object");
            }
            else
            {
                var isAvailable = bool.Parse(isAvailablePropertyValue.ToString());
                if (isAvailable)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    if (value == null)
                    {
                        return new ValidationResult("Remarks should not be null");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
