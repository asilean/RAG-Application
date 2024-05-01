namespace ProjectManagament_WebApp.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<ConversationHistory> ConversationHistories { get; set; }
        public List<ForgotPasswordCode> ForgotPasswordCodes { get; set; }
    }
}
