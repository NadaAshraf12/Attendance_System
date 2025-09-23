using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Users.Commands.ChangeEmail
{
    public class ChangeEmailHandler : IRequestHandler<ChangeEmailCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseModel _response;

        public ChangeEmailHandler(UserManager<ApplicationUser> userManager, IResponseModel response)
        {
            _userManager = userManager;
            _response = response;
        }

        public async Task<ResponseModel> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            // البحث عن المستخدم باستخدام الإيميل الحالي
            var user = await _userManager.FindByEmailAsync(request.CurrentEmail)
                       ?? throw new KeyNotFoundException("User not found");

            // إنشاء رمز التحقق لتغيير الإيميل
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

            // محاولة تغيير الإيميل
            var result = await _userManager.ChangeEmailAsync(user, request.NewEmail, token);

            if (!result.Succeeded)
                return _response.Response(400, true, "Failed to change email.", null);

            return _response.Response(200, false, "Email changed successfully.", null);
        }
    }
}
