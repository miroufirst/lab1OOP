namespace PeopleApp.Mapping;
public static class CommonAttributes
{
    private const string NamePattern = @"^\p{L}+(?:[-'’ ]\p{L}+)*$";

    public static readonly AttributeDefinition FirstName = new(
        "firstname", "First name", NamePattern, "Only letters, spaces, '-' and apostrophe are allowed.");

    public static readonly AttributeDefinition LastName = new(
        "lastname", "Last name", NamePattern, "Only letters, spaces, '-' and apostrophe are allowed.");
}