using Application.Interfaces.Entities;
using Application.Mapping;

namespace API.DTOs.Out
{
    public record GetUserDTOOut : IMapFrom<IUser>
    {
        public string Id { get; set; }

        public GetUserDTOOut() { }

        public GetUserDTOOut(string id)
        {
            Id = id;
        }
    }
}
