using PeopleApp.Domain;

namespace PeopleApp.Services;

public interface IPersonRepository
{
    IPerson[] GetAll();
    void Add(IPerson person);
    void SaveAll(IPerson[] people);
}