using SendGrid.Helpers.Mail;
using SendGrid;
using ITShop.API.Interface;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;

namespace ITShop.API.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly string _apiKey;
        private readonly string template_id;
        private readonly string template_id_ts;

        public SendGridService(IConfiguration configuration)
        {
            _apiKey = configuration.GetSection("SendGridApiKey").Value;
            template_id = configuration.GetSection("SendGridTemplateId").Value;
            template_id_ts = configuration.GetSection("SendGridTemplateId_Troubleshoot").Value;
        }

        public async Task<bool> SendEnquiryEmail(
            string name,
            string email,
            string message,
            string subject)
        {
            try
            {
                SendGridClient client = new(_apiKey);

                SendGridMessage msg = new()
                {
                    TemplateId = template_id,
                    From = new EmailAddress("dzp.shop123@gmail.com", "Support"),
                    Subject = $"New message: {subject}",


                    Personalizations = new List<Personalization>
                {
                       
                    new ()
                    {
                        Tos = new List<EmailAddress>
                        {
                            EmailAddress(email),
                        },
                    }
                    
                },
                    

                };
                
                msg.SetTemplateData(new
                {
                    name= name,
                    email= email,
                    message= message,
                    subject=subject
                });
                


                Response response = await client.SendEmailAsync(msg);

                string? body = await response.Body.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendEmailTroubleshoot(
            string title, 
            string message, 
            string description)
        {
            try
            {
                SendGridClient client = new(_apiKey);

                SendGridMessage msg = new()
                {
                    TemplateId = template_id_ts,
                    From = new EmailAddress("dzp.shop123@gmail.com", "Support"),
                    Subject = $"New message: {title}",


                    Personalizations = new List<Personalization>
                {

                    new ()
                    {
                        Tos = new List<EmailAddress>
                        {
                            EmailAddress("dzp.shop123@gmail.com"),
                        },
                    }

                },


                };

                msg.SetTemplateData(new
                {
                    title=title,
                    message=message,
                    description=description
                });;



                Response response = await client.SendEmailAsync(msg);

                string? body = await response.Body.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private static EmailAddress EmailAddress(string email, string? name = null)
        =>
            string.IsNullOrEmpty(name)
                ? new EmailAddress("dzp.shop123@gmail.com")
                : new EmailAddress("dzp.shop123@gmail.com", "Support");
    }
}
