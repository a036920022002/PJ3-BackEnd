namespace PJ3_BackEnd.Dtos
{
    public class AuthPostDto
    {
        public string email { get; set; } = null!;

        public string password { get; set; } = null!;

        public string? name { get; set; }

        public string? role { get; set; } = "viewer";
    }
}
