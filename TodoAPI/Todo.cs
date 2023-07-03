using System.ComponentModel.DataAnnotations;

namespace TodoAPI;
public class Todo
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    public string Description { get; set; } = "";
}