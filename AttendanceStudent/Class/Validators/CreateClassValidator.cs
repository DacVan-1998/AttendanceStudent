using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Class.DTO.Requests;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Interfaces;
using FluentValidation;

namespace AttendanceStudent.Class.Validators
{
    [ExcludeFromCodeCoverage]
    public class CreateClassValidator : AbstractValidator<CreateClassRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateClassValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(d => d.Name).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(LocalizationString.Common.EmptyField)
                .MaximumLength(Constants.FieldLength.TextMaxLength).WithMessage(LocalizationString.Common.MaxLengthField)
                .MustAsync(IsExistedName).WithMessage(LocalizationString.Class.ClassIsDuplicatedName);;
            
            RuleFor(d => d.Code).Cascade(CascadeMode.Stop)
                .MaximumLength(Constants.FieldLength.DescriptionMaxLength).WithMessage(LocalizationString.Common.MaxLengthField)
                .MustAsync(IsExistedCode).WithMessage(LocalizationString.Class.ClassIsDuplicatedCode);
        }
        
        private async Task<bool> IsExistedName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await _unitOfWork.Classes.GetClassByNameAsync(name, cancellationToken);
            return category == null;
        }
        
        private async Task<bool> IsExistedCode(string code, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await _unitOfWork.Classes.GetClassByCodeAsync(code, cancellationToken);
            return category == null;
        }
    }
}