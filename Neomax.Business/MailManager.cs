////-----------------------------------------------------------------------
//// <copyright file="MailManager.cs" company="Gabriel Furlani">
////  (R) Registrado 2018 Gabriel Furlani.
////  Desenvolvido por Gabriel Furlani.
//// </copyright>
////-----------------------------------------------------------------------
namespace Neomax.Business
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using System.Collections.Generic;
    using System.Linq;
    using Neomax.Data.DataAccess;
    using Neomax.Data.Repository;
    using Neomax.Data.Util;
    using Neomax.Model.Dto;
    using Neomax.Model.Exception;
    using Neomax.Model.Util;

    /// <summary>
    /// Manager for e-mails sent by the application
    /// </summary>
    public class MailManager
    {
        /// <summary> Static logger variable </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Sends an e-mail of Welcome
        /// </summary>
        /// <param name="destinationEmail">User's destination e-mail</param>
        /// <param name="userName">User's name</param>
        public static void SendWelcomeEmail(string destinationEmail, string userName)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(ApplicationConfiguration.SmtpHostAddress);

                mail.From = new MailAddress(ApplicationConfiguration.SmtpSenderEmail, ApplicationConfiguration.SmtpSenderName);
                mail.To.Add(destinationEmail);
                mail.Subject = "Bem vindo a Neomax";

                ////Reads the HTML template and replace the tags
                mail.IsBodyHtml = true;
                var htmlBody = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.EmailTemplateDirectoryPath) + "\\WelcomeEmail.html");
                htmlBody = htmlBody.Replace("%LogoImageLink%", ApplicationConfiguration.LogoImageLink);
                htmlBody = htmlBody.Replace("%UserName%", userName);
                mail.Body = htmlBody;

                ////SMTP configurations
                smtpClient.Port = ApplicationConfiguration.SmtpHostPort;
                smtpClient.EnableSsl = ApplicationConfiguration.SmtpHostSsl;

                if (!string.IsNullOrEmpty(ApplicationConfiguration.SmtpUsername))
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(ApplicationConfiguration.SmtpUsername, ApplicationConfiguration.SmtpPassword);
                }

                ////Send the email
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                log.Fatal("SendWelcomeEmail: " + ex.ToString() + " // InnerException: " + ex.InnerException?.ToString());
                throw new BusinessException("Não foi possível enviar o e-mail de boas-vindas");
            }
        }

        /// <summary>
        /// Sends an e-mail of First Password creation
        /// </summary>
        /// <param name="destinationEmail">User's destination e-mail</param>
        /// <param name="userName">User's name</param>
        /// <param name="idUser">User identifier</param>
        /// <param name="changePasswordToken">Token used to identify requisition</param>
        public static void SendCreationUserEmail(string destinationEmail, string changePasswordToken)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(ApplicationConfiguration.SmtpHostAddress);

                mail.From = new MailAddress(ApplicationConfiguration.SmtpSenderEmail, ApplicationConfiguration.SmtpSenderName);
                mail.To.Add(destinationEmail);
                mail.Subject = "Cadastro Neomax";

                var finalLink = ApplicationConfiguration.NewUserBaseLink + "/" + changePasswordToken;

                mail.IsBodyHtml = true;
                var htmlBody = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.EmailTemplateDirectoryPath) + "\\CreationUserEmail.html");
                htmlBody = htmlBody.Replace("%LogoImageLink%", ApplicationConfiguration.LogoImageLink);
                htmlBody = htmlBody.Replace("%ExternalPasswordChangeLink%", finalLink);
                mail.Body = htmlBody;

                smtpClient.Port = ApplicationConfiguration.SmtpHostPort;
                smtpClient.EnableSsl = ApplicationConfiguration.SmtpHostSsl;
                smtpClient.Credentials = new System.Net.NetworkCredential(ApplicationConfiguration.SmtpUsername, ApplicationConfiguration.SmtpPassword);

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                log.Fatal("SendPasswordCreationEmail: " + ex.ToString() + " // InnerException: " + ex.InnerException?.ToString());
                throw new BusinessException("Não foi possível enviar o e-mail de criação de senha");
            }
        }

        /// <summary>
        /// Sends an e-mail of Forgot Password
        /// </summary>
        /// <param name="destinationEmail">User's destination e-mail</param>
        /// <param name="userName">User's name</param>
        /// <param name="idUser">User identifier</param>
        /// <param name="changePasswordToken">Token for Password Change</param>
        public static void SendForgotPasswordEmail(string destinationEmail, string userName, int idUser, string changePasswordToken)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(ApplicationConfiguration.SmtpHostAddress);

                mail.From = new MailAddress(ApplicationConfiguration.SmtpSenderEmail, ApplicationConfiguration.SmtpSenderName);
                mail.To.Add(destinationEmail);
                mail.Subject = "Redefinir Senha";

                var finalLink = ApplicationConfiguration.ExternalPasswordChangeBaseLink + "/" + idUser.ToString() + "/" + changePasswordToken;

                ////Reads the HTML template and replace the tags
                mail.IsBodyHtml = true;
                var htmlBody = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(ApplicationConfiguration.EmailTemplateDirectoryPath) + "\\ForgotPasswordEmail.html");
                htmlBody = htmlBody.Replace("%LogoImageLink%", ApplicationConfiguration.LogoImageLink);
                htmlBody = htmlBody.Replace("%UserName%", userName);
                htmlBody = htmlBody.Replace("%ExternalPasswordChangeLink%", finalLink);
                mail.Body = htmlBody;

                ////SMTP configurations
                smtpClient.Port = ApplicationConfiguration.SmtpHostPort;
                smtpClient.EnableSsl = ApplicationConfiguration.SmtpHostSsl;
                smtpClient.Credentials = new System.Net.NetworkCredential(ApplicationConfiguration.SmtpUsername, ApplicationConfiguration.SmtpPassword);

                ////Send the email
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                log.Fatal("SendForgotPasswordEmail: " + ex.ToString() + " // InnerException: " + ex.InnerException?.ToString());
                throw new BusinessException("Erro ao enviar o e-mail de Esqueci a Senha");
            }
        }
    }
}