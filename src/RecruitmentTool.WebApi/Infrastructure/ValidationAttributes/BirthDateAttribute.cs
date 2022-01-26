namespace RecruitmentTool.WebApi.Infrastructure.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class BirthDateAttribute : ValidationAttribute
    {
        private readonly DateTime earliestDate = DateTime.Now.AddYears(-99);
        private readonly DateTime latestDate = DateTime.Now.AddYears(-18);

        public string GetErrorMessage => "Invalid Birth date.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime birthdate;
                var successfullyParsed = DateTime.TryParse(value.ToString(), out birthdate);

                if (successfullyParsed)
                {
                    int tooOld = DateTime.Compare(earliestDate, birthdate);
                    int tooYoung = DateTime.Compare(birthdate, latestDate);

                    if (tooOld >= 0 || tooYoung > 0)
                    {
                        return new ValidationResult(this.GetErrorMessage);

                    }
                } 
            }

            return ValidationResult.Success;
        }
    }
}
