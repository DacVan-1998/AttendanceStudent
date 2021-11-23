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
using AttendanceStudent.Subject.DTO.Requests;
using AttendanceStudent.Subject.DTO.Responses;
using AttendanceStudent.Subject.Interfaces;

namespace AttendanceStudent.Subject.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IStringLocalizationService _localizationService;


        public SubjectService(IUnitOfWork unitOfWork, IPaginationService paginationService, IStringLocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _localizationService = localizationService;
        }

        public async Task<Result<ActionResult>> CreateSubjectAsync(CreateSubjectRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var subjectEntity = new Models.Subject()
                {
                    Id = Guid.NewGuid(),
                    Code = request.Code,
                    Name = request.Name
                };
                await _unitOfWork.Subjects.AddAsync(subjectEntity, cancellationToken);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> UpdateSubjectAsync(Guid id, UpdateSubjectRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedSubject = await _unitOfWork.Subjects.GetSubjectByIdAsync(id, cancellationToken);
                if (existedSubject == null)
                    return Result<ActionResult>.Fail(LocalizationString.Subject.NotFound.ToErrors(_localizationService));

                var codeCheck = await _unitOfWork.Subjects.IsDuplicatedCodeAsync(id, request.Code, cancellationToken);
                if (codeCheck)
                    return Result<ActionResult>.Fail(LocalizationString.Subject.DuplicatedCode.ToErrors(_localizationService));

                var nameCheck = await _unitOfWork.Subjects.IsDuplicatedNameAsync(id, request.Name, cancellationToken);
                if (nameCheck)
                    return Result<ActionResult>.Fail(LocalizationString.Subject.DuplicatedName.ToErrors(_localizationService));

                existedSubject.Code = request.Code;
                existedSubject.Name = request.Name;
                _unitOfWork.Subjects.Update(existedSubject);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> DeleteSubjectAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedSubject = await _unitOfWork.Subjects.GetSubjectByIdAsync(id, cancellationToken);
                if (existedSubject == null)
                    return Result<ActionResult>.Fail(LocalizationString.Subject.NotFound.ToErrors(_localizationService));

                _unitOfWork.Subjects.Remove(existedSubject);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ViewSubjectResponse>> ViewSubjectAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedSubject = await _unitOfWork.Subjects.GetSubjectByIdAsync(id, cancellationToken);
                if (existedSubject == null)
                    return Result<ViewSubjectResponse>.Fail(LocalizationString.Subject.NotFound.ToErrors(_localizationService));

                return Result<ViewSubjectResponse>.Succeed(new ViewSubjectResponse()
                {
                    Id = existedSubject.Id,
                    Code = existedSubject.Code,
                    Name = existedSubject.Name
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<PaginationBaseResponse<ViewSubjectResponse>>> ViewListSubjectAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var queryable = await _unitOfWork.Subjects.SearchSubjectByCode(query, cancellationToken);
                var result = await _paginationService.PaginateAsync(queryable, query.Page, query.OrderBy, query.OrderByDesc, query.Size, cancellationToken);
                return Result<PaginationBaseResponse<ViewSubjectResponse>>.Succeed(new PaginationBaseResponse<ViewSubjectResponse>()
                {
                    CurrentPage = result.CurrentPage,
                    OrderBy = result.OrderBy,
                    OrderByDesc = result.OrderByDesc,
                    PageSize = result.PageSize,
                    TotalItems = result.TotalItems,
                    TotalPages = result.TotalPages,
                    Result = result.Result.Select(r => new ViewSubjectResponse()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Code = r.Code,
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