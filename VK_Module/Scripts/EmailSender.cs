using System.Net;
using System.Net.Mail;
using System.Collections.ObjectModel;

namespace VK_Module.Scripts
{
    public class EmailSender
    {
        private string smtpServer = "smtp.mail.ru";
        private int smtpPort = 587;
        private string smtpSenderMail = EMAILConfigurationManager._email;
        private string smtpPassword = EMAILConfigurationManager._smptemailpassword;
        
        public void SendMessage(string recipientMail, string subject, string content, ObservableCollection<AdvertisementFile> images)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpSenderMail, smtpPassword);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(smtpSenderMail);
                        mailMessage.To.Add(recipientMail);
                        mailMessage.Subject = subject;
                        mailMessage.Body = content;
                        if (images != null)
                        {
                            foreach (AdvertisementFile file in images)
                            {
                                Attachment attachment = new Attachment(file.ImagePath);
                                mailMessage.Attachments.Add(attachment);
                            }
                        }
                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
