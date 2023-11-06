using FluentValidation;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Validations.Payment
{
    public class PaymentValidator : AbstractValidator<CreatePayment>
    {
        public PaymentValidator()
        {
            RuleFor(o => o.OrderId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            
            RuleFor(o => o.PaymentDate)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .Must(IsFirstDateAfterSecondDate).WithMessage("Invalid {PropertyName}, The time must over from the present");

            RuleFor(o => o.PaymentMethod)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .IsEnumName(typeof(PaymentMethodEnum)).WithMessage("{PropertyName} must be COD or Momo");
  
            RuleFor(o => o.UserId)
               .NotEmpty().WithMessage("{PropertyName} is empty");
        }
        protected bool IsFirstDateAfterSecondDate(DateTime date)
        {
            DateTime currentDate = DateTime.Now;

            return date > currentDate;
        }
    }
}
