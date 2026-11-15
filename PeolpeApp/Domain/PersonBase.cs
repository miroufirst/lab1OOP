namespace PeopleApp.Domain;

public abstract class PersonBase : IPerson
{
    protected PersonBase(string firstName, string lastName)
    {
        FirstName = Require(firstName, nameof(firstName));
        LastName = Require(lastName, nameof(lastName));
    }

    public string FirstName { get; }
    public string LastName { get; }
    public abstract string Id { get; }

    protected static string Require(string? value, string name) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException($"{name} is required.", name)
            : value.Trim();
}