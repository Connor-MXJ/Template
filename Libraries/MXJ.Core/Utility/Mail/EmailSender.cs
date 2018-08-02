using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Mail
{
    /// <summary>
    /// 邮件发送
    /// </summary>
   public class EmailSender
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="model"></param>
        /// <returns>是否成功</returns>
        public static bool Send(EmailInfo model)
        {
            try
            {
                var mail = new MailMessage(model.ServerEmailAddress, model.EmailToAddress);
                mail.IsBodyHtml = model.IsBodyHtml;
                mail.Body = model.EmailContent;
                mail.BodyEncoding = Encoding.UTF8;
                mail.Subject = model.EmailSubject;
                string mailServerName = model.ServerEmailHost;
                var mailClient = new SmtpClient(mailServerName);
                mailClient.Port = model.ServerEmailPort;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = new NetworkCredential(model.ServerEmailAddress, model.ServerEmailPwd);
                mailClient.Send(mail);
                mail.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
