using System.Net.Mail;
using System.Net;
using System.Text;
using ProjectManagament_WebApp.Data;
using ProjectManagament_WebApp.Controllers;

namespace ProjectManagament_WebApp.Sevices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public readonly PMContext _context;
        public UserController User { get; }

        public EmailService(PMContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void SendCodeEmail(string toEmail, string code)
        {
            // Set up SMTP client
            SmtpClient client = new SmtpClient(_configuration.GetValue<string>("Smtp:Host"), _configuration.GetValue<int>("Smtp:Port"));
            client.EnableSsl = _configuration.GetValue<bool>("Smtp:EnableSsl");
            client.UseDefaultCredentials = _configuration.GetValue<bool>("Smtp:UseDefaultCredentials");
            client.Credentials = new NetworkCredential(_configuration.GetValue<string>("Smtp:Email"), _configuration.GetValue<string>("Smtp:Password"));

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration.GetValue<string>("Smtp:Email"));
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Compainer Password Renewal";
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new();
            mailBody.AppendFormat("<h1>Companier</h1>");
            mailBody.AppendFormat("<h3>This code is only valid for 5 minutes.</h3>");
            mailBody.AppendFormat($"<h1>Code: {code}</h1>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}
