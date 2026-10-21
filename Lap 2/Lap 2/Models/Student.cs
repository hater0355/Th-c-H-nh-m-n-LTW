namespace Lap_2.Models;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Major { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string StudyMode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
