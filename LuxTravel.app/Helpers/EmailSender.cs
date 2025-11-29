using System.Net;
using System.Net.Mail;

namespace LuxTravel.app.Helpers;

public class EmailSender
{
    public void SendVerificationCode(string to, string subject, string content)
    {
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("g.kokolashvili017@gmail.com", "gedw qsim gvih qwfv");

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("g.kokolashvili017@gmail.com");
        mailMessage.To.Add(to);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = content;

        smtpClient.Send(mailMessage);
    }

    public void SendBookingConfrimation(string to, string subject, string content)
    {
        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("g.kokolashvili017@gmail.com", "gedw qsim gvih qwfv");

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("g.kokolashvili017@gmail.com");
        mailMessage.To.Add(to);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        mailMessage.Body = content;

        smtpClient.Send(mailMessage);
    }
}
