using System;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Class.DTO.Requests;
using AttendanceStudent.Class.Interfaces;
using AttendanceStudent.Student.DTO.Requests;
using AttendanceStudent.Student.Interfaces;
using Infrastructure.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceStudent.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="studentService"></param>
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Create Student
        /// </summary>
        /// <param name="createStudentRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateStudentAsync(CreateStudentRequest createStudentRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _studentService.CreateStudentAsync(createStudentRequest, cancellationToken);
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
        /// Update Student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="updateStudentRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{studentId}")]
        public async Task<IActionResult> UpdateStudentAsync(Guid studentId, UpdateStudentRequest updateStudentRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _studentService.UpdateStudentAsync(studentId, updateStudentRequest, cancellationToken);
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
        /// <param name="studentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{studentId}")]
        public async Task<IActionResult> DeleteStudentAsync(Guid studentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(studentId, cancellationToken);
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
        /// View Student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{studentId}")]
        public async Task<IActionResult> ViewStudentAsync(Guid studentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _studentService.ViewStudentAsync(studentId, cancellationToken);
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
        /// View List Student
        /// </summary>
        /// <param name="paginationRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> ViewStudentsAsync([FromQuery] ViewListStudentsRequest paginationRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var result = await _studentService.ViewListStudentAsync(paginationRequest, cancellationToken);
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