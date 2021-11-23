using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Common.Models;

namespace AttendanceStudent.Commons
{
    public static class Constants
    {
        /// <summary>
        /// To enable database auto increment if needed
        /// </summary>
        public const bool EnableAi = true;
        
        public static class FieldLength
        {
            public const int TextMinLength = 3;
            public const int TextMaxLength = 255;
            public const int MiddleTextLength = 50;
            public const int UrlMaxLength = 1000;
            public const int DescriptionMaxLength = 1000;
            public const int RecaptchaMaxLength = 1000;
            public const int FirebaseTokenMaxLength = 700;
        }

        public static class ProductValue
        {
            public const decimal VolumeValueDefault = 10;
            public const decimal WeightValueDefault = 10;
        }

        public static class CommonFields
        {
            public const string RecaptchaToken = "RecaptchaToken";
        }

        public static class MaterializedPath
        {
            public const int PathLength = 5; // supported up to 50 levels deep
            public const long Max = 60466175; // ZZZZZ = 60 466 175
            public const string MaxString = "ZZZZZ";
            public const long Min = 0;
            public const string MinString = "00000";
        }

        public static class MimeTypes
        {
            public static class Text
            {
                public const string Plain = "text/plain";
                public const string Html = "text/html";
                public const string Xml = "text/xml";
                public const string RichText = "text/richtext";
            }

            public static class Application
            {
                public const string Soap = "application/soap+xml";
                public const string Octet = "application/octet-stream";
                public const string Rtf = "application/rtf";
                public const string Pdf = "application/pdf";
                public const string Zip = "application/zip";
                public const string Json = "application/json";
                public const string Xml = "application/xml";
            }

            public static class Image
            {
                public const string Gif = "image/gif";
                public const string Tiff = "image/tiff";
                public const string Jpeg = "image/jpeg";
            }
        }

        /// <summary>
        /// For ActionLog usage. Each action should be under ObjectName|ActionName format
        /// </summary>
        public static class Actions
        {
         
            public static class Resource
            {
                public static class File
                {
                    public const string Upload = "Resource|File|Upload";
                }
            }
        }

        public static readonly ErrorItem[] CannotFinishRequest = new ErrorItem[]
        {
            new ErrorItem()
            {
                Error = LocalizationString.Common.CannotFinishRequest,
                FieldName = LocalizationString.Common.UnknownFieldName
            }
        };

        public static readonly ErrorItem[] PermissionDenied = new ErrorItem[]
        {
            new ErrorItem()
            {
                Error = LocalizationString.Common.PermissionDenied,
                FieldName = LocalizationString.Common.PermissionFieldName
            }
        };

        public static readonly ErrorItem[] ViewListFailed = new ErrorItem[]
        {
            new ErrorItem()
            {
                Error = LocalizationString.Common.ViewListFailed,
                FieldName = LocalizationString.Common.UnknownFieldName
            }
        };

        public static readonly ErrorItem[] InvalidRecaptcha = new[]
        {
            new ErrorItem()
            {
                Error = LocalizationString.Common.InvalidRecaptcha,
                FieldName = LocalizationString.Common.UnknownFieldName
            }
        };
        

        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };

        
        public static string JsonDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";

        public static class LoginProviders
        {
            public const string Self = "SELF";
            public const string Ldap = "LDAP";
            public const string Google = "GOOGLE";
            public const string Microsoft = "MICROSOFT";
            public const string Facebook = "FACEBOOK";
        }

        public static class SupportedCultures
        {
            public static string[] Cultures =
            {
                English,
                Japanese,
                Vietnamese
            };

            public const string English = "en-US";
            public const string Japanese = "ja-JP";
            public const string Vietnamese = "vi-VN";
        }

        public static class Pagination
        {
            public const int DefaultPage = 1;
            public const int DefaultSize = 30;
            public const bool DefaultOrderByDesc = false;
            public const int DefaultCurrentPage = 0;
            public const int DefaultTotalPages = 0;
            public const int DefaultTotalItems = 0;
            public const string DefaultOrderBy = "";
        }

        public static class Permissions
        {
            // Structure area:module:perm
            public const string SysAdmin = "ROOT:ROOT:SYSADMIN";

            public const string TenantAdmin = "TENANT:ROOT:ADMIN";

            //public const string TenantUser = "TENANT:ROOT:USER";
            public const string UnlockAccount = "IDENTITY:ACCOUNT:UNLOCK";
            public const string LockAccount = "IDENTITY:ACCOUNT:LOCK";
        }

        public static class DefaultGuids
        {
            public static Guid SystemAccount = Guid.Parse("49e3275a-d497-4b45-bbcb-3214f3769d7f");
            public static Guid SystemAccountRole = Guid.Parse("49e3275a-d497-4b45-bbcb-3214f3769d7e");
            public static Guid TenantAdminRole = Guid.Parse("49e3275a-d497-4b45-bbcb-3214f3769d6e");
            public static Guid SystemAccountPermission = Guid.Parse("49e3275a-d497-4b45-bbcb-3214f3769d6e");

            public static Guid TenantAdminPermission = Guid.Parse("49e3275a-d497-4b45-bbcb-3214f3769d9e");
            //public static Guid TenantUser = Guid.Parse("49e3275a-d497-4b45-bbcb-3214f3769d6e");
        }

        public static class BackgroundService
        {
            public static class Queue
            {
                public const string Notification = "notification";
                public const string Reminder = "reminder";
                public const string Default = "default";
                public const string Synchronization = "synchronization";
            }
        }
    }
}