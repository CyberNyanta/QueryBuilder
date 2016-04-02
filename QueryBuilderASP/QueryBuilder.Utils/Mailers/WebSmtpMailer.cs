using System;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace QueryBuilder.Utils.Mailers
{
    public class WebSmtpMailer
    {
        private static WebSmtpMailer _instance;
        private static MailSettingsSectionGroup _mailSettings;

        protected WebSmtpMailer()
        {
            _mailSettings = WebConfigurationManager.OpenWebConfiguration("~/web.config").GetSectionGroup("system.net/mailSettings")
                    as MailSettingsSectionGroup;
        }

        public static WebSmtpMailer Instance()
        {
            return _instance ?? (_instance = new WebSmtpMailer());
        }

        public Task SendMailAsync(string addressesTo, string subjectMail, string bodyMail, string[] filesPathes = null)
        {
            if (string.IsNullOrWhiteSpace(addressesTo))
            {
                throw new ArgumentException("Incorrect e-mail address (To). ");
            }

            // Verify mail settings
            if (_mailSettings != null)
            {
                using (var mailClient = new SmtpClient())
                {
                    using (var message = new MailMessage(_mailSettings.Smtp.Network.UserName, addressesTo))
                    {
                        try
                        {
                            message.Subject = subjectMail;
                            message.Body = bodyMail;

                            if (filesPathes != null)
                            {
                                foreach (var filePath in filesPathes)
                                {
                                    if (File.Exists(filePath))
                                    {
                                        // Create  the file attachment for this e-mail message.
                                        var data = new Attachment(filePath, MediaTypeNames.Application.Octet);

                                        // Add the file attachment to this e-mail message.
                                        message.Attachments.Add(data);
                                    }
                                }
                            }

                            return mailClient.SendMailAsync(message);

                        }
                        catch (SmtpException e)
                        {
                            throw new SmtpException("Mail.Send: " + e.Message);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Empty mail settings.");
            }
        }

        public Task SendAsyncRegisterNotification(string email)
        {
            return SendMailAsync(email, "AltexSoft M2T2", "Thank you for registering!", null);
        }

    }
}
