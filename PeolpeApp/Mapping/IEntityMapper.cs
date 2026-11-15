using PeopleApp.Domain;

namespace PeopleApp.Mapping;

public interface IEntityMapper
{
    string TypeName { get; }
    AttributeDefinition[] Attributes { get; }
    bool CanHandle(IPerson person);
    IPerson Create(string[] values);
    string[] Extract(IPerson person);
}