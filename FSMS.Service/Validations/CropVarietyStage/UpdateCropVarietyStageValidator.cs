
using FluentValidation;
using FSMS.Service.ViewModels.CropVarietyGrowthTasks;
using FSMS.Service.ViewModels.CropVarietyStages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSMS.Service.Validations.CropVarietyStage
{
    public class UpdateCropVarietyStageValidator : AbstractValidator<UpdateCropVarietyStage>
    {

        public UpdateCropVarietyStageValidator()
        {
            RuleFor(o => o.StageName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Length(1, 100).WithMessage("{PropertyName} must be less than or equals 100 characters.");
            RuleFor(o => o.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(200).WithMessage("{PropertyName} must be less than or equals 200 characters.");
            RuleFor(o => o.StartDate)
                .Must(IsValidDate).WithMessage("Invalid {PropertyName}, The time gap must be around 30 year from the present and not exceeding 10 years");
            RuleFor(o => o.EndDate)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be greater than or equal to StartDate")
                .Must(IsFirstDateAfterSecondDate).WithMessage("Invalid {PropertyName}, The time must over from the present");
        }
        protected bool IsFirstDateAfterSecondDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;

            return date > currentDate;
        }
        protected bool IsValidDate(DateTime taskDate)
        {
            int yearInput = taskDate.Year;
            int yearNow = DateTime.UtcNow.Year;
            if (yearInput > yearNow - 50 && yearInput < yearNow + 20)
                return true;
            return false;
        }
        protected bool IsValidImageExtension(string filename)
        {
            string validExtensionsPattern = @"\.(jpeg|png|bmp|webp)$";
            if (Regex.IsMatch(filename, validExtensionsPattern, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
