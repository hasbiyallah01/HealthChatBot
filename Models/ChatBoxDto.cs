namespace HealthChatBox.Models
{
    public class ChatEntryDto
    {
        public string Query { get; set; }
        public string Response { get; set; }
    }

    public class ChatResponseDto
    {
        public int Id { get; set; }
        public string Query { get; set; }
        public string Response { get; set; }
    }
}

