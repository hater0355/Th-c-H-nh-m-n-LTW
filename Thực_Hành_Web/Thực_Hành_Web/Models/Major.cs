using System.ComponentModel.DataAnnotations;

namespace Thực_Hành_Web.Models;

public class Major
{
    public int MajorID { get; set; }

    [Required]
    [StringLength(80)]
    public string MajorName { get; set; } = string.Empty;

    public ICollection<Learner> Learners { get; set; } = new HashSet<Learner>();
}
