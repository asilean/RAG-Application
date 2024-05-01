namespace ProjectManagament_WebApp.Sevices
{
    public interface IEmailService
    {
        void SendCodeEmail(string toEmail, string code);
    }
}
