using PeopleApp.Domain;

namespace PeopleApp.Demo;
public static class DemoData
{
    public static IPerson[] Create() => new IPerson[]
    {
        new Student("Vasyl",    "Pupkin",      2, "KB123456", "3012345678", "football"),
        new Student("Olena",    "Shevchenko",  2, "KB123457", "3012345679", "swimming"),
        new Student("Andrii",   "Koval",       2, "KB123458", "3012345680"),
        new Student("Maria",    "Bondar",      3, "KB123459", "3012345681", "tennis"),
        new Student("Ihor",     "Melnyk",      2, "KB123460", "3012345682", "basketball"),
        new Student("Kateryna", "Tkachenko",   1, "KB123461", "3012345683"),
        new Courier("Petro",    "Ivanchenko",  "AEK123456"),
        new Courier("Oksana",   "Lysenko",     "BXH654321"),
        new Fireman("Taras",    "Hrytsenko",   "FR00123", "Station 5"),
        new Fireman("Yurii",    "Savchenko",   "FR00456"),
    };
}