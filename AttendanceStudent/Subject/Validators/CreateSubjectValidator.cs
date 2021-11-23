using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Subject.DTO.Requests;
using FluentValidation;

namespace AttendanceStudent.Subject.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateSubjectValidator : AbstractValidator<CreateSubjectRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateSubjectValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(d => d.Name).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField)
                .MaximumLength(Constants.FieldLength.TextMaxLength).WithMessage(LocalizationString.Common.MaxLengthField)
                .MustAsync(IsExistedName).WithMessage(LocalizationString.Subject.DuplicatedName);
            
            RuleFor(d => d.Code).Cascade(CascadeMode.Stop)
                .MaximumLength(Constants.FieldLength.DescriptionMaxLength).WithMessage(LocalizationString.Common.MaxLengthField)
                .MustAsync(IsExistedCode).WithMessage(LocalizationString.Subject.DuplicatedCode);
        }
        
        private async Task<bool> IsExistedName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await _unitOfWork.Subjects.GetSubjectByNameAsync(name, cancellationToken);
            return category == null;
        }
        
        private async Task<bool> IsExistedCode(string code, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await _unitOfWork.Subjects.GetSubjectByCodeAsync(code, cancellationToken);
            return category == null;
        }
    }
}