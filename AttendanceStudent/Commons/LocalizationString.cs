using System.Diagnostics.CodeAnalysis;

namespace AttendanceStudent.Commons
{
    /// <summary>
    /// To define all localized strings in the system
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LocalizationString
    {
        public static class Common
        {
            public const string Success = "Success";
            public const string Error = "Error";
            public const string DataValidationError = "Data validation error";
            public const string UnknownFieldName = "General";
            public const string PermissionFieldName = "Permission";
            public const string CannotFinishRequest = "Cannot finish you action at this time, try again";
            public const string PermissionDenied = "You are not authorized to perform this action, try with other account";
            public const string ViewListFailed = "Cannot finish you action at this time, try again";
            public const string InvalidRecaptcha = "Captcha is not valid";
            public const string ItemNotFound = "Item is not found, deactivated or deleted";
            public const string EmptyField = "{PropertyName} is empty";
            public const string NullField = "{PropertyName} is null";
            public const string IncorrectFormatField = "{PropertyName} ({PropertyValue}) format is not correct";
            public const string MaxLengthField = "The length of {PropertyName} must be {MaxLength} characters or fewer. You entered {TotalLength} characters";
            public const string MinLengthField = "The length of {PropertyName} must be at least {MaxLength} characters. You entered {TotalLength} characters";
            public const string DuplicatedField = "{PropertyName} is duplicated";
            public const string DisabledFeature = "Feature is disabled";
            public const string NotValidEnumValue = "{PropertyName} value ({PropertyValue}) is not valid";
            public const string GreaterThan0Value = "{PropertyName} value must greater than 0";
        }
        
        public static class File
        {
            public const string FailedToUpload = "Failed to upload file";
            public const string FileNameIsTooLong = "File name is too long";
            public const string NotAllowedExtensions = "File extension is not allowed";
            public const string NotAllowedContentTypes = "File content type is not allowed";
            public const string FileSizeIsTooLarge = "File size is too large";
            public const string VirusDetected = "Virus has been detected in your file";
            public const string Uploaded = "Uploaded {0} file(s)";
        }

        
        public static class Class
        {
            public const string ClassIsDuplicatedName = "Class name is duplicated";
            public const string ClassIsDuplicatedCode = "Class code is duplicated";
            public const string Updated = "Updated Category {0}";
            public const string NotFound = "Class is not found or deleted";

            public const string AlreadyDeleted = "Category is already deleted, you cannot delete it again";
            public const string Deleted = "Deleted Category {0}";
            public const string FailedToDelete = "Failed to delete Category {0}";

            public const string AlreadyActivated = "Category is already activated, you cannot activate it again";
            public const string Activated = "Activated Category {0}";
            public const string FailedToActivate = "Failed to Activate Category {0}";

            public const string AlreadyDeactivated = "Category is already deactivated, you cannot deactivate it again";
            public const string Deactivated = "Deactivated Category {0}";
            public const string FailedToDeactivate = "Failed to deactivate Category {0}";

            public const string DuplicatedName = "Class name is duplicated";
            public const string DuplicatedCode = "Class code is duplicated";

            public const string FailedToAddCategory = "Failed to add Categories to Tenant {0}";
            public const string AddedCategoryToTenant = "Added categories to Tenant {0}";
            public const string AlreadyExisted = "Category {0} is already existed in categories list of Tenant, you cannot add it again";

            public const string NotExist = "Categories {0} is not exist in categories list of Tenant, you cannot remove it";
            public const string RemovedCategoryFromTenant = "Removed Categories from Tenant {0}";
            public const string FailedToRemoveCategory = "Failed to remove Categories to Tenant {0}";
        }
        
    }
}