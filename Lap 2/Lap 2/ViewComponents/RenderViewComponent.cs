using Lap_2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lap_2.ViewComponents;

public class RenderViewComponent : ViewComponent
{
    private readonly List<MenuItem> _menuItems =
    [
        new() { Id = 1, Name = "Branches", Link = "/Branches/List" },
        new() { Id = 2, Name = "Students", Link = "/Student/Index" },
        new() { Id = 3, Name = "Subjects", Link = "/Subjects/List" },
        new() { Id = 4, Name = "Courses", Link = "/Courses/List" }
    ];

    public Task<IViewComponentResult> InvokeAsync()
    {
        return Task.FromResult<IViewComponentResult>(View("RenderLeftMenu", _menuItems));
    }
}
