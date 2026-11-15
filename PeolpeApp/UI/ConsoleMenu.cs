using PeopleApp.Actions;
using PeopleApp.Demo;
using PeopleApp.Domain;
using PeopleApp.Mapping;
using PeopleApp.Services;

namespace PeopleApp.UI;

public sealed class ConsoleMenu
{
    private readonly IPersonService _service;
    private readonly MapperRegistry _registry;
    private readonly IPersonAction[] _actions;

    public ConsoleMenu(IPersonService service, MapperRegistry registry, IPersonAction[] actions)
    {
        _service = service;
        _registry = registry;
        _actions = actions;
    }

    public void Run()
    {
        bool exit = false;
        while (!exit)
        {
            PrintMenu();
            string choice = (Console.ReadLine() ?? "0").Trim();
            Console.WriteLine();
            try
            {
                switch (choice)
                {
                    case "1": AddPerson(); break;
                    case "2": ShowAll(); break;
                    case "3": DeleteById(); break;
                    case "4": FindByLastName(); break;
                    case "5": FindById(); break;
                    case "6": RunVariantTask(); break;
                    case "7": DemonstrateActions(); break;
                    case "8": LoadDemoData(); break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("Unknown command."); break;
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException or FormatException or IOException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine();
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("=== People database (variant 6) ===");
        Console.WriteLine("1. Add person");
        Console.WriteLine("2. Show all");
        Console.WriteLine("3. Delete by Id");
        Console.WriteLine("4. Find by last name");
        Console.WriteLine("5. Find by Id");
        Console.WriteLine("6. Task: 2nd-course students who do sports");
        Console.WriteLine("7. Demonstrate actions (Study/Teach/Drive/PlayGuitar)");
        Console.WriteLine("8. Load demo data");
        Console.WriteLine("0. Exit");
        Console.Write("> ");
    }

    private void AddPerson()
    {
        IEntityMapper[] mappers = _registry.Mappers;
        Console.WriteLine("Select type:");
        for (int i = 0; i < mappers.Length; i++)
            Console.WriteLine($"  {i + 1}. {mappers[i].TypeName}");
        Console.Write("> ");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > mappers.Length)
        {
            Console.WriteLine("Invalid choice.");
            return;
        }

        IEntityMapper mapper = mappers[choice - 1];
        string[] values = new string[mapper.Attributes.Length];
        for (int i = 0; i < values.Length; i++)
            values[i] = ReadValid(mapper.Attributes[i]);

        _service.Add(mapper.Create(values));
        Console.WriteLine("Saved.");
    }

    private static string ReadValid(AttributeDefinition attribute)
    {
        while (true)
        {
            string hint = attribute.Required ? "" : $" [optional, default '{attribute.DefaultValue}']";
            Console.Write($"{attribute.Prompt}{hint}: ");
            string raw = Console.ReadLine() ?? "";
            if (attribute.IsValid(raw))
                return attribute.Normalize(raw);
            Console.WriteLine($"  Invalid input: {attribute.ErrorMessage}");
        }
    }

    private void ShowAll()
    {
        IPerson[] all = _service.GetAll();
        Console.WriteLine($"Total records: {all.Length}");
        foreach (IPerson person in all)
            PrintPerson(person);
    }

    private void DeleteById()
    {
        Console.Write("Id to delete: ");
        string id = Console.ReadLine() ?? "";
        Console.WriteLine(_service.RemoveById(id) ? "Deleted." : "Nothing found.");
    }

    private void FindByLastName()
    {
        Console.Write("Last name: ");
        IPerson[] found = _service.FindByLastName(Console.ReadLine() ?? "");
        Console.WriteLine($"Found: {found.Length}");
        foreach (IPerson person in found)
            PrintPerson(person);
    }

    private void FindById()
    {
        Console.Write("Id: ");
        IPerson? person = _service.FindById(Console.ReadLine() ?? "");
        if (person is null)
            Console.WriteLine("Nothing found.");
        else
            PrintPerson(person);
    }

    private void RunVariantTask()
    {
        // Дані заново зчитуються з файлу.
        Student[] found = StudentQueries.SecondCourseSportsmen(_service.GetAll());
        Console.WriteLine($"2nd-course students who do sports: {found.Length}");
        foreach (Student student in found)
            PrintPerson(student);
    }

    private void DemonstrateActions()
    {
        Console.Write("Id of the person: ");
        IPerson? person = _service.FindById(Console.ReadLine() ?? "");
        if (person is null)
        {
            Console.WriteLine("Nothing found.");
            return;
        }

        bool any = false;
        foreach (IPersonAction action in _actions)
        {
            if (!action.AppliesTo(person)) continue;
            any = true;
            int total = action.Execute(person);
            Console.WriteLine($"{person.FirstName} {person.LastName}: {action.Name} performed (total: {total}).");
        }
        if (!any)
            Console.WriteLine("This person has no available actions.");
    }

    private void LoadDemoData()
    {
        int added = 0, skipped = 0;
        foreach (IPerson person in DemoData.Create())
        {
            try
            {
                _service.Add(person);
                added++;
            }
            catch (InvalidOperationException)
            {
                skipped++;
            }
        }
        Console.WriteLine($"Demo data: added {added}, skipped (already present) {skipped}.");
    }

    private void PrintPerson(IPerson person)
    {
        IEntityMapper mapper = _registry.FindFor(person);
        string[] values = mapper.Extract(person);
        Console.WriteLine($"[{mapper.TypeName}] Id = {person.Id}");
        for (int i = 0; i < values.Length; i++)
            Console.WriteLine($"    {mapper.Attributes[i].Key}: {values[i]}");
    }
}