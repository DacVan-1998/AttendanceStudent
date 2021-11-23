using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Subject.DTO.Requests;
using AttendanceStudent.Subject.Interfaces;
using Infrastructure.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceStudent.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="subjectService"></param>
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        /// <summary>
        /// Create Subject
        /// </summary>
        /// <param name="createSubjectRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSubjectAsync(CreateSubjectRequest createSubjectRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _subjectService.CreateSubjectAsync(createSubjectRequest, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Update Subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="updateSubjectRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{subjectId}")]
        public async Task<IActionResult> UpdateSubjectAsync(Guid subjectId, UpdateSubjectRequest updateSubjectRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _subjectService.UpdateSubjectAsync(subjectId, updateSubjectRequest, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Delete Subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{subjectId}")]
        public async Task<IActionResult> DeleteSubjectAsync(Guid subjectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _subjectService.DeleteSubjectAsync(subjectId, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse());
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// View Subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{subjectId}")]
        public async Task<IActionResult> ViewSubjectAsync(Guid subjectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _subjectService.ViewSubjectAsync(subjectId, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse(data: result.Data));
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        /// View List Subject
        /// </summary>
        /// <param name="paginationRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> ViewSubjectsAsync([FromQuery] ViewListSubjectsRequest paginationRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _subjectService.ViewListSubjectAsync(paginationRequest, cancellationToken);
                if (result.Succeeded)
                    return Ok(new SuccessResponse(data: result.Data));
                return Accepted(new FailureResponse(result.Errors));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}