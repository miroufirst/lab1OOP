using PeopleApp.Domain;

namespace PeopleApp.Services;
public sealed class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public IPerson[] GetAll() => _repository.GetAll();

    public void Add(IPerson person)
    {
        if (FindById(person.Id) is not null)
            throw new InvalidOperationException($"A person with Id '{person.Id}' already exists.");
        _repository.Add(person);
    }

    public bool RemoveById(string id)
    {
        IPerson[] all = _repository.GetAll();
        int kept = 0;
        foreach (IPerson p in all)
        {
            if (!SameId(p, id)) kept++;
        }
        if (kept == all.Length)
            return false;

        IPerson[] rest = new IPerson[kept];
        int index = 0;
        foreach (IPerson p in all)
        {
            if (!SameId(p, id)) rest[index++] = p;
        }
        _repository.SaveAll(rest);
        return true;
    }

    public IPerson[] FindByLastName(string lastName)
    {
        IPerson[] all = _repository.GetAll();
        int count = 0;
        foreach (IPerson p in all)
        {
            if (SameLastName(p, lastName)) count++;
        }

        IPerson[] found = new IPerson[count];
        int index = 0;
        foreach (IPerson p in all)
        {
            if (SameLastName(p, lastName)) found[index++] = p;
        }
        return found;
    }

    public IPerson? FindById(string id)
    {
        foreach (IPerson p in _repository.GetAll())
        {
            if (SameId(p, id)) return p;
        }
        return null;
    }

    private static bool SameId(IPerson p, string id) =>
        string.Equals(p.Id, id.Trim(), StringComparison.OrdinalIgnoreCase);

    private static bool SameLastName(IPerson p, string lastName) =>
        string.Equals(p.LastName, lastName.Trim(), StringComparison.OrdinalIgnoreCase);
}