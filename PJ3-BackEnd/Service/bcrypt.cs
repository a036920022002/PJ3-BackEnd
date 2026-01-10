namespace PJ3_BackEnd.Service
{
    public interface IPasswordService
    {
        string HashPassword(string password); // 定義動作名稱
        bool VerifyPassword(string password, string hash);
    }

    public class bcrypt : IPasswordService
    {
        public string HashPassword(string password)
        {

            // 這裡寫下 BCrypt 具體的加密邏輯
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            // 這裡寫下 BCrypt 具體的驗證邏輯
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

    }
    

}

