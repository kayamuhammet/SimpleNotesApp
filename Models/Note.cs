using System.ComponentModel.DataAnnotations;

namespace SimpleNotesApp.Models
{
    public class Note
    {
        public int Id {get; set;}

        [Required]
        [MaxLength(100)]
        public string? Title {get; set;}

        [Required]
        public string? Context {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
    }
}