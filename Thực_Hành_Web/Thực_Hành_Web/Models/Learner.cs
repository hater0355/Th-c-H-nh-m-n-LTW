using System.ComponentModel.DataAnnotations;

namespace Thực_Hành_Web.Models;

public class Learner
{
    public int LearnerID { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên.")]
    [StringLength(80, ErrorMessage = "Tên không được vượt quá 80 ký tự.")]
    [Display(Name = "Tên")]
    public string FirstMidName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập họ.")]
    [StringLength(80, ErrorMessage = "Họ không được vượt quá 80 ký tự.")]
    [Display(Name = "Họ")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Ngày nhập học")]
    public DateTime EnrollmentDate { get; set; } = DateTime.Today;

    [Display(Name = "Chuyên ngành")]
    public int MajorID { get; set; }

    [Display(Name = "Chuyên ngành")]
    public Major? Major { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
}
