using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Student.DTO.Requests;
using FluentValidation;

namespace AttendanceStudent.Student.Validators
{
    public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStudentRequestValidator(IUnitOfWork unitOfWork, IStringLocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            RuleFor(d => d.StudentCode).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField)
                .MinimumLength(Constants.FieldLength.TextMinLength)
                .WithMessage(LocalizationString.Common.MinLengthField)
                .MaximumLength(Constants.FieldLength.TextMaxLength)
                .WithMessage(LocalizationString.Common.MaxLengthField)
                .MustAsync(IsUniqueStudentCode).WithMessage(LocalizationString.Student.DuplicatedCode);

            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .EmailAddress().WithMessage(LocalizationString.Common.IncorrectFormatField)
                .MustAsync(IsUniqueEmail).WithMessage(LocalizationString.Student.DuplicatedEmail);

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop).VietnamesePhoneNumber(localizationService)
                .MustAsync(IsUniquePhoneNumber).WithMessage(LocalizationString.Student.DuplicatedPhoneNumber);

            RuleFor(d => d.FullName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField)
                .MinimumLength(Constants.FieldLength.TextMinLength)
                .WithMessage(LocalizationString.Common.MinLengthField)
                .MaximumLength(Constants.FieldLength.TextMaxLength)
                .WithMessage(LocalizationString.Common.MaxLengthField);
        }

        private async Task<bool> IsUniqueStudentCode(string? studentCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(studentCode)) return false;
            var existedStudent = await _unitOfWork.Students.GetStudentByCodeAsync(studentCode, cancellationToken);
            return existedStudent == null;
        }
        
        private async Task<bool> IsUniqueEmail(string? email, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var existedStudent = await _unitOfWork.Students.GetStudentByEmailAsync(email, cancellationToken);
            return existedStudent == null;
        }
        
        private async Task<bool> IsUniquePhoneNumber(string? phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            var existedStudent = await _unitOfWork.Students.GetStudentByPhoneNumberAsync(phoneNumber, cancellationToken);
            return existedStudent == null;
        }
    }
}