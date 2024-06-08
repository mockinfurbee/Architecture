using API.DTOs.In;
using Application.DTOs;

namespace API.Converters
{
    public static class AuthUserDTOConverter
    {
        /// <summary>
        /// Maps external contract to internal model.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static AuthUserDTO ToServiceModel(this AuthUserDTOIn from)
        {
            return new AuthUserDTO(from.Guid, from.Password);
        }
    }
}
