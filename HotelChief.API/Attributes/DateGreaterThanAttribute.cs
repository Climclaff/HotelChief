namespace HotelChief.API.Attributes
{
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property)]
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        public DateGreaterThanAttribute(string dateToCompareToFieldName)
        {
            DateToCompareToFieldName = dateToCompareToFieldName;
        }

        private string DateToCompareToFieldName { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime? firstDate = (DateTime?)value;

            DateTime? secondDate = (DateTime?)validationContext?.ObjectType?.GetProperty(DateToCompareToFieldName)?.GetValue(validationContext.ObjectInstance, null);

            if (firstDate > secondDate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date is not later");
            }
        }
    }
}
