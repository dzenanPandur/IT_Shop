using ITShop.API.Enums;

namespace ITShop.API
{
    public class Message
    {
        public virtual ExceptionCode Status { get; set; }
        public virtual string Info { get; set; }
        public virtual object Data { get; set; }
        public virtual bool IsValid { get; set; }

        public Message()
        {
        }

        public Message(bool isValid, string info, ExceptionCode status, object data = null)
        {
            IsValid = isValid;
            Status = status;
            Info = info;
            Data = data;
        }
    }

}
