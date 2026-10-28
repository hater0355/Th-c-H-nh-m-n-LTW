using DBFirstApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DBFirstApp.Controllers;

public class LearnersController(SchoolContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var learners = await context.Learners.AsNoTracking()
            .Include(learner => learner.Major)
            .OrderBy(learner => learner.LearnerId)
            .ToListAsync();
        return View(learners);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateMajorsAsync();
        return View(new Learner { EnrollmentDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstMidName,LastName,EnrollmentDate,MajorId")] Learner learner)
    {
        if (!await context.Majors.AnyAsync(major => major.MajorId == learner.MajorId))
        {
            ModelState.AddModelError(nameof(learner.MajorId), "Chọn một chuyên ngành hợp lệ.");
        }

        if (ModelState.IsValid)
        {
            context.Add(learner);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateMajorsAsync(learner.MajorId);
        return View(learner);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var learner = await context.Learners.FindAsync(id.Value);
        if (learner is null) return NotFound();
        await PopulateMajorsAsync(learner.MajorId);
        return View(learner);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LearnerId,FirstMidName,LastName,EnrollmentDate,MajorId")] Learner input)
    {
        if (id != input.LearnerId) return NotFound();

        var learner = await context.Learners.FindAsync(id);
        if (learner is null) return NotFound();
        if (!await context.Majors.AnyAsync(major => major.MajorId == input.MajorId))
        {
            ModelState.AddModelError(nameof(input.MajorId), "Chọn một chuyên ngành hợp lệ.");
        }

        if (ModelState.IsValid)
        {
            learner.FirstMidName = input.FirstMidName;
            learner.LastName = input.LastName;
            learner.EnrollmentDate = input.EnrollmentDate;
            learner.MajorId = input.MajorId;
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await PopulateMajorsAsync(input.MajorId);
        return View(input);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var learner = await context.Learners.AsNoTracking()
            .Include(item => item.Major)
            .Include(item => item.Enrollments)
            .FirstOrDefaultAsync(item => item.LearnerId == id.Value);
        if (learner is null) return NotFound();
        return View(learner);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var learner = await context.Learners.Include(item => item.Enrollments)
            .FirstOrDefaultAsync(item => item.LearnerId == id);
        if (learner is null) return NotFound();
        if (learner.Enrollments.Count > 0)
        {
            TempData["DeleteError"] = "Không thể xóa người học còn đăng ký khóa học.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        context.Learners.Remove(learner);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateMajorsAsync(int? selectedMajorId = null)
    {
        ViewData["MajorId"] = new SelectList(
            await context.Majors.AsNoTracking().OrderBy(major => major.MajorName).ToListAsync(),
            nameof(Major.MajorId), nameof(Major.MajorName), selectedMajorId);
    }
}

