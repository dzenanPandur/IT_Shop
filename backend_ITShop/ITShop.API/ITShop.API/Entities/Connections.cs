namespace ITShop.API.Entities
{
   
        public partial class Connections
        {
            public Guid Id { get; set; }
            public Guid PersonId { get; set; }
            public string SignalrId { get; set; }
            public DateTime TimeStamp { get; set; }
        }
    
}
