using Microsoft.AspNetCore.Mvc;
using Thực_Hành_Web.Models;

namespace Thực_Hành_Web.ViewComponents;

public class RenderViewComponent : ViewComponent
{
    private static readonly List<MenuItem> MenuItems =
    [
        new() { Id = 1, Name = "Branches", Link = "#branches" },
        new() { Id = 2, Name = "Students", Link = "/Admin/Student/List" },
        new() { Id = 3, Name = "Learners", Link = "/Learner" },
        new() { Id = 4, Name = "Subjects", Link = "#subjects" },
        new() { Id = 5, Name = "Courses", Link = "#courses" }
    ];

    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View("RenderLeftMenu", MenuItems));
    }
}
