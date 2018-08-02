using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Utility.Mail
{
   public class EmailInfo
    {
        /// <summary>
        /// 发送邮件的地址
        /// </summary>
       public string ServerEmailAddress { get; set; }
        /// <summary>
        /// 发送邮件的密码
        /// </summary>
        public string ServerEmailPwd { get; set; }
        /// <summary>
        /// 邮件服务器主机  
        /// </summary>
        public string ServerEmailHost { get; set; }
        /// <summary>
        /// 邮件服务器端口
        /// </summary>
        public int ServerEmailPort { get; set; }
        /// <summary>
        /// 接收邮件的地址
        /// </summary>
        public string EmailToAddress { get; set; }
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string EmailSubject { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmailContent { get; set; }
        /// <summary>
        /// 邮件体是否为html编码
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// 启用 Ssl
        /// </summary>
        public bool EnableSsl { get; set; }
    }
}
