using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Thực_Hành_Web.Data;
using Thực_Hành_Web.Models;

namespace Thực_Hành_Web.Controllers;

public class LearnerController(SchoolContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var learners = await db.Learners
            .AsNoTracking()
            .Include(learner => learner.Major)
            .OrderBy(learner => learner.LearnerID)
            .ToListAsync();
        return View(learners);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateMajorsAsync();
        return View(new Learner { EnrollmentDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
    {
        if (!await db.Majors.AnyAsync(major => major.MajorID == learner.MajorID))
        {
            ModelState.AddModelError(nameof(learner.MajorID), "Vui lòng chọn chuyên ngành hợp lệ.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateMajorsAsync(learner.MajorID);
            return View(learner);
        }

        db.Learners.Add(learner);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var learner = await db.Learners.FindAsync(id.Value);
        if (learner is null)
        {
            return NotFound();
        }

        await PopulateMajorsAsync(learner.MajorID);
        return View(learner);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner input)
    {
        if (id != input.LearnerID)
        {
            return NotFound();
        }

        var learner = await db.Learners.FindAsync(id);
        if (learner is null)
        {
            return NotFound();
        }

        if (!await db.Majors.AnyAsync(major => major.MajorID == input.MajorID))
        {
            ModelState.AddModelError(nameof(input.MajorID), "Vui lòng chọn chuyên ngành hợp lệ.");
        }

        if (!ModelState.IsValid)
        {
            await PopulateMajorsAsync(input.MajorID);
            return View(input);
        }

        learner.FirstMidName = input.FirstMidName;
        learner.LastName = input.LastName;
        learner.MajorID = input.MajorID;
        learner.EnrollmentDate = input.EnrollmentDate;
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var learner = await db.Learners
            .AsNoTracking()
            .Include(item => item.Major)
            .Include(item => item.Enrollments)
            .FirstOrDefaultAsync(item => item.LearnerID == id.Value);

        if (learner is null)
        {
            return NotFound();
        }

        ViewBag.HasEnrollments = learner.Enrollments.Count > 0;
        return View(learner);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var learner = await db.Learners
            .Include(item => item.Enrollments)
            .FirstOrDefaultAsync(item => item.LearnerID == id);

        if (learner is null)
        {
            return NotFound();
        }

        if (learner.Enrollments.Count > 0)
        {
            TempData["LearnerDeleteError"] = "Không thể xóa người học vì vẫn còn đăng ký khóa học.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        db.Learners.Remove(learner);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateMajorsAsync(int? selectedMajorId = null)
    {
        ViewBag.Majors = new SelectList(
            await db.Majors.AsNoTracking().OrderBy(major => major.MajorName).ToListAsync(),
            nameof(Major.MajorID),
            nameof(Major.MajorName),
            selectedMajorId);
    }
}
