using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.RollCall.DTO.Requests;
using FluentValidation;

namespace AttendanceStudent.RollCall.Validators
{
    public class CreateRollCallRequestValidator : AbstractValidator<CreateRollCallRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateRollCallRequestValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(d => d.ClassId).Cascade(CascadeMode.Stop)
                .MustAsync(IsExistedClass).WithMessage(LocalizationString.Class.NotFound);;
            
            RuleFor(d => d.SubjectId).Cascade(CascadeMode.Stop)
                .MustAsync(IsExistedSubject).WithMessage(LocalizationString.Subject.NotFound);
        }
        
        private async Task<bool> IsExistedClass(Guid classId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await _unitOfWork.Classes.GetClassByIdAsync(classId, cancellationToken);
            return category != null;
        }
        
        private async Task<bool> IsExistedSubject(Guid subjectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await _unitOfWork.Subjects.GetSubjectByIdAsync(subjectId, cancellationToken);
            return category != null;
        }
    }
}