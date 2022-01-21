namespace RecruitmentTool.WebApi.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class BirthDateAttribute : ValidationAttribute
    {
        private readonly DateTime earliestDate = DateTime.Now.AddYears(-100);

        public string GetErrorMessage => "Invalid Birth date.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime birthdate;
                var successfullyParsed = DateTime.TryParse(value.ToString(), out birthdate);

                if (successfullyParsed)
                {
                    int result = DateTime.Compare(earliestDate, birthdate);

                    if (result >= 0)
                    {
                        return new ValidationResult(this.GetErrorMessage);

                    }
                } 
            }

            return ValidationResult.Success;
        }
    }
}
