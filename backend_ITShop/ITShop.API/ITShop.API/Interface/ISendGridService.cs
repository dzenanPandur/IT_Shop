namespace ITShop.API.Interface
{
    public interface ISendGridService 
    {
        public Task<bool> SendEnquiryEmail(string name, string email, string message, string subject);
        public Task<bool> SendEmailTroubleshoot(string title, string message, string description);
    }
}
