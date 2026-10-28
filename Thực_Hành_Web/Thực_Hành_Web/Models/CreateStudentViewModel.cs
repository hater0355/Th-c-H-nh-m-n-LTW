using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Thực_Hành_Web.Models;

public class CreateStudentViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Họ tên phải có từ 4 đến 100 ký tự.")]
    [Display(Name = "Họ và tên")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập email.")]
    [EmailAddress(ErrorMessage = "Email chưa đúng định dạng.")]
    [RegularExpression(@"^[A-Za-z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi gmail.com.")]
    [StringLength(254, ErrorMessage = "Email không được dài quá 254 ký tự.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có từ 8 đến 100 ký tự.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,100}$", ErrorMessage = "Mật khẩu cần có chữ hoa, chữ thường, chữ số và ký tự đặc biệt.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn ngành học.")]
    [Display(Name = "Ngành học")]
    public Branch? Branch { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
    [Display(Name = "Giới tính")]
    public Gender? Gender { get; set; }

    [Display(Name = "Hệ chính quy")]
    public bool IsRegular { get; set; }

    [Display(Name = "Địa chỉ")]
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Vui lòng chọn ngày sinh.")]
    [Range(typeof(DateTime), "1/1/1963", "12/31/2005", ErrorMessage = "Ngày sinh phải từ 01/01/1963 đến 31/12/2005.")]
    [Display(Name = "Ngày sinh")]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập điểm.")]
    [Range(typeof(decimal), "0.0", "10.0", ErrorMessage = "Điểm phải nằm trong khoảng từ 0 đến 10.")]
    [Display(Name = "Điểm")]
    public decimal? Score { get; set; }

    [Display(Name = "Ảnh đại diện")]
    public IFormFile? Avatar { get; set; }
}
