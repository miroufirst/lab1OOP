using PeopleApp.Domain;

namespace PeopleApp.Mapping;

public abstract class EntityMapperBase<T> : IEntityMapper where T : class, IPerson
{
    private const int CommonCount = 2;

    protected EntityMapperBase(AttributeDefinition[] specific)
    {
        Attributes = ArrayUtil.Concat(
            new[] { CommonAttributes.FirstName, CommonAttributes.LastName }, specific);
    }

    public abstract string TypeName { get; }
    public AttributeDefinition[] Attributes { get; }

    public bool CanHandle(IPerson person) => person is T;

    public IPerson Create(string[] values)
    {
        string[] specific = new string[values.Length - CommonCount];
        Array.Copy(values, CommonCount, specific, 0, specific.Length);
        return Build(values[0], values[1], specific);
    }

    public string[] Extract(IPerson person)
    {
        T typed = (T)person;
        return ArrayUtil.Concat(new[] { typed.FirstName, typed.LastName }, ExtractSpecific(typed));
    }

    protected abstract T Build(string firstName, string lastName, string[] specific);
    protected abstract string[] ExtractSpecific(T person);
}