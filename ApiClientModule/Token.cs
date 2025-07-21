

namespace ApiClientModule
{
    public class Token
    {
        public TokenTypeEnum TokenType { get; set; }
        public required string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.MaxValue;
        public int ExpiresIn { get; set; }
        public Result Result { get; set; }
    }

    public class Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum TokenTypeEnum
    {
        Bearer = 0
    }
}
