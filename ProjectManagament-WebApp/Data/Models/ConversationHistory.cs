namespace ProjectManagament_WebApp.Data.Models
{
    public class ConversationHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ModuleId { get; set; }
        public string Context { get; set; }
        public string Role { get; set; }
        public bool IsDeleted { get; set; }

        public User User { get; set; }
        public Module Module { get; set; }
    }
}
