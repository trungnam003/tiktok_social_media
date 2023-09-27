using MediatR;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Users.Commands.ChangePassword;

public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordCommand, ApiSuccessResult<string>>
{
    private readonly IUserRepository _userRepository;

    public ChangeUserPasswordHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiSuccessResult<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.ChangePasswordAsync(request.GetUserId(), request.OldPassword, request.NewPassword);
        return new ApiSuccessResult<string>("Change password successfully");
    }
}