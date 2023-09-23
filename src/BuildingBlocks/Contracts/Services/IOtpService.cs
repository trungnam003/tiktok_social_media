namespace Contracts.Services;

public interface IOtpService
{
    static readonly string OtpSchema = "otp";
    Task<string> GenerateOtpAsync(string key, int expiryInDays = 2);
    Task<bool> ValidateOtpAsync(string key, string otp);
    
    string GenerateOtp(string key, int expiryInDays = 2);
    bool ValidateOtp(string key, string otp);
}