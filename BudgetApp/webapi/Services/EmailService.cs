using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;

namespace Back_End.Services
{
    public class EmailService
    {
        public static Task SendConfirmationCode(string Email, string Code, IConfiguration IConf)
        {
            var Subject = "Email confirmation code";

            var SenderEmail = IConf.GetSection("EmailService:Sender").Value!;
            var SenderPassword = IConf.GetSection("EmailService:Password").Value!;
            var Host = IConf.GetSection("EmailService:Host").Value!;

            var client = new SmtpClient(Host, 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(SenderEmail, SenderPassword)
            };

            return client.SendMailAsync(new MailMessage(
                from: SenderEmail,
                to: Email,
                Subject,
                Code));

        }

    }
}
