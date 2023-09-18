namespace Tiktok.API.Domain.Common.Constants;

public static class SystemConstants
{
    public const string IdentitySchema = "Identity";

    public static class AppClaims
    {
        public const string Id = "id";
        public const string UserName = "username";
        public const string FullName = "fullname";
        public const string Roles = "roles";
        public const string Permissions = "permissions";
        public const string Email = "email";
    }
}