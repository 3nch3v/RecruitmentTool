namespace RecruitmentTool.WebApi.Infrastructure.ValidationAttributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class SkillsCount : ValidationAttribute
    {
        public string GetErrorMessage => "Skills count should be at least 1.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var skills = value as ICollection;

                if (skills.Count > 0)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(this.GetErrorMessage);
            }

            return new ValidationResult(this.GetErrorMessage);
        }
    }
}
