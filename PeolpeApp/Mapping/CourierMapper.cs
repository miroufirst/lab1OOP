using PeopleApp.Domain;

namespace PeopleApp.Mapping;

public sealed class CourierMapper : EntityMapperBase<Courier>
{
    public CourierMapper() : base(new[]
    {
        new AttributeDefinition("driverLicense", "Driver license (e.g. AEK123456)", @"^[A-Z]{3}\d{6}$",
            "Format: three capital Latin letters and 6 digits."),
    })
    { }

    public override string TypeName => "Courier";

    protected override Courier Build(string firstName, string lastName, string[] v) =>
        new(firstName, lastName, v[0]);

    protected override string[] ExtractSpecific(Courier c) => new[] { c.DriverLicense };
}