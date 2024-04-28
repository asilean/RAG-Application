namespace ProjectManagament_WebApp.Data.Models
{
    public class Module
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public List<ConversationHistory> ConversationHistories { get; set; }
    }
}
