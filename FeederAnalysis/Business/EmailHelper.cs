using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace FeederAnalysis.Business
{
    public class EmailHelper
    {
        static List<string> lstEmailTo = new List<string>()
        {
            "quyetpv@umcvn.com",
            "hanhvt@umcvn.com",
            "nhungtt@umcvn.com",
            "oqc-pd3@umcvn.com",
            "thanhtv@umcvn.com",
            "khanhdv@umcvn.com",
            "oanhqt@umcvn.com",
            "tuanta@umcvn.com",
            "kiettv@umcvn.com",
            "vannd@umcvn.com",
            "phuctran@umcvn.com",
            "phamphuong@umcvn.com",
            "khanhdv@umcvn.com",
            "dungdv@umcvn.com",
            "Thaodt@umcvn.com"
        };
        static List<string> lstEmailCC = new List<string>()
        {
            "lenpt@umcvn.com",
        };
        public static void SenMailOutlook(string body)
        {
            try
            {
                //var userName = "umc_form_request@umcvn.com";
                //var password = "Umchd@123";
                //var subject = "System";
                var userName = "DXsystem@umcvn.com";
                var password = "Lca@12345";
                var subject = "System";
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.office365.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,

                    TargetName = "STARTTLS/smtp.office365.com",
                    Credentials = new NetworkCredential(userName, password)
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                mailMessage.From = new MailAddress(userName, "DX System");
                foreach (var item in lstEmailTo)
                {
                    mailMessage.To.Add(item);
                    // mailMessage.To.Add("quyetpv@umcvn.com");
                }
                foreach (var item in lstEmailCC)
                {
                    mailMessage.CC.Add(item);
                }
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                smtpClient.SendCompleted += (s, e) =>
                {
                    smtpClient.Dispose();
                    mailMessage.Dispose();
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
        public static void SenMailCali(string body)
        {
            try
            {
                var userName = "umc_form_request@umcvn.com";
                var password = "Umchd@123";
                var subject = "Danh sách thiết bị cần hiệu chuẩn";
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.office365.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,

                    TargetName = "STARTTLS/smtp.office365.com",
                    Credentials = new NetworkCredential(userName, password)
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                mailMessage.From = new MailAddress(userName, "IT System");
                mailMessage.To.Add("huept@umc.co.jp");
                mailMessage.CC.Add("quyetpv@umcvn.com");
                //mailMessage.CC.Add("hungnd@umcvn.com");
                mailMessage.CC.Add("trangpt@umcvn.com");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                smtpClient.SendCompleted += (s, e) =>
                {
                    smtpClient.Dispose();
                    mailMessage.Dispose();
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
        public static void SenMailEdu(string subject, string body)
        {
            try
            {
                var userName = "umc_form_request@umcvn.com";
                var password = "Umchd@123";
                // var subject = "Cảnh báo.";
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.office365.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,

                    TargetName = "STARTTLS/smtp.office365.com",
                    Credentials = new NetworkCredential(userName, password)
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                mailMessage.From = new MailAddress(userName, "IT System");
                mailMessage.To.Add("vn-edu@umcvn.com");
                mailMessage.To.Add("phuongnt_9800@umcvn.com");
                mailMessage.To.Add("thongo@umcvn.com");
                mailMessage.To.Add("thaipt@umcvn.com");
                mailMessage.To.Add("haoct@umcvn.com");
                mailMessage.CC.Add("quyetpv@umcvn.com");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                smtpClient.SendCompleted += (s, e) =>
                {
                    smtpClient.Dispose();
                    mailMessage.Dispose();
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }
        public static void SenMailProfile(string body)
        {
            try
            {
                var userName = "umc_form_request@umcvn.com";
                var password = "Umchd@123";
                var subject = "Cảnh báo thiết bị profile";
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.office365.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,

                    TargetName = "STARTTLS/smtp.office365.com",
                    Credentials = new NetworkCredential(userName, password)
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                mailMessage.From = new MailAddress(userName, "IT System");
                mailMessage.To.Add("quyetpv@umcvn.com");
                mailMessage.To.Add("haupd_31916@umcvn.com");
                mailMessage.To.Add("dungdv@umcvn.com");
                mailMessage.To.Add("tinhtv@umcvn.com");
                mailMessage.To.Add("benvv@umcvn.com");
                mailMessage.To.Add("chungtd@umcvn.com");
                mailMessage.To.Add("thangpv@umcvn.com");
                mailMessage.To.Add("nhungtt@umcvn.com");
                mailMessage.To.Add("thieubv@umcvn.com");
                mailMessage.To.Add("thanhlv@umcvn.com");
                mailMessage.To.Add("khaitm@umcvn.com");
                mailMessage.To.Add("vovx@umcvn.com");
                mailMessage.To.Add("maint_262@umcvn.com");
                mailMessage.To.Add("khinhpt@umcvn.com");
                mailMessage.To.Add("linhtt@umcvn.com");
                mailMessage.To.Add("thangnv@umcvn.com");
                //mailMessage.CC.Add("hungnd@umcvn.com");
                mailMessage.CC.Add("duynq@umcvn.com");
                mailMessage.CC.Add("haoct@umcvn.com");
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                smtpClient.SendCompleted += (s, e) =>
                {
                    smtpClient.Dispose();
                    mailMessage.Dispose();
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw ex;
            }
        }

        internal static void SenMailWORelationship(string mailBody)
        {
            try
            {
                var userName = "DXsystem@umcvn.com";
                var password = "Lca@12345";
                var subject = "WO Relationship System";
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.office365.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,

                    TargetName = "STARTTLS/smtp.office365.com",
                    Credentials = new NetworkCredential(userName, password)
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                mailMessage.From = new MailAddress(userName, "DX System");

                mailMessage.To.Add("hanhnth@umcvn.com"); // hạnh PC
                mailMessage.To.Add("quyetpv@umcvn.com"); // anh quyết:))
                mailMessage.To.Add("nhungtt@umcvn.com"); // ko biết ai
                //mailMessage.CC.Add(item);
                //mailMessage.To.Add("dx_dev_group@umcvn.com"); 

                mailMessage.Subject = subject;
                mailMessage.Body = mailBody;
                mailMessage.IsBodyHtml = true;
                smtpClient.SendCompleted += (s, e) =>
                {
                    smtpClient.Dispose();
                    mailMessage.Dispose();
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                
            }
        }
    }
}
