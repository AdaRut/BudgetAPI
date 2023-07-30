namespace BudgetAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(String message) : base(message)
        {
        }
    }
}
