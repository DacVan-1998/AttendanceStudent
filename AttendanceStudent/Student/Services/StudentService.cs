using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.Student.DTO.Requests;
using AttendanceStudent.Student.DTO.Responses;
using AttendanceStudent.Student.Interfaces;

namespace AttendanceStudent.Student.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IStringLocalizationService _localizationService;

        public StudentService(IUnitOfWork unitOfWork, IPaginationService paginationService, IStringLocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _localizationService = localizationService;
        }

        public async Task<Result<ActionResult>> CreateStudentAsync(CreateStudentRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var student = new Models.Student()
                {
                    Id = Guid.NewGuid(),
                    StudentCode = request.StudentCode,
                    FullName = request.FullName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber
                };
                await _unitOfWork.Students.AddAsync(student, cancellationToken);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> UpdateStudentAsync(Guid id, UpdateStudentRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedStudent = await _unitOfWork.Students.GetStudentByIdAsync(id, cancellationToken);
                if (existedStudent == null)
                    return Result<ActionResult>.Fail(LocalizationString.Student.NotFound.ToErrors(_localizationService));

                var codeCheck = await _unitOfWork.Students.IsDuplicatedCodeAsync(id, request.StudentCode, cancellationToken);
                if (codeCheck)
                    return Result<ActionResult>.Fail(LocalizationString.Student.DuplicatedCode.ToErrors(_localizationService));

                var emailCheck = await _unitOfWork.Students.IsDuplicatedEmailAsync(id, request.Email, cancellationToken);
                if (emailCheck)
                    return Result<ActionResult>.Fail(LocalizationString.Student.DuplicatedEmail.ToErrors(_localizationService));


                var phoneNumberCheck = await _unitOfWork.Students.IsDuplicatedPhoneNumberAsync(id, request.PhoneNumber, cancellationToken);
                if (phoneNumberCheck)
                    return Result<ActionResult>.Fail(LocalizationString.Student.DuplicatedPhoneNumber.ToErrors(_localizationService));

                existedStudent.StudentCode = request.StudentCode;
                existedStudent.FullName = request.FullName;
                existedStudent.Email = request.Email;
                existedStudent.PhoneNumber = request.PhoneNumber;
                _unitOfWork.Students.Update(existedStudent);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> DeleteStudentAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedStudent = await _unitOfWork.Students.GetStudentByIdAsync(id, cancellationToken);
                if (existedStudent == null)
                    return Result<ActionResult>.Fail(LocalizationString.Student.NotFound.ToErrors(_localizationService));

                _unitOfWork.Students.Remove(existedStudent);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ViewStudentResponse>> ViewStudentAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedStudent = await _unitOfWork.Students.GetStudentByIdAsync(id, cancellationToken);
                if (existedStudent == null)
                    return Result<ViewStudentResponse>.Fail(LocalizationString.Student.NotFound.ToErrors(_localizationService));

                return Result<ViewStudentResponse>.Succeed(new ViewStudentResponse()
                {
                    Id = existedStudent.Id,
                    StudentCode = existedStudent.StudentCode,
                    FullName = existedStudent.FullName,
                    Email = existedStudent.Email,
                    PhoneNumber = existedStudent.PhoneNumber
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<PaginationBaseResponse<ViewStudentResponse>>> ViewListStudentAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var queryable = await _unitOfWork.Students.SearchStudent(query, cancellationToken);
                var result = await _paginationService.PaginateAsync(queryable, query.Page, query.OrderBy, query.OrderByDesc, query.Size, cancellationToken);
                return Result<PaginationBaseResponse<ViewStudentResponse>>.Succeed(new PaginationBaseResponse<ViewStudentResponse>()
                {
                    CurrentPage = result.CurrentPage,
                    OrderBy = result.OrderBy,
                    OrderByDesc = result.OrderByDesc,
                    PageSize = result.PageSize,
                    TotalItems = result.TotalItems,
                    TotalPages = result.TotalPages,
                    Result = result.Result.Select(r => new ViewStudentResponse()
                    {
                        Id = r.Id,
                        StudentCode = r.StudentCode,
                        FullName = r.FullName,
                        Email = r.Email,
                        PhoneNumber = r.PhoneNumber,
                        TotalImages = r.Images.Count
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}