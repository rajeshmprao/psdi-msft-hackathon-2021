namespace PSDIPortal.Email
{

    using PSDIPortal.Common;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PSDIPortal.Models;
    using System;
    using System.Configuration;
    using Microsoft.Extensions.Logging;
    using System.Web;
    using System.Net.Http;
    using Newtonsoft.Json.Linq;
    using System.IO;
    using System.Text;
    using Microsoft.AspNetCore.Hosting;

    public class EmailProcessor : IEmailProcessor
    {
        //private readonly string senderAddress = ConfigurationManager.AppSettings["From"];
        private readonly string senderAddress = "pafemail@microsoft.com";
        //private readonly string hostname = ConfigurationManager.AppSettings["hostname"];
        private readonly ILogger logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>Initializes a new instance of the <see cref="EmailProcessor" /> class.</summary>
        /// <param name="logger">The logger.</param>
        public EmailProcessor(ILogger<EmailProcessor> logger, IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this._webHostEnvironment = webHostEnvironment;
        }
        public async Task SendMetricAnamolyEmail(String user, String metricName, String metricValue)
        {
            try
            {
                SendEmail emailFormat = new SendEmail();                                        
                Notification emailNotification = new Notification();                            
                EmailNotification notificationDetail = new EmailNotification();
                List<string> toList = new List<string>();

                Dictionary<string, string> paramDict = new Dictionary<string, string>();        
                Dictionary<string, object> emailSendingInfo = new Dictionary<string, object>();

                string emailTemplate = "AnamolyEmailTemplate";
                string emailSubject = "Alert! Action required on " + metricName+ " metrics";
                string actorTeam = user;
                string message = "Anamoly has been found for the below Metric";
                string nextStep = "Please take the required action";

                paramDict.Add("actorTeam", actorTeam);
                paramDict.Add("message", message);
                paramDict.Add("nextStep", nextStep);

                // Add all the required parameter which needs to be chagned in template
                paramDict.Add("name",metricName );
                paramDict.Add("value", metricValue);
                //paramDict.Add("requestLink", this.GetRequestLink(reqmodel.id));
                toList.Add("dipalla@microsoft.com");

                emailSendingInfo.Add("SenderAddress", this.senderAddress);
                emailSendingInfo.Add("Content", emailTemplate);
                emailSendingInfo.Add("Subject", emailSubject);
                emailSendingInfo.Add("ReceiverAddresses", toList);

                await this.SendMail(emailTemplate, paramDict, emailSendingInfo);
            }

            catch
            {

            }
        }

        private async Task SendMail(string templateName, Dictionary<string, string> templateParams, Dictionary<string, object> emailParams)
        {
            string emailEndPoint = System.Configuration.ConfigurationManager.AppSettings["emailEndPoint"];
            var templatePath = templateName.EndsWith(".html") ? templateName : templateName + ".html";
            
            string absoluteTemplatePath;
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                absoluteTemplatePath = Path.Combine(webRootPath, templatePath);
               // absoluteTemplatePath = System.Web.Http.Current.Server.MapPath($"~\\Email\\EmailTemplates\\{templatePath}");
            }
            catch (Exception ex)
            {
                this.logger.LogWarning("Error in Server.MapPath", ex);

                // absoluteTemplatePath = HostingEnvironment.MapPath("~//email/emailTemplates/" + templatePath);
                var siteRoot = System.Configuration.ConfigurationManager.AppSettings["siteRoot"];
                absoluteTemplatePath = $"{siteRoot}\\Email\\EmailTemplates\\{templatePath}";
                this.logger.LogInformation($"path from site Root: {absoluteTemplatePath}");
            }

            var templateText = File.ReadAllText(absoluteTemplatePath);

            foreach (var templateParam in templateParams.Keys)
            {
                var templateParamKey = $"<<{templateParam}>>";
                templateText = templateText.Replace(templateParamKey, templateParams[templateParam]);
            }

            var attachmentEmail = new JObject();
            var payload = new JObject();
            if (emailParams.ContainsKey("EmailAttachment"))
            {
                string emailAttachmentInBase64 = emailParams["EmailAttachment"].ToString();
                attachmentEmail = new JObject
                {
                    ["Name"] = emailParams["EmailAttachmentName"].ToString(),
                    ["ContentBytes"] = emailParams["EmailAttachment"].ToString(),
                };

                var attachments = new JArray();
                attachments.Add(attachmentEmail);

                payload = new JObject
                {
                    ["Body"] = templateText,
                    ["To"] = string.Join(";", (IEnumerable<string>)emailParams["ReceiverAddresses"]),
                    ["CC"] = string.Join(";", (IEnumerable<string>)emailParams["AlternateReceiverAddreses"]),
                    ["BCC"] = string.Empty,
                    ["Subject"] = emailParams["Subject"].ToString(),
                    ["Attachments"] = attachments,
                };
            }
            else
            {
                payload = new JObject
                {
                    ["Body"] = templateText,
                    ["To"] = string.Join(";", (IEnumerable<string>)emailParams["ReceiverAddresses"]),
                    ["CC"] = string.Join(";", (IEnumerable<string>)emailParams["AlternateReceiverAddreses"]),
                    ["BCC"] = string.Empty,
                    ["Subject"] = emailParams["Subject"].ToString(),
                };
            }

            using (var client = new HttpClient())
            {
                var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(emailEndPoint, content);
                string responseContent = await response.Content.ReadAsStringAsync();
                var result = responseContent;
            }
        }
    }
}

