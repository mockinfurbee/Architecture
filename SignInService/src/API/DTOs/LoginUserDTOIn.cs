namespace API.DTOs
{
    public record LoginUserDTOIn(string Guid, string Password, bool RememberMe);
}
