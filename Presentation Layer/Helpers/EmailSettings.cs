using Data_Access_Layer.Models;
using System.Net;
using System.Net.Mail;

namespace Presentation_Layer.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient(" smtp.libero.it", 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("kirollosmikhail@libero.it", "P@assw0rd351");
            Client.Send("kirollosmikhail@libero.it", email.To, email.Subject, email.Body);

        }
    }
}
