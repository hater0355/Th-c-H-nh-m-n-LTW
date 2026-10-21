using Lap_2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lap_2.Controllers;

public class StudentController : Controller
{
    public IActionResult Index()
    {
        var students = new List<Student>
        {
            new() { Id = 101, FullName = "Hải Nam", DateOfBirth = DateTime.MinValue, Major = "CNTT", Gender = "Nam", StudyMode = "Chính quy", Address = "A1-2018", Email = "nam@g.com" },
            new() { Id = 102, FullName = "Minh Tú", DateOfBirth = DateTime.MinValue, Major = "Kinh tế", Gender = "Nữ", StudyMode = "Chính quy", Address = "A1-2019", Email = "tu@g.com" },
            new() { Id = 103, FullName = "Hoàng Phong", DateOfBirth = DateTime.MinValue, Major = "Công trình", Gender = "Nam", StudyMode = "Phi chính quy", Address = "A1-2020", Email = "phong@g.com" },
            new() { Id = 104, FullName = "Xuân Mai", DateOfBirth = DateTime.MinValue, Major = "Điện - Điện tử", Gender = "Nữ", StudyMode = "Phi chính quy", Address = "A1-2021", Email = "mai@g.com" }
        };
        return View(students);
    }
}
