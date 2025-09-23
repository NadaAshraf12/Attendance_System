using CleanArch.App.Interface.Auth;
using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Common.Dtos;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArch.App.Features.Users.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, ResponseModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwt;
        private readonly IResponseModel _response;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwt,
            IResponseModel response,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwt = jwt;
            _response = response;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ جلب المستخدم
            var user = await _userManager.FindByEmailAsync(request.Email)
                       ?? throw new KeyNotFoundException("User not found");

            // 2️⃣ التحقق من كلمة السر مع lockout
            var passCheck = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

            if (passCheck.IsLockedOut)
                throw new UnauthorizedAccessException("User account locked out. Try again later.");

            if (!passCheck.Succeeded)
                throw new UnauthorizedAccessException("Invalid credentials");

            // 3️⃣ جلب IP و Device تلقائي إذا مش موجودين
            var ipAddress = request.IpAddress ?? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var device = request.Device ?? _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            // 4️⃣ إنشاء الـ JWT
            AuthResultDto tokens = await _jwt.CreateTokenAsync(
                user,
                device: device,
                deviceId: request.DeviceId,
                ipAddress: ipAddress,
                cancellationToken: cancellationToken);

            // 5️⃣ إعادة الاستجابة
            return _response.Response(200, false, "Login successful.", tokens);
        }
    }
}
