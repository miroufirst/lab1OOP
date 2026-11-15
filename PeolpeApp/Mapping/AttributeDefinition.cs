using System.Text.RegularExpressions;

namespace PeopleApp.Mapping;

public sealed class AttributeDefinition
{
    private readonly Regex _regex;

    public AttributeDefinition(string key, string prompt, string pattern, string errorMessage,
                               bool required = true, string defaultValue = "")
    {
        Key = key;
        Prompt = prompt;
        ErrorMessage = errorMessage;
        Required = required;
        DefaultValue = defaultValue;
        _regex = new Regex(pattern, RegexOptions.CultureInvariant);
    }

    public string Key { get; }
    public string Prompt { get; }
    public string ErrorMessage { get; }
    public bool Required { get; }
    public string DefaultValue { get; }

    public string Normalize(string? raw)
    {
        string value = raw?.Trim() ?? "";
        return value.Length == 0 && !Required ? DefaultValue : value;
    }

    public bool IsValid(string? raw)
    {
        string value = Normalize(raw);
        if (value.Length == 0)
            return !Required;
        return _regex.IsMatch(value);
    }
}