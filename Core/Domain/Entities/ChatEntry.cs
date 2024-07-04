namespace HealthChatBox.Core.Domain.Entities
{
    public class ChatEntry : Auditables
    {
        public int Id { get; set; }
        public string Query { get; set; }
        public string Response { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

