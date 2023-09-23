using Contracts.Services;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class OtpService : IOtpService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public OtpService(IConnectionMultiplexer redis)
    {
        _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        _db = _redis.GetDatabase();
    }

    public async Task<string> GenerateOtpAsync(string key, int expiryInDays = 2)
    {
        // random 6 digit number
        var otp = new Random().Next(100000, 999999).ToString();
        // set to redis
        var otpKey = IOtpService.OtpSchema + ":" + key;
        await _db.StringSetAsync(otpKey, otp, TimeSpan.FromDays(expiryInDays));
        return otp;
    }

    public Task<bool> ValidateOtpAsync(string key, string otp)
    {
        throw new NotImplementedException();
    }

    public string GenerateOtp(string key, int expiryInDays = 2)
    {
        // random 6 digit number
        var otp = new Random().Next(100000, 999999).ToString();
        // set to redis
        var otpKey = IOtpService.OtpSchema + ":" + key;
         _db.StringSet(otpKey, otp, TimeSpan.FromDays(expiryInDays));
        return otp;
    }

    public bool ValidateOtp(string key, string otp)
    {
        throw new NotImplementedException();
    }
}