using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.RollCall.DTO.Requests;
using FluentValidation;

namespace AttendanceStudent.RollCall.Validators
{
    public class RemoveStudentToRollCallRequestValidator: AbstractValidator<AddStudentToRollCallRequest>
    {
        public RemoveStudentToRollCallRequestValidator(IUnitOfWork unitOfWork, IStringLocalizationService localizationService)
        {
            RuleFor(x => x.Students)
                .Cascade(CascadeMode.Stop)
                .IsValidStudents(unitOfWork, localizationService);
        }
    }
}