namespace PeopleApp.Domain;

public sealed class Courier : PersonBase, ILearner, IDriver, IGuitarPlayer
{
    private readonly SelfStudySkill _selfStudy = new();
    private readonly GuitarSkill _guitar = new();

    public Courier(string firstName, string lastName, string driverLicense)
        : base(firstName, lastName)
    {
        DriverLicense = Require(driverLicense, nameof(driverLicense));
    }

    public string DriverLicense { get; }
    public override string Id => DriverLicense;

    public int StudySessions => _selfStudy.StudySessions;
    public void Study() => _selfStudy.Study();

    public int TripsMade { get; private set; }
    public void Drive() => TripsMade++;

    public int SongsPlayed => _guitar.SongsPlayed;
    public void PlayGuitar() => _guitar.PlayGuitar();
}