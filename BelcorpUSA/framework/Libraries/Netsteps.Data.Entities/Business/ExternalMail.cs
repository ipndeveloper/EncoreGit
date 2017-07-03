using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using MailBee.SmtpMail;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities
{
    public class ExternalMail
    {
        public static void SendMailConfirmPolicy(byte[] pAttachment, string pNameAttachment, string pExtensionAttachment, Account pAccount, int pLanguageParse)
        {
            string smtpServerName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpServer);
            int smtpPort = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.SmtpPort, 25);
            bool useSMTPAuthentication = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseSmtpAuthentication);
            string smtpUserName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpUserName);
            string smtpPassword = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpPassword);

            Smtp.LicenseKey = "MN200-9F57689E57B057AB577699C15A6B-9A7C";
            var smtp = new Smtp();
            var smtpServer = useSMTPAuthentication
                    ? new SmtpServer(smtpServerName, smtpUserName, smtpPassword)
                    : new SmtpServer(smtpServerName);
            smtpServer.Port = smtpPort;
            smtp.SmtpServers.Add(smtpServer);
            MailBee.Mime.MailMessage mail = smtp.Message;
            var template = EmailTemplate.GetFirstTemplateByTemplateTypeID(NetSteps.Data.Entities.Constants.EmailTemplateType.Terms.ToShort());
            if (template != null)
            {
                //var translation = template.EmailTemplateTranslations.GetByLanguageID(pAccount.DefaultLanguageID);/*CS12JUN2016.Comentado*/
                var translation = template.EmailTemplateTranslations.GetByLanguageID(pLanguageParse);
                if (translation != null)
                {
                    mail.To.Add(pAccount.EmailAddress);
                    mail.Attachments.Add(pAttachment, pNameAttachment, "", pExtensionAttachment,
                        null, MailBee.Mime.NewAttachmentOptions.None, MailBee.Mime.MailTransferEncoding.None);
                    mail.From.Email = translation.FromAddress;
                    mail.Subject = translation.Subject;
                    mail.BodyHtmlText = translation.Body;
                    smtp.Send();
                }
                else
                {
                    throw new InvalidOperationException(String.Format("There is no 'Terms' email template translation for LanguageId:{0} defined.", pAccount.DefaultLanguageID));
                }
            }
            else
            {
                throw new InvalidOperationException("There is no 'Terms' email template defined.");
            }
        }

        public static void SendMailSupportTicket(SupportTicket pTicket, Business.SupportTicketsBE pTicketBE, string NotifyTo, int pLanguageParse)
        {
            string smtpServerName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpServer);
            int smtpPort = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.SmtpPort, 25);
            bool useSMTPAuthentication = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseSmtpAuthentication);
            string smtpUserName = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpUserName);
            string smtpPassword = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.SmtpPassword);

            Smtp.LicenseKey = "MN200-9F57689E57B057AB577699C15A6B-9A7C";
            var smtp = new Smtp();
            var smtpServer = useSMTPAuthentication
                    ? new SmtpServer(smtpServerName, smtpUserName, smtpPassword)
                    : new SmtpServer(smtpServerName);
            smtpServer.Port = smtpPort;
            smtp.SmtpServers.Add(smtpServer);
            MailBee.Mime.MailMessage mail = smtp.Message;
            var template = EmailTemplate.GetFirstTemplateByTemplateTypeID(NetSteps.Data.Entities.Constants.EmailTemplateType.SupportTicketInfoRequestAutoresponder.ToShort());
            if (template != null)
            {
                if (pTicket.AssignedUserID != null)
                {
                    //Buscar mail del responsable asignado
                    CorporateUser cUser = (new Repositories.CorporateUserRepository()).LoadByUserIdFull(pTicketBE.AssignedUserID);

                    var translation = template.EmailTemplateTranslations.GetByLanguageID(pLanguageParse);
                    if (translation != null)
                    {
                        string htmGestion = "";
                        string htmProperties = "";

                        //Buscar información de cuenta del consultor
                        pTicket.Account = Account.LoadFull(pTicket.AccountID);

                        // Buscar Información de gestión y propiedades del ticket
                        List<Business.SupportMotivePropertyTypes> ticketProperties = SupportTicket.ListarSupportMotivePropertyTypesPorMotivo(pTicketBE.SupportMotiveID, pTicket.SupportTicketID);                     
                        List<Business.SupportTicketGestionBE> TicketGestionBE = SupportTicket.ListarSupportTicketGestionBE(pTicket.SupportTicketID);
                        string Categoria = "";                        

                        Categoria = Business.SupportMotives.ConcatenarSupportLevel(pTicketBE.SupportLevelID);
                        Categoria += Repositories.SupportMotiveDataAccess.Search().Find(x => x.SupportMotiveID == pTicketBE.SupportMotiveID).Name;             

                        if (TicketGestionBE.Count > 0)
                        {
                            htmGestion += "<table class=\"tblinfo\" align=\"center\">";
                            htmGestion += "<tr class=\"thinfo\">";
                            htmGestion += "<td>Data</td>";
                            htmGestion += "<td>Descricao</td>";
                            htmGestion += "<td>Situacao</td>";
                            htmGestion += "<td>Usuario</td>";
                            htmGestion += "</tr>";

                            foreach (Business.SupportTicketGestionBE gestion in TicketGestionBE)
                            {
                                htmGestion += "<tr>";
                                htmGestion += "<td>" + gestion.DateCreatedUTC.ToString("dd/MM/yyyy hh:mm tt") + "</td>";
                                htmGestion += "<td>" + gestion.Descripction + "</td>";
                                htmGestion += "<td>" + gestion.NameStatus + "</td>";
                                htmGestion += "<td>" + gestion.Username + "</td>";
                                htmGestion += "</tr>";
                            }
                            htmGestion += "</table>";
                        }

                        if ( ticketProperties.Count > 0)
                        {
                            htmProperties += "<table class=\"tblinfo\" align=\"center\">";
                            htmProperties += "<tr class=\"thinfo\">";
                            htmProperties += "<td>Perguntas</td>";
                            htmProperties += "<td>Respostas</td>";                            
                            htmProperties += "</tr>";
                            foreach (Business.SupportMotivePropertyTypes property in ticketProperties)
                            {
                                htmProperties += "<tr>";
                                htmProperties += "<td>" + property.Name + "</td>";
                                htmProperties += "<td>" + property.PropertyValue +"</td>";                               
                                htmProperties += "</tr>";
                            }
                            htmProperties += "</table>";
                        }

                        //Agregar datos a la plantilla
                        string subjectMail = "";
                        string msgMail = "";
                        subjectMail = translation.Subject.Replace("{ticket_id}", pTicket.SupportTicketNumber);
                        subjectMail = subjectMail.Replace("{motive_name}", Categoria);
                        subjectMail = subjectMail.Replace("{account_id}", pTicket.Account.AccountNumber);
                        subjectMail = subjectMail.Replace("{account_name}", pTicket.Account.FullName);

                        msgMail = translation.Body.Replace("{ticket_id}", pTicket.SupportTicketNumber);
                        msgMail = msgMail.Replace("{motive_name}", Categoria);
                        msgMail = msgMail.Replace("{account_id}", pTicket.Account.AccountNumber);
                        msgMail = msgMail.Replace("{account_name}", pTicket.Account.FullName);
                        msgMail = msgMail.Replace("{descripcion}", pTicket.Description);
                        msgMail = msgMail.Replace("{properties}", htmProperties);
                        msgMail = msgMail.Replace("{gestion}", htmGestion);
                        msgMail = Encoding.UTF8.GetString(Encoding.BigEndianUnicode.GetBytes(msgMail));

                        mail.Charset = "utf-8";
                        mail.To.Add(cUser.Email);
                        mail.Cc.Add(NotifyTo);
                        mail.From.Email = translation.FromAddress;
                        mail.Subject = subjectMail;
                        mail.BodyHtmlText = msgMail;
                        smtp.Send();
                    }
                    else
                    {
                        throw new InvalidOperationException(String.Format("There is no 'Support' email template translation for LanguageId:{0} defined.", pLanguageParse));
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("There is no 'Support' email template defined.");
            }
        }
    }
}

