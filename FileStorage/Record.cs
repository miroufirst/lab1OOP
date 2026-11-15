namespace FileStorage;

public sealed class Record
{
    public Record(string typeName, string objectName, string[] keys, string[] values)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            throw new ArgumentException("Type name is required.", nameof(typeName));
        if (string.IsNullOrWhiteSpace(objectName))
            throw new ArgumentException("Object name is required.", nameof(objectName));
        if (keys is null || values is null || keys.Length != values.Length || keys.Length == 0)
            throw new ArgumentException("Keys and values must be non-empty arrays of equal length.");

        TypeName = typeName;
        ObjectName = objectName;
        Keys = keys;
        Values = values;
    }

    public string TypeName { get; }
    public string ObjectName { get; }
    public string[] Keys { get; }
    public string[] Values { get; }

    public string? GetValue(string key)
    {
        for (int i = 0; i < Keys.Length; i++)
        {
            if (string.Equals(Keys[i], key, StringComparison.OrdinalIgnoreCase))
                return Values[i];
        }
        return null;
    }
}