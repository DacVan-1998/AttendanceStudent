using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Interfaces;
using FluentValidation;

namespace AttendanceStudent.Student.Validators
{
    public static class VietnamesePhoneNumberValidator
    {
        public static IRuleBuilderOptions<T, string> VietnamesePhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder,
            IStringLocalizationService localizationService)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(localizationService[LocalizationString.Common.EmptyField].Value)
                .Matches("^(\\+84|0)(3|5|7|8|9)[0-9]\\d{7}")
                .WithMessage(localizationService[LocalizationString.Common.IncorrectFormatField].Value);
        }
    }
}