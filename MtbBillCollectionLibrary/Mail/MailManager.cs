using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace MtbBillCollectionLibrary.Mail
{
    public class MailManager
    {
        public string GetEmailBodyForNewUser(string customerFullName,string userCode,string password,string urlForActivaetAcc)
        {
            StringBuilder message = new StringBuilder();
            message.Append("Dear ");
            message.Append(customerFullName);
            message.Append(",\n\nYou are suucessfully register for MTB Web Service.");
            message.Append("\nPlease keep safe following informations:\n");
            message.Append("\nUser Id: ");
            message.Append(userCode);
            message.Append("\nPassword: ");
            message.Append(password);
            message.Append("\n\nPlease click the follwing link and follow the instruction to activate yor account.\n ");
            message.Append(urlForActivaetAcc);
            message.Append("\n\nPlease contact Admin for any kind of assistance.");
            message.Append("\n\nThank you.");
            return message.ToString();
        }

        public string GetEmailSubForNewUser()
        {
            string msgSub = "Information about account Opening for MTB Web Service on Mutual Trust Bank Ltd.";
            return msgSub;
        }

        public bool SendMail(Entity.EMail email)
        {
            MailAddress to = new MailAddress(email.ReciverName);
            MailAddress from = new MailAddress(email.SenderName);
            MailMessage message = new MailMessage(from, to);
            message.Subject = email.EmailMessege.MessageSubject;
            message.Body =  email.EmailMessege.MessageBody;
            //192.168.32.37  mutualtrustbank.com
            SmtpClient client = new SmtpClient(email.SMTPServerName);            
            if (email.IsDefaultSecurityEnable == false)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(email.NetworkUserName, email.NetworkUserPassword);
            }
            
            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return false;
        }
    }
}
