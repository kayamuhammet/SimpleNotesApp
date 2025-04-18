namespace SimpleNotesApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        // Navigation
        public ICollection<Note>? Notes { get; set; }
    }
}
