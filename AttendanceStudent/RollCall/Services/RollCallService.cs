using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO.Pagination.Requests;
using Application.Common.Models;
using AttendanceStudent.Attendance.DTO.Responses;
using AttendanceStudent.Class.DTO.Responses;
using AttendanceStudent.Commons;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Extensions;
using AttendanceStudent.Commons.Interfaces;
using AttendanceStudent.Commons.Models;
using AttendanceStudent.Database.Configurations;
using AttendanceStudent.File.DTO.Responses;
using AttendanceStudent.Models;
using AttendanceStudent.RollCall.DTO.Requests;
using AttendanceStudent.RollCall.DTO.Responses;
using AttendanceStudent.RollCall.Interfaces;
using AttendanceStudent.Student.DTO.Responses;
using AttendanceStudent.Subject.DTO.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

namespace AttendanceStudent.RollCall.Services
{
    public class RollCallService : IRollCallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaginationService _paginationService;
        private readonly IStringLocalizationService _localizationService;
        private readonly ResourceConfiguration _resourceConfiguration;

        public RollCallService(IUnitOfWork unitOfWork, IPaginationService paginationService, IStringLocalizationService localizationService, IOptions<ResourceConfiguration> resourceConfiguration)
        {
            _unitOfWork = unitOfWork;
            _paginationService = paginationService;
            _localizationService = localizationService;
            _resourceConfiguration = resourceConfiguration.Value;
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
                    FromDate = existedRollCall.FromDate.ToString("yyyy//MM/dd"),
                    EndDate = existedRollCall.EndDate.ToString("yyyy/MM/dd"),
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
                    },
                    AttendanceLogs = existedRollCall.AttendanceLogs.Select(x => new AttendanceLogResponse()
                    {
                        Id = x.Id,
                        AttendanceDate = x.AttendanceDate.ToString("d"),
                        AttendanceTime = x.AttendanceTime,
                        LogImagePaths = new List<string>(),
                        PresentRate = x.AttendanceStudents.Where(at => at.IsPresent).ToList().Count + "/" + x.AttendanceStudents.Count,
                        AttendanceStudents = new List<AttendanceStudentViewResponse>()
                    }).ToList(),
                    Students = existedRollCall.StudentRollCalls.Select(x => new ViewStudentResponse()
                    {
                        Id = x.StudentId,
                        Email = x.Student.Email,
                        FullName = x.Student.FullName,
                        PhoneNumber = x.Student.PhoneNumber,
                        StudentCode = x.Student.StudentCode
                    }).ToList()
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
                        FromDate = r.FromDate.ToString("yyyy//MM/dd"),
                        EndDate = r.EndDate.ToString("yyyy/MM/dd"),
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
                        },
                        Students = r.StudentRollCalls.Select(x => new ViewStudentResponse()
                        {
                            Id = x.StudentId,
                            Email = x.Student.Email,
                            FullName = x.Student.FullName,
                            PhoneNumber = x.Student.PhoneNumber,
                            StudentCode = x.Student.StudentCode
                        }).ToList()
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> AddStudentToRollCallAsync(Guid rollCallId, AddStudentToRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Get Tenant 
                var rollCall = await _unitOfWork.RollCalls.GetRollCallByIdAsync(rollCallId, cancellationToken);
                if (rollCall == null)
                    return Result<ActionResult>.Fail(LocalizationString.Common.ItemNotFound.ToErrors(_localizationService));

                //Check input students list must not be existed in students list of roll call in the db
                foreach (var item in request.Students.Where(item => rollCall.StudentRollCalls.Any(x => x.StudentId == item)))
                    return Result<ActionResult>.Fail(string.Format(LocalizationString.RollCall.AlreadyExisted, item).ToErrors(_localizationService));

                foreach (var item in request.Students)
                {
                    rollCall.StudentRollCalls.Add(new StudentRollCall()
                    {
                        StudentId = item,
                        RollCallId = rollCallId
                    });
                }

                _unitOfWork.RollCalls.Update(rollCall);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> RemoveStudentToRollCallAsync(Guid rollCallId, RemoveStudentToRollCallRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Get Roll Call 
                var rollCall = await _unitOfWork.RollCalls.GetRollCallByIdAsync(rollCallId, cancellationToken);
                if (rollCall == null)
                    return Result<ActionResult>.Fail(LocalizationString.Common.ItemNotFound.ToErrors(_localizationService));

                //Check input categories list must not be existed in categories list of tenant in the db
                foreach (var item in request.Students.Where(item => rollCall.StudentRollCalls.All(x => x.StudentId != item)))
                    return Result<ActionResult>.Fail(string.Format(LocalizationString.RollCall.NotExist, item).ToErrors(_localizationService));

                foreach (var item in request.Students)
                    rollCall.StudentRollCalls.Remove(rollCall.StudentRollCalls.First(x => x.RollCallId == rollCall.Id && x.StudentId == item));

                _unitOfWork.RollCalls.Update(rollCall);
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                return result > 0 ? Result<ActionResult>.Succeed() : Result<ActionResult>.Fail(Constants.CannotFinishRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Result<ActionResult>> ImportStudentToRollCallAsync(Guid rollCallId, IFormFile excelFile, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Get Roll Call 
                var rollCall = await _unitOfWork.RollCalls.GetRollCallByIdAsync(rollCallId, cancellationToken);
                if (rollCall == null)
                    return Result<ActionResult>.Fail(LocalizationString.Common.ItemNotFound.ToErrors(_localizationService));

                var response = new UploadFileResponse();
                // Validate input
                // Check name
                if (excelFile.FileName.Length > Constants.FieldLength.TextMaxLength)
                    response.Errors.Add(new ErrorItem()
                    {
                        FieldName = excelFile.FileName,
                        Error = _localizationService[LocalizationString.File.FileNameIsTooLong].Value
                    });
                // Check ext
                if (!excelFile.IsAllowedExtension(_resourceConfiguration))
                    response.Errors.Add(new ErrorItem()
                    {
                        FieldName = excelFile.FileName,
                        Error = _localizationService[LocalizationString.File.NotAllowedExtensions].Value
                    });
                // Check content type
                if (!excelFile.IsAllowedContentType(_resourceConfiguration))
                    response.Errors.Add(new ErrorItem()
                    {
                        FieldName = excelFile.FileName,
                        Error = _localizationService[LocalizationString.File.NotAllowedContentTypes].Value
                    });
                // Check size
                if (excelFile.IsOverMaxSize(_resourceConfiguration))
                    response.Errors.Add(new ErrorItem()
                    {
                        FieldName = excelFile.FileName,
                        Error = _localizationService[LocalizationString.File.FileSizeIsTooLarge].Value
                    });


                if (response.Errors.Count > 0)
                    return Result<ActionResult>.Fail(response.Errors);

                // Save to the server disk

                var path = Path.Combine(_resourceConfiguration.ImportExcelFileFolder, excelFile.Name);
                await using var stream = new FileStream(path, FileMode.Create);
                await excelFile.CopyToAsync(stream, cancellationToken); //save the file
                stream.Close();

                List<Models.Student> studentList;
                await using (FileStream fileStream = new(path, FileMode.Open))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var excel = new ExcelPackage(fileStream);
                    var workSheet = excel.Workbook.Worksheets.FirstOrDefault();

                    var newCollectionStudentDto = workSheet.ConvertSheetToObjects<StudentConvertExcelDto>();
                    studentList = newCollectionStudentDto.Select(x => new Models.Student()
                    {
                        Id = Guid.NewGuid(),
                        StudentCode = x.StudentCode,
                        Email = x.Email,
                        FullName = x.FullName,
                        PhoneNumber = x.PhoneNumber
                    }).ToList();
                }

                //Check exist student in student list from excel
                var studentCodeList = studentList.Select(x => x.StudentCode).ToList();
                var existedStudent = await _unitOfWork.Students.GetStudentsByCodesAsync(studentCodeList, cancellationToken);
                var existedStudentCodeList = existedStudent.Select(x => x.StudentCode).ToList();
                var existedStudentIdList = existedStudent.Select(x => x.Id).ToList();

                var studentAddList = new List<Models.Student>(studentList);
                studentAddList.RemoveAll(x => existedStudentCodeList.Contains(x.StudentCode));

                await _unitOfWork.Students.AddRangeAsync(studentAddList, cancellationToken);

                var studentRollCalls = studentAddList.Select(x => new StudentRollCall()
                {
                    RollCallId = rollCall.Id,
                    StudentId = x.Id
                }).ToList();

                studentRollCalls.AddRange(existedStudentIdList.Select(x => new StudentRollCall()
                {
                    StudentId = x,
                    RollCallId = rollCallId
                }).ToList());

                rollCall.StudentRollCalls = studentRollCalls;
                var result = await _unitOfWork.CompleteAsync(cancellationToken);
                // Save log
                return result <= 0 ? Result<ActionResult>.Fail(Constants.CannotFinishRequest) : Result<ActionResult>.Succeed();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}