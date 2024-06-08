using API.DTOs.Out;
using Application.DTOs;

namespace API.Converters
{
    public static class GetUserDTOConverter
    {
        /// <summary>
        /// Maps intrnal model to external contract model.
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static GetUserDTOOut ToOutModel(this GetUserDTO from)
        {
            return new GetUserDTOOut(from.Id);
        }
    }
}
