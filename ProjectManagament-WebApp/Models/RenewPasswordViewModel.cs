namespace ProjectManagament_WebApp.Models
{
    public class RenewPasswordViewModel
    {
        public Guid UserId { get; set; };
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
