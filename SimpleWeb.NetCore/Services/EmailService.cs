using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Services
{
    public class EmailService
    {
        System.Net.Mail.SmtpClient client;

        public EmailService()
        {
            client = new System.Net.Mail.SmtpClient();
            client.Host = "localhost";

            //设置是否需要发送是否需要身份验证，如果不需要下面的credentials是不需要的
            client.UseDefaultCredentials = false;
        }


        public async Task<bool> SendConfirmEmail(IdentityUser user, string url)
        {
            try
            {
                var message = new StringBuilder();

                message.Append("亲爱的" + user.UserName + "您好：<br/><br/>");
                message.Append("点击以下链接激活你的账号。<br/><br/>");
                message.Append($"<a href =\"{url}\">点击该URL链接激活你的账号</a><br/><br/>");
                message.Append("(如果无法点击该URL链接地址，请将它复制并粘帖到浏览器的地址输入框，然后单击回车即可。)<br/><br/>");
                message.Append("注意:请您在收到邮件24小时内使用，否则该链接将会失效。<br/><br/>");
                message.Append("我们将一如既往、热忱的为您服务！<br/><br/>");


                //实例化一个邮件消息对象 
                System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();

                //发件人地址
                Message.From = new System.Net.Mail.MailAddress("no-reply@locahost");

                //将邮件发送给管理员
                Message.To.Add(user.Email);

                //邮件主题
                Message.Subject = "Confrim your email address";

                //邮件体（内容）
                Message.Body = message.ToString();

                //设置邮件标题的编码
                Message.SubjectEncoding = System.Text.Encoding.UTF8;

                //设置邮件内容的编码
                Message.BodyEncoding = System.Text.Encoding.UTF8;

                //指定邮件优先级 (三种级别)
                Message.Priority = System.Net.Mail.MailPriority.High;

                //获取或设置一个值，该值指示电子邮件正文是否为 HTML。
                Message.IsBodyHtml = true;

                //发送邮件
                await client.SendMailAsync(Message);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
