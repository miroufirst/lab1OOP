namespace PeopleApp.Domain;

public sealed class Student : PersonBase, ILearner, IGuitarPlayer
{
    private readonly GuitarSkill _guitar = new();

    public Student(string firstName, string lastName, int course, string studentId, string taxId, string sport = "")
        : base(firstName, lastName)
    {
        if (course < 1 || course > 6)
            throw new ArgumentOutOfRangeException(nameof(course), "Course must be 1..6.");

        Course = course;
        StudentId = Require(studentId, nameof(studentId));
        TaxId = Require(taxId, nameof(taxId));
        Sport = sport?.Trim() ?? "";
    }

    public int Course { get; }
    public string StudentId { get; }
    public string TaxId { get; }
    public string Sport { get; }
    public bool DoesSport => Sport.Length > 0;

    public override string Id => StudentId;

    public int StudySessions { get; private set; }
    public void Study() => StudySessions++;

    public int SongsPlayed => _guitar.SongsPlayed;
    public void PlayGuitar() => _guitar.PlayGuitar();
}