namespace PeopleApp.Domain;

public sealed class SelfStudySkill : ILearner
{
    public int StudySessions { get; private set; }
    public void Study() => StudySessions++;
}

public sealed class GuitarSkill : IGuitarPlayer
{
    public int SongsPlayed { get; private set; }
    public void PlayGuitar() => SongsPlayed++;
}