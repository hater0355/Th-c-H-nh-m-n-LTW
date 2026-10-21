namespace Lap_1.Models;

public sealed class StudentStore
{
    private readonly object _gate = new();
    private readonly List<Student> _students =
    [
        new() { Id = 101, Name = "Hải Nam", Branch = Branch.IT, Gender = Gender.Male, IsRegular = true, Address = "A1-2018", Email = "nam@g.com", Password = "" },
        new() { Id = 102, Name = "Minh Tú", Branch = Branch.BE, Gender = Gender.Female, IsRegular = true, Address = "A1-2019", Email = "tu@g.com", Password = "" },
        new() { Id = 103, Name = "Hoàng Phong", Branch = Branch.CE, Gender = Gender.Male, IsRegular = false, Address = "A1-2020", Email = "phong@g.com", Password = "" },
        new() { Id = 104, Name = "Xuân Mai", Branch = Branch.EE, Gender = Gender.Female, IsRegular = false, Address = "A1-2021", Email = "mai@g.com", Password = "" }
    ];

    public IReadOnlyList<Student> GetAll()
    {
        lock (_gate)
        {
            return _students.Select(student => Copy(student)).ToList();
        }
    }

    public void Add(Student student)
    {
        lock (_gate)
        {
            student.Id = _students.Count == 0 ? 101 : _students.Max(existing => existing.Id) + 1;
            _students.Add(Copy(student));
        }
    }

    private static Student Copy(Student student) => new()
    {
        Id = student.Id,
        Name = student.Name,
        Email = student.Email,
        Password = student.Password,
        Branch = student.Branch,
        Gender = student.Gender,
        IsRegular = student.IsRegular,
        Address = student.Address,
        DateOfBirth = student.DateOfBirth,
        AvatarPath = student.AvatarPath
    };
}
