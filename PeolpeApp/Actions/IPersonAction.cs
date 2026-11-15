using PeopleApp.Domain;

namespace PeopleApp.Actions;

public interface IPersonAction
{
    string Name { get; }
    bool AppliesTo(IPerson person);
    int Execute(IPerson person);
}

public sealed class StudyAction : IPersonAction
{
    public string Name => "Study()";
    public bool AppliesTo(IPerson person) => person is ILearner;
    public int Execute(IPerson person)
    {
        var learner = (ILearner)person;
        learner.Study();
        return learner.StudySessions;
    }
}

public sealed class TeachAction : IPersonAction
{
    public string Name => "Teach()";
    public bool AppliesTo(IPerson person) => person is ITeacher;
    public int Execute(IPerson person)
    {
        var teacher = (ITeacher)person;
        teacher.Teach();
        return teacher.LessonsGiven;
    }
}

public sealed class DriveAction : IPersonAction
{
    public string Name => "Drive()";
    public bool AppliesTo(IPerson person) => person is IDriver;
    public int Execute(IPerson person)
    {
        var driver = (IDriver)person;
        driver.Drive();
        return driver.TripsMade;
    }
}

public sealed class PlayGuitarAction : IPersonAction
{
    public string Name => "PlayGuitar()";
    public bool AppliesTo(IPerson person) => person is IGuitarPlayer;
    public int Execute(IPerson person)
    {
        var player = (IGuitarPlayer)person;
        player.PlayGuitar();
        return player.SongsPlayed;
    }
}