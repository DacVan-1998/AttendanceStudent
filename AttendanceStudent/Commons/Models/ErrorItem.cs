using System.Diagnostics.CodeAnalysis;
// ReSharper disable All

#pragma warning disable 8618
namespace Application.Common.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorItem
    {
        public string FieldName { get; set; }
        public string Error { get; set; }
    }
}