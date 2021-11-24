using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.RollCall.DTO.Requests;
using AttendanceStudent.RollCall.DTO.Responses;
using AttendanceStudent.RollCall.Interfaces;
using AttendanceStudent.Subject.DTO.Responses;

namespace AttendanceStudent.RollCall.Services
{
    public class RollCallService : IRollCallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IStringLocalizationService _localizationService;

        public RollCallService(IUnitOfWork unitOfWork, IPaginationService paginationService, IStringLocalizationService localizationService)
        {
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _localizationService = localizationService;
        }

        public async Task<Result<ActionResult>> CreateRollCallAsync(CreateRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var rollCall = new Models.RollCall()
                {
                    Id = Guid.NewGuid(),
                    FromDate = request.FromDate,
                    EndDate = request.EndDate,
                    StartTime = request.StartTime,
                    FinishTime = request.FinishTime,
                    ClassId = request.ClassId,
                    SubjectId = request.SubjectId
                };
                await _unitOfWork.RollCalls.AddAsync(rollCall, cancellationToken);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> UpdateRollCallAsync(Guid rollCallId, UpdateRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedRollCall = await _unitOfWork.RollCalls.GetRollCallByIdAsync(rollCallId, cancellationToken);
                if (existedRollCall == null)
                    return Result<ActionResult>.Fail(LocalizationString.RollCall.NotFound.ToErrors(_localizationService));

                existedRollCall.FromDate = request.FromDate;
                existedRollCall.EndDate = request.EndDate;
                existedRollCall.StartTime = request.StartTime;
                existedRollCall.FinishTime = request.FinishTime;
                existedRollCall.ClassId = request.ClassId;
                existedRollCall.SubjectId = request.SubjectId;

                _unitOfWork.RollCalls.Update(existedRollCall);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> DeleteRollCallAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedRollCall = await _unitOfWork.RollCalls.GetRollCallByIdAsync(id, cancellationToken);
                if (existedRollCall == null)
                    return Result<ActionResult>.Fail(LocalizationString.RollCall.NotFound.ToErrors(_localizationService));

                _unitOfWork.RollCalls.Remove(existedRollCall);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ViewRollCallResponse>> ViewRollCallAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var existedRollCall = await _unitOfWork.RollCalls.GetRollCallByIdAsync(id, cancellationToken);
                if (existedRollCall == null)
                    return Result<ViewRollCallResponse>.Fail(LocalizationString.RollCall.NotFound.ToErrors(_localizationService));

                return Result<ViewRollCallResponse>.Succeed(new ViewRollCallResponse()
                {
                    Id = existedRollCall.Id,
                    FromDate = existedRollCall.FromDate.ToString("MM/dd/yyyy"),
                    EndDate = existedRollCall.EndDate.ToString("MM/dd/yyyy"),
                    StartTime = existedRollCall.StartTime,
                    FinishTime = existedRollCall.FinishTime,
                    Class = new ViewClassResponse()
                    {
                        Id = existedRollCall.Class?.Id ?? Guid.Empty,
                        Code = existedRollCall.Class?.Code ?? string.Empty,
                        Name = existedRollCall.Class?.Name ?? string.Empty,
                    },
                    Subject = new ViewSubjectResponse()
                    {
                        Id = existedRollCall.Subject?.Id ?? Guid.Empty,
                        Code = existedRollCall.Subject?.Code ?? string.Empty,
                        Name = existedRollCall.Subject?.Name ?? string.Empty,
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<PaginationBaseResponse<ViewRollCallResponse>>> ViewListRollCallAsync(PaginationBaseRequest query, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var queryable = await _unitOfWork.RollCalls.SearchRollCall(query, cancellationToken);
                var result = await _paginationService.PaginateAsync(queryable, query.Page, query.OrderBy, query.OrderByDesc, query.Size, cancellationToken);
                return Result<PaginationBaseResponse<ViewRollCallResponse>>.Succeed(new PaginationBaseResponse<ViewRollCallResponse>()
                {
                    CurrentPage = result.CurrentPage,
                    OrderBy = result.OrderBy,
                    OrderByDesc = result.OrderByDesc,
                    PageSize = result.PageSize,
                    TotalItems = result.TotalItems,
                    TotalPages = result.TotalPages,
                    Result = result.Result.Select(r => new ViewRollCallResponse()
                    {
                        Id = r.Id,
                        FromDate = r.FromDate.ToString("MM/dd/yyyy"),
                        EndDate = r.EndDate.ToString("MM/dd/yyyy"),
                        StartTime = r.StartTime,
                        FinishTime = r.FinishTime,
                        Class = new ViewClassResponse()
                        {
                            Id = r.Class?.Id ?? Guid.Empty,
                            Code = r.Class?.Code ?? string.Empty,
                            Name = r.Class?.Name ?? string.Empty,
                        },
                        Subject = new ViewSubjectResponse()
                        {
                            Id = r.Subject?.Id ?? Guid.Empty,
                            Code = r.Subject?.Code ?? string.Empty,
                            Name = r.Subject?.Name ?? string.Empty,
                        }
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