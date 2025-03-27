using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

namespace ApiOwn.Services;

public class EmailService
{
    public bool Send(
        string toName, 
        string toEmail, 
        string subject, 
        string body, 
        string fromName = "Equipe BM")
    {
        var smtpClient = new SmtpClient(Configuration.Smtp.Host, Configuration.Smtp.Port);
        
        smtpClient.Credentials = new System.Net.NetworkCredential(Configuration.Smtp.UserName, Configuration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;
        
        var fromEmail = Configuration.Smtp.UserName;
        var mail = new MailMessage();
        
        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }
}