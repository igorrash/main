using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using External;
using Util;
using System.Configuration;

namespace Send
{
    public class SendNotifications
    {
        private static Log _log;
        private Stopwatch stopwatch = new Stopwatch();


        public SendNotifications()
        {
            _log = new Log { LogFile = "SendExternal" };
            var thread = new Thread(GetSendItems);
            thread.Start();
        }

        private void GetSendItems()
        {
            try
            {
                //send live message
                SentEmail("9192084623", "@txt.att.net", "Live:" + DateTime.Now, string.Empty);

                //log
                System.IO.File.WriteAllText(@"C:\Users\Public\WriteText.txt", "GetSendItems");
                _log.LogMsg("GetSendItems");
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                GetSendItems();
            }

            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];
            var database = ConfigurationManager.AppSettings["database"];

            var connect = new DbConnect(database, username, password);
            while (true)
            {
                var errorMsg = string.Empty;
                var receipients = new List<string>();
                var errorOccured = false;
                try
                {
                    ServerIsLive(connect);
                    var dt =
                        connect.RunSql("SELECT DISTINCT n.noticeId, msg.subject, msg.content, m.phoneNumber, m.firstName, m.LastName, p.email " +
                                       " FROM SEND_GROUPS g, SEND_MEMBERS m, SEND_MESSAGES msg, SEND_NOTICES n, SEND_PHONE_SERVICE_PROVIDERS p " +
                                       " where n.sent = 'N' " +
                                       " AND n.sendDateTime <= CONVERT_TZ(sysdate(), 'MST','EST')" +
                                       " AND g.groupName = n.groupName " +
                                       " AND m.memberId IN (Select gg.memberId FROM SEND_GROUPS gg WHERE gg.groupName = g.groupName) " +
                                       " AND msg.messageId = n.messageId " +
                                       " AND p.provider = m.phoneServiceProvider");

                    foreach (DataRow row in dt.Rows)
                    {
                        SentEmail(row["phoneNumber"].ToString(), row["email"].ToString(), row["subject"].ToString(),
                            row["content"].ToString());
                        var update =
                            "UPDATE SEND_NOTICES SET sent = \'Y\', sentOn = CONVERT_TZ(sysdate(), 'MST','EST')  where noticeId = " +
                            row["noticeId"];

                        connect.RunSql(update);

                        receipients.Add(row["firstName"] + ", " + row["lastName"]);
                        _log.LogMsg(row["firstName"] + ", " + row["lastName"] + ": " + row["phoneNumber"] +
                                    row["email"] + row["subject"] +
                                    row["content"]);

                        Thread.Sleep(10000);
                    }
                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    errorOccured = true;
                    errorMsg += ex.Message;
                    _log.LogMsg("Error in GetSendItems");
                    _log.LogMsg(ex.Message + DateTime.Now);
                }

                if (receipients.Count != 0 && !errorOccured)
                {
                    //send summary message
                    var body = receipients.Aggregate("count=" + receipients.Count,
                        (current, receipient) => current + (receipient + "\r\n"));
                    body += errorMsg;
                    SentEmail("9192084623", "@txt.att.net", "Summary" + DateTime.Now, body);
                }
            }
        // ReSharper disable once FunctionNeverReturns
        }

        private void ServerIsLive(DbConnect connect)
        {
            if(stopwatch.IsRunning && stopwatch.ElapsedMilliseconds < 1000*6)
                return;

            stopwatch.Reset();
            stopwatch.Start();

            var winId = System.Security.Principal.WindowsIdentity.GetCurrent();
            var userName = winId == null ? string.Empty : winId.Name;

            connect.RunSql("DELETE FROM IS_SERVER_LIVE WHERE serverId = '" + userName + "'");
            connect.RunSql("INSERT INTO IS_SERVER_LIVE (serverId, serverName, isLive) VALUES ('" + userName + "','" +
                           userName + "','Y')");
        }

        private static void SentEmail(string phoneNumber, string atEmail, string subject, string body)
        {
            try
            {
                _log.LogMsg(string.Format("PhoneNumber:{0}, EmailProvider:{1}, subject:{2}, body:{3}",
                    phoneNumber, atEmail, subject,body));
                const string smtpAddress = "smtp.mail.yahoo.com";
                const int portNumber = 587;

                var churchEmail = ConfigurationManager.AppSettings["churchEmail"];
                var churchPassword = ConfigurationManager.AppSettings["churchEmailPwd"];
                string emailTo = phoneNumber.Trim() + atEmail;

                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(churchEmail);
                    mail.To.Add(emailTo);
                    mail.Subject = subject.ToLower().Trim() == "cor" ? subject + DateTime.Now.Date : subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;
                    // Can set to false, if you are sending pure text.

                    // mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                    // mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                    var smtp = new SmtpClient(smtpAddress, portNumber)
                    {
                        Credentials = new NetworkCredential(churchEmail, churchPassword),
                        EnableSsl = true
                    };

                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                _log.LogMsg("Error in SentEmail");
                _log.LogMsg(ex.Message);
            }
            
        }
    }
}
