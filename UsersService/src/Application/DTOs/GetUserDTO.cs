using Application.Interfaces.Entities;
using Application.Mapping;

namespace Application.DTOs
{
    public record GetUserDTO : IMapFrom<IUser>
    {
        public string Id { get; set; }

        public GetUserDTO() { }

        public GetUserDTO(string id)
        {
            Id = id;
        }
    }
}
