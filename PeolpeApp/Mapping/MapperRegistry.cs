using PeopleApp.Domain;

namespace PeopleApp.Mapping;

public sealed class MapperRegistry
{
    public MapperRegistry(IEntityMapper[] mappers)
    {
        Mappers = mappers;
    }

    public IEntityMapper[] Mappers { get; }

    public IEntityMapper FindByType(string typeName)
    {
        foreach (IEntityMapper mapper in Mappers)
        {
            if (string.Equals(mapper.TypeName, typeName, StringComparison.OrdinalIgnoreCase))
                return mapper;
        }
        throw new FormatException($"Unknown data type '{typeName}'.");
    }

    public IEntityMapper FindFor(IPerson person)
    {
        foreach (IEntityMapper mapper in Mappers)
        {
            if (mapper.CanHandle(person))
                return mapper;
        }
        throw new InvalidOperationException($"No mapper registered for {person.GetType().Name}.");
    }
}