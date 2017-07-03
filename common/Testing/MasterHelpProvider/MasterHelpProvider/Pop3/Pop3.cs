using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace TestMasterHelpProvider.Pop3
{
    public class Pop3
    {

        #region " Variables "
        Utilities.Email.Pop3.Pop3MailClient Client;

        private string strPopServer = "pop.gmail.com";
        private int intPort = 995;
        private bool blnUseSSL = true;
        private string strEmail = "qa.automation.ns@gmail.com";
        private string strPassword = "n3tst3ps$";
        #endregion

        #region " Constructors "
        public Pop3(string PopServer, int Port, bool UseSSL, string Email, string Password)
        {
            strPopServer = PopServer;
            intPort = Port;
            blnUseSSL = UseSSL;
            strEmail = Email;
            strPassword = Password;
        }

        public Pop3()
        {

        }
        #endregion

        private void Connect()
        {
            Client = new Utilities.Email.Pop3.Pop3MailClient(strPopServer, intPort, blnUseSSL, strEmail, strPassword);
            Client.IsAutoReconnect = true;
            Client.Connect();
        }

        private void Disconnect()
        {
             Client.Disconnect();
        }

        public void WaitForNewEmail()
        {
            int i = 0;

            while (i == 0)
            {
                i = NewEmailCount();
                if (i >= 1)
                {
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(30000);
                }
            }
        }

        public void SaveEmail()
        {
            WaitForNewEmail();

            SaveEmailAsHTML(false);
        }

        public void SaveEviteEmail()
        {
            WaitForNewEmail();

            SaveEmailAsHTML(true);
        }

        public void DeleteAllEmails()
        {
            try
            {

                SortedList<string, int> EmailIDs;

                EmailIDs = GetEmailIDs(); //get email count

                do
                {
                    DeleteEmail(EmailIDs); // delete all emails

                    EmailIDs.Clear(); //reset for next loop

                    EmailIDs = GetEmailIDs(); //get email count (google only feeds a count of 250 max even if there are thousands of emails)
                }
                while (EmailIDs.Count >= 1);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteEmail(SortedList<string, int> EmailIDs)
        {
            try
            {

                Connect();

                foreach (KeyValuePair<string, int> key in EmailIDs) //mark all emails as deleted, wont take affect til disconnect
                {
                    Client.DeleteEmail(key.Value);
                }

                Disconnect();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public void SaveEmailAsHTML(Boolean blnFilterForInvite)
        {
            string strBody;
            try
            {
                strBody = DownloadEmail(blnFilterForInvite);

                strBody = "<html>" + System.Environment.NewLine +
                    "<body>" + System.Environment.NewLine + strBody +
                    "</body>" + System.Environment.NewLine + "</html>";

                if (blnFilterForInvite == false)
                {
                    File.WriteAllText("C:\\TestFiles\\Email.html", strBody);
                }
                else
                {
                    File.WriteAllText("C:\\TestFiles\\Invitation.html", strBody);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SaveEmailAsHTML(string strBody)
        {
            try
            {

                strBody = "<html>" + System.Environment.NewLine +
                    "<body>" + System.Environment.NewLine + strBody +
                    "</body>" + System.Environment.NewLine + "</html>";

                File.WriteAllText("C:\\TestFiles\\Invitation.html", strBody);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public SortedList<string, int> GetEmailIDs()
        {
            try
            {

                Connect();

                SortedList<string, int> EmailIDs;
                //Client.GetEmailIdList(out EmailIDs);
                Client.GetUniqueEmailIdList(out EmailIDs);

                Disconnect();

                return EmailIDs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Int32 NewEmailCount()
        {
            try
            {

                Connect();

                List<Utilities.Email.Pop3.Message> Messages = Client.GetMessageList();

                Disconnect();

                return Messages.Count;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ArrayList DownloadEmails()
        {
            try
            {
                ArrayList objArray = new ArrayList();

                Connect();

                List<Utilities.Email.Pop3.Message> Messages = Client.GetMessageList();

                for (int i = 1; i <= Messages.Count; i++)
                {
                    Utilities.Email.Pop3.Message Message = Client.GetMessage(Messages[i - 1]);

                    objArray.Add(Message);
                }

                Disconnect();

                return objArray;

            }

            catch (Exception ex)
            {
                throw;
            }
        }

        public string DownloadEmail(Boolean blnFilterForInvite)
        {
            string strBody = String.Empty;
            try
            {

                Connect();

                List<Utilities.Email.Pop3.Message> Messages = Client.GetMessageList();

                for (int i = 1; i <= Messages.Count; i++)
                {
                    Utilities.Email.Pop3.Message Message = Client.GetMessage(Messages[i - 1]);
                    string strFrom = Message.MessageBody.From;
                    string strSubject = Message.MessageBody.Subject;
                    if (blnFilterForInvite == true)
                    {
                        //if ((strFrom.Contains("corporate@scentsy.us")) && (strSubject.Contains("Invitation")))
                        if(strSubject.Contains("Invitation"))
                        {
                            //strBody = Message.MessageBody.BodyText;
                            strBody = Message.MessageBody.Body.Boundries[1].BodyText;
                            break;
                        }
                    }
                    else
                    {
                        strBody = Message.MessageBody.Body.Boundries[1].BodyText;
                        break;
                    }

                }

                Disconnect();

                return strBody;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
