using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.Interfaces;
using FluentValidation;

namespace AttendanceStudent.RollCall.Validators
{
    public static class StudentListValidator
    {
        public static IRuleBuilderOptions<T, List<Guid>> IsValidStudents<T>(this IRuleBuilder<T, List<Guid>> ruleBuilder, IUnitOfWork unitOfWork, IStringLocalizationService localizationService)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(localizationService[LocalizationString.Common.EmptyField].Value)
                .MustBeUniqueStudentIds(localizationService)
                .MustBeExistedStudents(unitOfWork, localizationService);
        }

        /// <summary>
        /// Category Ids must be unique
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="localizationService"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private static IRuleBuilderOptions<T, List<Guid>> MustBeUniqueStudentIds<T>(this IRuleBuilder<T, List<Guid>> ruleBuilder, IStringLocalizationService localizationService)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(localizationService[LocalizationString.Common.EmptyField].Value)
                .Must((_, list, _) =>
                {
                    if (list == null || list.Count == 0)
                        return false;
                    return list.Count == list.Distinct().Count();
                }).WithMessage(localizationService[LocalizationString.Student.Duplicated].Value);
        }

        /// <summary>
        /// Role Ids must be existed in the db with active status
        /// </summary>
        /// <param name="ruleBuilder"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="localizationService"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private static IRuleBuilderOptions<T, List<Guid>> MustBeExistedStudents<T>(this IRuleBuilder<T, List<Guid>> ruleBuilder, IUnitOfWork unitOfWork, IStringLocalizationService localizationService)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(localizationService[LocalizationString.Common.EmptyField].Value)
                .MustAsync(
                    async (_, list, _) =>
                    {
                        //Check input list
                        if (list == null || list.Count == 0)
                            return false;
                        if (list.Count != list.Distinct().Count())
                            return false;
                        // Hit to the db 
                        var students = await unitOfWork.Students.GetStudentsAsync(list, CancellationToken.None);
                        if (students == null || students.Count == 0)
                            return false;
                        return students.Count == list.Distinct().Count();
                    }).WithMessage(localizationService[LocalizationString.Student.NotFound].Value);
        }
    }
}