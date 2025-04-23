namespace SimpleNotesApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Navigation
        public ICollection<Note>? Notes { get; set; }
    }
}
