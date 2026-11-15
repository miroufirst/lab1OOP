using System.Text;
using FileStorage;
using PeopleApp.Domain;
using PeopleApp.Mapping;

namespace PeopleApp.Services;

/// <summary>
/// Перетворює записи сховища на сутності та навпаки. Залежить від абстракцій
/// (IRecordStorage, MapperRegistry), а не від конкретного файлу чи типу сутності.
/// </summary>
public sealed class PersonRepository : IPersonRepository
{
    private readonly IRecordStorage _storage;
    private readonly MapperRegistry _registry;

    public PersonRepository(IRecordStorage storage, MapperRegistry registry)
    {
        _storage = storage;
        _registry = registry;
    }

    public IPerson[] GetAll()
    {
        Record[] records = _storage.ReadAll();
        IPerson[] people = new IPerson[records.Length];
        for (int i = 0; i < records.Length; i++)
            people[i] = ToPerson(records[i]);
        return people;
    }

    public void Add(IPerson person) => _storage.Append(ToRecord(person));

    public void SaveAll(IPerson[] people)
    {
        Record[] records = new Record[people.Length];
        for (int i = 0; i < people.Length; i++)
            records[i] = ToRecord(people[i]);
        _storage.WriteAll(records);
    }

    private IPerson ToPerson(Record record)
    {
        IEntityMapper mapper = _registry.FindByType(record.TypeName);
        string[] values = new string[mapper.Attributes.Length];

        for (int i = 0; i < values.Length; i++)
        {
            AttributeDefinition attribute = mapper.Attributes[i];
            string value = attribute.Normalize(record.GetValue(attribute.Key));
            if (!attribute.IsValid(value))
                throw new FormatException($"Record '{record.ObjectName}', '{attribute.Key}': {attribute.ErrorMessage}");
            values[i] = value;
        }
        return mapper.Create(values);
    }

    private Record ToRecord(IPerson person)
    {
        IEntityMapper mapper = _registry.FindFor(person);
        string[] values = mapper.Extract(person);
        string[] keys = new string[values.Length];
        for (int i = 0; i < keys.Length; i++)
            keys[i] = mapper.Attributes[i].Key;

        return new Record(mapper.TypeName, BuildObjectName(person), keys, values);
    }

    private static string BuildObjectName(IPerson person)
    {
        var sb = new StringBuilder();
        foreach (char c in person.FirstName + person.LastName)
        {
            if (char.IsLetterOrDigit(c))
                sb.Append(c);
        }
        return sb.ToString();
    }
}