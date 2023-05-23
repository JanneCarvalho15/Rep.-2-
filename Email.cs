using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CBR
{
     public class Email
     {
        public string Provedor { get; private set; }

        public string Username { get; private set; }

         public string PassWord { get; private set; }

        public Email(string provedor, string username, string passWord)
        {
            Provedor = provedor ?? throw new ArgumentNullException (nameof (provedor));
            Username = username ?? throw new ArgumentNullException (nameof (username));
            PassWord = passWord ?? throw new ArgumentNullException (nameof (passWord));
        }

        public void CBR (List<string> emailsTo, string subject, string body, List<string> attachment)

        {
            var message = PrepareteMessage (emailsTo, subject, body, attachment);

            CBRbySmtp(message);
        }
        private MailMessage PrepareteMessage(List<string> emailsTo, string subject, string body, List<string> attachments)        {      
            var mail = new MailMessage();
            mail.From = new MailAddress(Username);
         
            foreach (var email in emailsTo)
            {
                if (ValidateEmail(email))
                {
                    mail.To.Add(email);
                }
            }
              
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            foreach (var file in attachments)
            {
                var data = new Attachment(file, MediaTypeNames.Application.Octet);
                ContentDisposition disposition = data.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(file);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(file);

                mail.Attachments.Add(data);
            }

            return mail;
        }

        private bool ValidateEmail(string email)
        {
            Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
            if (expression.IsMatch(email))
                return true;

            return false;
        }

        private void CBRbySmtp(MailMessage message)

            {
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
                smtpClient.Host = Provedor;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 50000;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(Username, PassWord);
                smtpClient.Send(message);
                smtpClient.Dispose();
            }
                         
        }

    }

