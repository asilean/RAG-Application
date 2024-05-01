namespace ProjectManagament_WebApp.Data.Models
{
    public class ForgotPasswordCode
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public User User { get; set; }
    }
}
