using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

// ReSharper disable All
#pragma warning disable 8625

namespace Application.Common.Models
{
    /// <summary>
    /// To generate result for all action in this system.
    /// Result contains:
    ///     1. Succeeded flag: true/false
    ///     2. Errors list: List of errors if any in case of failure
    ///     3. Data object: Data object if action need to pass data back to caller
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    [ExcludeFromCodeCoverage]
    public class Result<TDto> where TDto : class
    {
        internal Result(bool success, IEnumerable<ErrorItem> errors, TDto dto, bool forbidden)
        {
            Succeeded = success;
            Errors = errors.ToArray();
            Data = dto;
            Forbidden = forbidden;
        }

        public TDto Data { get; private set; }
        public bool Succeeded { get; private set; }
        public bool Forbidden { get; private set; }

        public IEnumerable<ErrorItem> Errors { get; private set; }

        public static Result<TDto> Succeed(TDto data = null)
        {
            return new Result<TDto>(true, new ErrorItem[] { }, data,false);
        }

        public static Result<TDto> Fail(IEnumerable<ErrorItem> errors, TDto data = null)
        {
            return new Result<TDto>(false, errors, data,false);
        }

        public static Result<TDto> Forbid(IEnumerable<ErrorItem> errors, TDto data = null)
        {
            return new Result<TDto>(false, errors, data,true);
        }
    }
}