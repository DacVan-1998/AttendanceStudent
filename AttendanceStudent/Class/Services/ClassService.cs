using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Class.DTO.Requests;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Class.Interfaces;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Commons.Models;

namespace AttendanceStudent.Class.Services
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IStringLocalizationService _localizationService;


        public ClassService(IUnitOfWork unitOfWork, IPaginationService paginationService, IStringLocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _localizationService = localizationService;
        }

        public async Task<Result<ActionResult>> CreateClassAsync(CreateClassRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedClassCode = await _unitOfWork.Classes.GetClassByCodeAsync(request.Code, cancellationToken);
                if (existedClassCode != null)
                    return Result<ActionResult>.Fail(LocalizationString.Class.DuplicatedCode.ToErrors(_localizationService));

                var existedClassName = await _unitOfWork.Classes.GetClassByNameAsync(request.Name, cancellationToken);
                if (existedClassName != null)
                    return Result<ActionResult>.Fail(LocalizationString.Class.DuplicatedName.ToErrors(_localizationService));

                var classEntity = new Models.Class()
                {
                    Id = Guid.NewGuid(),
                    Code = request.Code,
                    Name = request.Name
                };
                await _unitOfWork.Classes.AddAsync(classEntity, cancellationToken);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> UpdateClassAsync(Guid id, UpdateClassRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedClass = await _unitOfWork.Classes.GetClassByIdAsync(id, cancellationToken);
                if (existedClass == null)
                    return Result<ActionResult>.Fail(LocalizationString.Class.NotFound.ToErrors(_localizationService));

                var codeCheck = await _unitOfWork.Classes.IsDuplicatedCodeAsync(id, request.Code, cancellationToken);
                if (codeCheck)
                    return Result<ActionResult>.Fail(LocalizationString.Class.DuplicatedCode.ToErrors(_localizationService));

                var nameCheck = await _unitOfWork.Classes.IsDuplicatedNameAsync(id, request.Name, cancellationToken);
                if (nameCheck)
                    return Result<ActionResult>.Fail(LocalizationString.Class.DuplicatedName.ToErrors(_localizationService));

                existedClass.Code = request.Code;
                existedClass.Name = request.Name;
                _unitOfWork.Classes.Update(existedClass);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> DeleteClassAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedClass = await _unitOfWork.Classes.GetClassByIdAsync(id, cancellationToken);
                if (existedClass == null)
                    return Result<ActionResult>.Fail(LocalizationString.Class.NotFound.ToErrors(_localizationService));

                _unitOfWork.Classes.Remove(existedClass);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ViewClassResponse>> ViewClassAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedClass = await _unitOfWork.Classes.GetClassByIdAsync(id, cancellationToken);
                if (existedClass == null)
                    return Result<ViewClassResponse>.Fail(LocalizationString.Class.NotFound.ToErrors(_localizationService));

                return Result<ViewClassResponse>.Succeed(new ViewClassResponse()
                {
                    Id = existedClass.Id,
                    Code = existedClass.Code,
                    Name = existedClass.Name
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<PaginationBaseResponse<ViewClassResponse>>> ViewListClassAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var queryable = await _unitOfWork.Classes.SearchClassByCode(query, cancellationToken);
                var result = await _paginationService.PaginateAsync(queryable, query.Page, query.OrderBy, query.OrderByDesc, query.Size, cancellationToken);
                return Result<PaginationBaseResponse<ViewClassResponse>>.Succeed(new PaginationBaseResponse<ViewClassResponse>()
                {
                    CurrentPage = result.CurrentPage,
                    OrderBy = result.OrderBy,
                    OrderByDesc = result.OrderByDesc,
                    PageSize = result.PageSize,
                    TotalItems = result.TotalItems,
                    TotalPages = result.TotalPages,
                    Result = result.Result.Select(r => new ViewClassResponse()
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