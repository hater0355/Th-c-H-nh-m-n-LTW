using Thực_Hành_Web.Models;

namespace Thực_Hành_Web.Services;

public class StudentStore
{
    private readonly object _sync = new();
    private readonly List<Student> _students =
    [
        new() { Id = 101, Name = "Hải Nam", Branch = Branch.IT, Gender = Gender.Male, IsRegular = true, Address = "A1-2018", Email = "nam@gmail.com", DateOfBirth = new DateTime(2004, 1, 1), Score = 8.5m },
        new() { Id = 102, Name = "Minh Tú", Branch = Branch.BE, Gender = Gender.Female, IsRegular = true, Address = "A1-2019", Email = "tu@gmail.com", DateOfBirth = new DateTime(2004, 5, 12), Score = 9.2m },
        new() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE, Gender = Gender.Male, IsRegular = false, Address = "A1-2020", Email = "phong@gmail.com", DateOfBirth = new DateTime(2003, 9, 20), Score = 7.8m },
        new() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE, Gender = Gender.Female, IsRegular = false, Address = "A1-2021", Email = "mai@gmail.com", DateOfBirth = new DateTime(2004, 3, 8), Score = 9.5m }
    ];

    public IReadOnlyList<Student> GetAll()
    {
        lock (_sync)
        {
            return _students.OrderBy(student => student.Id).ToArray();
        }
    }

    public void Add(Student student)
    {
        lock (_sync)
        {
            student.Id = _students.Count == 0 ? 101 : _students.Max(item => item.Id) + 1;
            _students.Add(student);
        }
    }
}
