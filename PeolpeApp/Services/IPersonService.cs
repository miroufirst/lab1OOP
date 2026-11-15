using PeopleApp.Domain;

namespace PeopleApp.Services;

public interface IPersonService
{
    IPerson[] GetAll();
    void Add(IPerson person);
    bool RemoveById(string id);
    IPerson[] FindByLastName(string lastName);
    IPerson? FindById(string id);
}