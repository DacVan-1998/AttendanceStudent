using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Class.DTO.Requests;
using AttendanceStudent.Class.Interfaces;
using Infrastructure.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceStudent.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="classService"></param>
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }


        /// <summary>
        /// Create Class
        /// </summary>
        /// <param name="createClassRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateClassAsync(CreateClassRequest createClassRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _classService.CreateClassAsync(createClassRequest, cancellationToken);
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
        /// Update Class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="updateClassRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{classId}")]
        public async Task<IActionResult> UpdateClassAsync(Guid classId, UpdateClassRequest updateClassRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _classService.UpdateClassAsync(classId, updateClassRequest, cancellationToken);
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
        /// Delete Class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{classId}")]
        public async Task<IActionResult> DeleteClassAsync(Guid classId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _classService.DeleteClassAsync(classId, cancellationToken);
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
        /// View Class
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{classId}")]
        public async Task<IActionResult> ViewClassAsync(Guid classId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _classService.ViewClassAsync(classId, cancellationToken);
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
        /// View List Class
        /// </summary>
        /// <param name="paginationRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> ViewClassesAsync([FromQuery] ViewListClassesRequest paginationRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _classService.ViewListClassAsync(paginationRequest, cancellationToken);
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