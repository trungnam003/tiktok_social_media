namespace Tiktok.API.Domain.Services;

public interface IOtpService
{
    static readonly string OtpSchema = "otp";
    Task<string> GenerateOtpAsync(string key, int expiryInDays = 2);
    /// <summary>
    /// 0 - otp not found or expired
    /// 1 - otp matched
    /// -1 - otp not matched
    /// </summary>
    /// <param name="key">email</param>
    /// <param name="otp"></param>
    /// <returns>0 1 or -1</returns>
    Task<int> ValidateOtpAsync(string key, string otp);
    
    Task DeleteOtpAsync(string key);
    
    string GenerateOtp(string key, int expiryInDays = 2);
    bool ValidateOtp(string key, string otp);
}