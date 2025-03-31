
using MailKit.Net.Smtp;
using MimeKit;
using PruebaTecnica.Shared.Responses;


namespace PruebaTecnica.API.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }

}
