namespace BudgetAPI
{
    public class AuthenticationSettings
    {
        public String JwtKey { get; set; }
        public int JwtExpireHours { get; set; }
        public String JwtIssuer { get; set; }
    }
}
