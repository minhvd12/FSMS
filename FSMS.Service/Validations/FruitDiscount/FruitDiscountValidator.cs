using FluentValidation;
using FSMS.Service.ViewModels.FruitDiscounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Validations.FruitDiscount
{
    public class FruitDiscountValidator : AbstractValidator<CreateFruitDiscount>
    {
        public FruitDiscountValidator()
        {

            RuleFor(o => o.FruitId)
              .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(o => o.DiscountName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .MaximumLength(200).WithMessage("{PropertyName} must be less than or equals 200 characters.");
            RuleFor(o => o.DiscountThreshold)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("{PropertyName} must be less than 100");
            RuleFor(o => o.DiscountPercentage)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("{PropertyName} must be less than 100");
            RuleFor(o => o.DiscountExpiryDate)
               .NotEmpty().WithMessage("{PropertyName} is empty")
               .Must(IsFirstDateAfterSecondDate).WithMessage("Invalid {PropertyName}, The time must over from the present");
            RuleFor(o => o.DepositAmount)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
        }
        protected bool IsFirstDateAfterSecondDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;

            return date > currentDate;
        }
    }
}
