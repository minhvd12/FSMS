using FluentValidation;
using FSMS.Service.Enums;
using FSMS.Service.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FSMS.Service.Validations.Order
{
    public class OrderValidator : AbstractValidator<CreateOrder>
    {
        public OrderValidator()
        {

            RuleFor(o => o.OrderDate)
                .Must(IsCurrentDate).WithMessage("Invalid {PropertyName}, The time must from the present");
            RuleFor(o => o.UserId)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(o => o.DeliveryAddress)
                .NotEmpty().WithMessage("{PropertyName} is empty");
            RuleFor(o => o.PaymentMethod)
                .NotEmpty().WithMessage("{PropertyName} is empty")
                .IsEnumName(typeof(PaymentMethodEnum)).WithMessage("{PropertyName} must be Momo or COD");
            RuleFor(o => o.PhoneNumber)
               .Must(IsValidPhoneNumber).WithMessage("{PropertyName}  must start with one of the following prefixes: 032-039, 070-079, 081-089, 090-099, or 012-019. and must be 10 or 11 digits long.");
            RuleForEach(o => o.OrderDetails)
                .NotEmpty().WithMessage("{PropertyName} is empty");
        }
        protected bool IsCurrentDate(DateTime inputDate)
        {
            DateTime today = DateTime.Now;
            return inputDate.Year == today.Year &&
                inputDate.Month == today.Month &&
                inputDate.Day == today.Day;
        }
        protected bool IsValidPhoneNumber(string inputPhoneNumber)
        {
            string phoneNumberPattern = @"^(03[2-9]|07[0|6-9]|08[1-9]|09[0-9]|01[2|6-9])+([0-9]{7,8})\b";
            return Regex.IsMatch(inputPhoneNumber, phoneNumberPattern);
        }
    }
}
