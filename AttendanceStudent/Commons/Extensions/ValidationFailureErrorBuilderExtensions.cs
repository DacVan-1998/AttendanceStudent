using System.Collections.Generic;
using System.Linq;
using Application.Common.Models;
using FluentValidation.Results;

namespace Application.Common.Extensions
{
    public static class ValidationFailureErrorBuilderExtensions
    {
        public static IEnumerable<ErrorItem> BuildArray(this IEnumerable<ValidationFailure> errors)
        {
            return errors.Select(e => new ErrorItem()
            {
                FieldName = e.PropertyName,
                Error = e.ErrorMessage
            }).ToArray();
        }
    }
}