using Lap_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lap_1.Controllers;

public class StudentController(StudentStore store, IWebHostEnvironment environment) : Controller
{
    private const long MaxAvatarBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    [HttpGet("/Admin/Student/List", Name = "StudentList")]
    [HttpGet("/Student/Index")]
    public IActionResult Index() => View("Index", store.GetAll());

    [HttpGet("/Admin/Student/Add", Name = "StudentAdd")]
    [HttpGet("/Student/Create")]
    public IActionResult Create() => View(new StudentCreateViewModel());

    [HttpPost("/Admin/Student/Add")]
    [HttpPost("/Student/Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentCreateViewModel input, CancellationToken cancellationToken)
    {
        if (input.Avatar is { Length: > MaxAvatarBytes })
        {
            ModelState.AddModelError(nameof(input.Avatar), "Ảnh đại diện không được vượt quá 5 MB.");
        }
        else if (input.Avatar is { Length: > 0 } avatar && !IsAllowedImage(avatar.FileName, avatar.ContentType, await ReadHeaderAsync(avatar, cancellationToken)))
        {
            ModelState.AddModelError(nameof(input.Avatar), "Chỉ chấp nhận ảnh JPG, PNG hoặc WebP hợp lệ.");
        }

        if (!ModelState.IsValid)
        {
            return View(input);
        }

        if (input.Avatar is { Length: > 0 } image)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{extension}";
            var uploadDirectory = Path.Combine(environment.WebRootPath, "uploads", "students");
            Directory.CreateDirectory(uploadDirectory);
            var filePath = Path.Combine(uploadDirectory, fileName);
            await using var output = System.IO.File.Create(filePath);
            await image.CopyToAsync(output, cancellationToken);
            input.Student.AvatarPath = $"/uploads/students/{fileName}";
        }

        // This demo has no authentication system, so never retain the submitted password.
        input.Student.Password = string.Empty;
        store.Add(input.Student);
        return RedirectToRoute("StudentList");
    }

    private static async Task<byte[]> ReadHeaderAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var header = new byte[12];
        await using var stream = file.OpenReadStream();
        var count = await stream.ReadAsync(header.AsMemory(), cancellationToken);
        return header[..count];
    }

    private static bool IsAllowedImage(string fileName, string contentType, byte[] header)
    {
        var extension = Path.GetExtension(fileName);
        if (!AllowedExtensions.Contains(extension))
        {
            return false;
        }

        var matchesSignature = extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => header.Length >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
            ".png" => header.Length >= 8 && header.AsSpan(0, 8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }),
            ".webp" => header.Length >= 12 && header.AsSpan(0, 4).SequenceEqual("RIFF"u8) && header.AsSpan(8, 4).SequenceEqual("WEBP"u8),
            _ => false
        };

        return matchesSignature && contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
}
