using API.DTOs;
using Application.DTOs;

namespace API.Converters
{
    public static class LoginUserDTOConverter
    {
        public static LoginUserDTO ToServiceModel(this LoginUserDTOIn from)
        {
            return new LoginUserDTO(from.Guid, from.Password, from.RememberMe);
        }
    }
}
