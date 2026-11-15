namespace PeopleApp.Domain;

public sealed class Fireman : PersonBase, ILearner, ITeacher, IGuitarPlayer
{
    public const string DefaultStation = "Unassigned";

    private readonly SelfStudySkill _selfStudy = new();
    private readonly GuitarSkill _guitar = new();

    public Fireman(string firstName, string lastName, string serviceId, string? station = null)
        : base(firstName, lastName)
    {
        ServiceId = Require(serviceId, nameof(serviceId));
        Station = string.IsNullOrWhiteSpace(station) ? DefaultStation : station.Trim();
    }

    public string ServiceId { get; }
    public string Station { get; }
    public override string Id => ServiceId;

    public int StudySessions => _selfStudy.StudySessions;
    public void Study() => _selfStudy.Study();
    public int LessonsGiven { get; private set; }
    public void Teach() => LessonsGiven++;

    public int SongsPlayed => _guitar.SongsPlayed;
    public void PlayGuitar() => _guitar.PlayGuitar();
}