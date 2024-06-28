using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace onlineLegalWF.Class
{
    public class SendMail
    {
        public async Task<bool> sendEmail(string subject,string mailto, string body, string attachfile)
        {
            bool status = false;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("no_reply@assetworldcorp-th.com", "Aw[@dM1N#")
                };

                
                var mailMessage = new MailMessage { From = new MailAddress("no_reply@assetworldcorp-th.com") };
                mailMessage.To.Add(mailto);
                mailMessage.Body = body;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(attachfile)) 
                {
                    var attachment = new System.Net.Mail.Attachment(attachfile);
                    mailMessage.Attachments.Add(attachment);
                }
                

                await client.SendMailAsync(mailMessage).ConfigureAwait(false);
                status = true;
                return status;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                throw ex;
            }
        }
        public async Task<bool> sendEmails(string subject, string[] mailto, string body, string attachfile)
        {
            bool status = false;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("no_reply@assetworldcorp-th.com", "Awc@2019")
                };

                //var attachment = new System.Net.Mail.Attachment(attachfile);
                var mailMessage = new MailMessage { From = new MailAddress("no_reply@assetworldcorp-th.com") };
                if (mailto.Length > 0) 
                {
                    foreach (var item in mailto)
                    {
                        mailMessage.To.Add(item);
                    }
                }
                mailMessage.Body = body;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                //if (attachment != null)
                if (!string.IsNullOrEmpty(attachfile))
                {
                    var attachment = new System.Net.Mail.Attachment(attachfile);
                    mailMessage.Attachments.Add(attachment);
                }


                await client.SendMailAsync(mailMessage).ConfigureAwait(false);
                status = true;
                return status;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                throw ex;
            }
        }

        public async Task<bool> sendEmailCC(string subject, string mailto,string mailcc, string body, string attachfile)
        {
            bool status = false;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("no_reply@assetworldcorp-th.com", "Awc@2019")
                };

                //var attachment = new System.Net.Mail.Attachment(attachfile);
                var mailMessage = new MailMessage { From = new MailAddress("no_reply@assetworldcorp-th.com") };
                mailMessage.To.Add(mailto);
                mailMessage.CC.Add(mailcc);
                mailMessage.Body = body;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                //if (attachment != null)
                if (!string.IsNullOrEmpty(attachfile))
                {
                    var attachment = new System.Net.Mail.Attachment(attachfile);
                    mailMessage.Attachments.Add(attachment);
                }


                await client.SendMailAsync(mailMessage).ConfigureAwait(false);
                status = true;
                return status;
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
                throw ex;
            }
        }
    }
}