namespace PeopleApp.Domain;
public interface IPerson
{
    string FirstName { get; }
    string LastName { get; }
    string Id { get; }
}

public interface ILearner { int StudySessions { get; } void Study(); }
public interface ITeacher { int LessonsGiven { get; } void Teach(); }
public interface IDriver { int TripsMade { get; } void Drive(); }
public interface IGuitarPlayer { int SongsPlayed { get; } void PlayGuitar(); }