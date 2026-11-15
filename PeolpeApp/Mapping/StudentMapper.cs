using PeopleApp.Domain;

namespace PeopleApp.Mapping;

public sealed class StudentMapper : EntityMapperBase<Student>
{
    public StudentMapper() : base(new[]
    {
        new AttributeDefinition("course", "Course (1-6)", @"^[1-6]$",
            "Course must be a number from 1 to 6."),
        new AttributeDefinition("studentId", "Student ticket (e.g. KB123456)", @"^[A-Z]{2}\d{6}$",
            "Format: two capital Latin letters and 6 digits."),
        new AttributeDefinition("taxId", "Tax ID (10 digits)", @"^\d{10}$",
            "Tax ID must contain exactly 10 digits."),
        new AttributeDefinition("sport", "Sport hobby (empty if none)", @"^\p{L}+(?:[-' ]\p{L}+)*$",
            "Only letters, spaces and '-' are allowed.", required: false, defaultValue: ""),
    })
    { }

    public override string TypeName => "Student";

    protected override Student Build(string firstName, string lastName, string[] v) =>
        new(firstName, lastName, int.Parse(v[0]), v[1], v[2], v[3]);

    protected override string[] ExtractSpecific(Student s) =>
        new[] { s.Course.ToString(), s.StudentId, s.TaxId, s.Sport };
}