using System.ComponentModel.DataAnnotations;

namespace Lap_1.Models;

public class StudentCreateViewModel
{
    public Student Student { get; set; } = new();

    [Display(Name = "Ảnh đại diện")]
    public IFormFile? Avatar { get; set; }
}
