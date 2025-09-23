using CleanArch.Infra.Identity;
using Mapster;

namespace CleanArch.App.Features.Users.Commands.UpdateProfile
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<ApplicationUser, UserProfileDto>();
        }
    }
}
