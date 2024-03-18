﻿using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserViewModel>> GetAllAsync();
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task SingOutAsync();
        Task<ServiceResult> RegisterAsync(SaveUserViewModel vm, string origin, string userRole);
        Task<string> ConfirmEmailAsync(string UserId, string token);
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task<ServiceResult> ForgotPasswordAsync(ForgorPasswordViewModel vm, string origin);
    }
}
