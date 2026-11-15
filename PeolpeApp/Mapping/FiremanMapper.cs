using PeopleApp.Domain;

namespace PeopleApp.Mapping;

public sealed class FiremanMapper : EntityMapperBase<Fireman>
{
    public FiremanMapper() : base(new[]
    {
        new AttributeDefinition("serviceId", "Service ID (e.g. FR00123)", @"^[A-Z]{2}\d{5}$",
            "Format: two capital Latin letters and 5 digits."),
        new AttributeDefinition("station", "Fire station", @"^[\p{L}\d][\p{L}\d .\-]*$",
            "Letters, digits, spaces, '.' and '-' are allowed.",
            required: false, defaultValue: Fireman.DefaultStation),
    })
    { }

    public override string TypeName => "Fireman";

    protected override Fireman Build(string firstName, string lastName, string[] v) =>
        new(firstName, lastName, v[0], v[1]);

    protected override string[] ExtractSpecific(Fireman f) => new[] { f.ServiceId, f.Station };
}