using System.Text;
using FileStorage;
using PeopleApp.Actions;
using PeopleApp.Mapping;
using PeopleApp.Services;
using PeopleApp.UI;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var registry = new MapperRegistry(new IEntityMapper[]
{
    new StudentMapper(),
    new CourierMapper(),
    new FiremanMapper(),
});

IRecordStorage storage = new TextRecordStorage("people.db");
IPersonRepository repository = new PersonRepository(storage, registry);
IPersonService service = new PersonService(repository);

IPersonAction[] actions =
{
    new StudyAction(), new TeachAction(), new DriveAction(), new PlayGuitarAction(),
};

new ConsoleMenu(service, registry, actions).Run();