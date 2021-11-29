using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.RollCall.DTO.Requests;
using AttendanceStudent.RollCall.Interfaces;
using Infrastructure.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceStudent.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("[controller]")]
    public class RollCallController : ControllerBase
    {
        private readonly IRollCallService _rollCallService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="rollCallService"></param>
        public RollCallController(IRollCallService rollCallService)
        {
            _rollCallService = rollCallService;
        }

        /// <summary>
        /// Create Roll Call
        /// </summary>
        /// <param name="createRollCallRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRollCallAsync(CreateRollCallRequest createRollCallRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _rollCallService.CreateRollCallAsync(createRollCallRequest, cancellationToken);
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
        /// Update Roll Call
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="updateRollCallRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{rollCallId}")]
        public async Task<IActionResult> UpdateRollCallAsync(Guid rollCallId, UpdateRollCallRequest updateRollCallRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _rollCallService.UpdateRollCallAsync(rollCallId, updateRollCallRequest, cancellationToken);
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
        /// <param name="rollCallId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{rollCallId}")]
        public async Task<IActionResult> DeleteRollCallAsync(Guid rollCallId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _rollCallService.DeleteRollCallAsync(rollCallId, cancellationToken);
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
        /// View Roll Call
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{rollCallId}")]
        public async Task<IActionResult> ViewSubjectAsync(Guid rollCallId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _rollCallService.ViewRollCallAsync(rollCallId, cancellationToken);
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
        /// View List Roll Call
        /// </summary>
        /// <param name="paginationRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> ViewRollCallsAsync([FromQuery] ViewListRollCallsRequest paginationRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _rollCallService.ViewListRollCallAsync(paginationRequest, cancellationToken);
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
        /// Add Students to Roll Call
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="addStudentToRollCallRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Student/{rollCallId}")]
        public async Task<IActionResult> AddStudentToRollCallAsync(Guid rollCallId, AddStudentToRollCallRequest addStudentToRollCallRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _rollCallService.AddStudentToRollCallAsync(rollCallId, addStudentToRollCallRequest, cancellationToken);
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
        /// Remove Students to Roll Call
        /// </summary>
        /// <param name="rollCallId"></param>
        /// <param name="removeStudentToRollCallRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Student/{rollCallId}")]
        public async Task<IActionResult> RemoveStudentToRollCallAsync(Guid rollCallId, RemoveStudentToRollCallRequest removeStudentToRollCallRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _rollCallService.RemoveStudentToRollCallAsync(rollCallId, removeStudentToRollCallRequest, cancellationToken);
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