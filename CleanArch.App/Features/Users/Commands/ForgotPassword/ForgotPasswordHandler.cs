using CleanArch.App.Features.Users.Commands.ForgotPassword;
using CleanArch.App.Interface;
using CleanArch.App.Services;
using CleanArch.Infra.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, ResponseModel>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordHandler> _logger;

    public ForgotPasswordHandler(UserManager<ApplicationUser> userManager,
                               IEmailService emailService,
                               ILogger<ForgotPasswordHandler> logger)
    {
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<ResponseModel> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"Password reset requested for non-existent email: {request.Email}");
                return ResponseModel.Success("If account exists, email has been sent.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var resetLink = $"https://yourwebsite.com/reset-password?token={encodedToken}&email={user.Email}";

            _logger.LogInformation($"Sending password reset email to {user.Email}");

            await _emailService.SendEmailAsync(
                user.Email,
                "Reset Password",
                $"Click here to reset password: <a href='{resetLink}'>{resetLink}</a>");

            _logger.LogInformation($"Password reset email sent successfully to {user.Email}");

            return ResponseModel.Success("If account exists, email has been sent.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in ForgotPasswordHandler for email: {request.Email}");
            return ResponseModel.Fail("An error occurred while processing your request.");
        }
    }
}