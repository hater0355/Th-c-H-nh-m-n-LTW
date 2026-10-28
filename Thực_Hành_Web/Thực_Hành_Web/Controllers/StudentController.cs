using Microsoft.AspNetCore.Mvc;
using Thực_Hành_Web.Models;
using Thực_Hành_Web.Services;

namespace Thực_Hành_Web.Controllers;

[Route("Admin/Student")]
public class StudentController(StudentStore studentStore, IWebHostEnvironment environment) : Controller
{
    private const long MaxAvatarSize = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedAvatarExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

    [HttpGet("List", Name = "StudentList")]
    public IActionResult Index()
    {
        return View(studentStore.GetAll());
    }

    [HttpGet("Add", Name = "StudentAdd")]
    public IActionResult Create()
    {
        return View(new CreateStudentViewModel());
    }

    [HttpPost("Add", Name = "StudentAddPost")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStudentViewModel input)
    {
        if (input.Avatar is { Length: > 0 } avatar)
        {
            var extension = Path.GetExtension(avatar.FileName);
            if (!AllowedAvatarExtensions.Contains(extension))
            {
                ModelState.AddModelError(nameof(input.Avatar), "Ảnh phải có định dạng JPG, PNG, GIF hoặc WEBP.");
            }
            else if (avatar.Length > MaxAvatarSize)
            {
                ModelState.AddModelError(nameof(input.Avatar), "Dung lượng ảnh tối đa là 5 MB.");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(input);
        }

        string? avatarPath = null;
        if (input.Avatar is { Length: > 0 } validAvatar)
        {
            var extension = Path.GetExtension(validAvatar.FileName).ToLowerInvariant();
            var uploadDirectory = Path.Combine(environment.WebRootPath, "uploads", "students");
            Directory.CreateDirectory(uploadDirectory);
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadDirectory, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await validAvatar.CopyToAsync(stream);
            avatarPath = $"/uploads/students/{fileName}";
        }

        studentStore.Add(new Student
        {
            Name = input.Name.Trim(),
            Email = input.Email.Trim(),
            Password = input.Password,
            Branch = input.Branch,
            Gender = input.Gender,
            IsRegular = input.IsRegular,
            Address = input.Address?.Trim(),
            DateOfBirth = input.DateOfBirth!.Value,
            Score = input.Score,
            AvatarPath = avatarPath
        });

        return RedirectToRoute("StudentList");
    }
}
