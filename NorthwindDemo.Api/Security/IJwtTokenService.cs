namespace NorthwindDemo.Api.Security
{
    public interface IJwtTokenService
    {
        string CreateToken(string username, string role);
    }
}
