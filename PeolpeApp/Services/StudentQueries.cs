using PeopleApp.Domain;

namespace PeopleApp.Services;

public static class StudentQueries
{
    public static Student[] SecondCourseSportsmen(IPerson[] people)
    {
        int count = 0;
        foreach (IPerson p in people)
        {
            if (IsMatch(p)) count++;
        }

        Student[] result = new Student[count];
        int index = 0;
        foreach (IPerson p in people)
        {
            if (p is Student s && IsMatch(s)) result[index++] = s;
        }
        return result;
    }

    private static bool IsMatch(IPerson p) => p is Student { Course: 2, DoesSport: true };
}