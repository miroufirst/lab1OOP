namespace FileStorage;
public interface IRecordStorage
{
    Record[] ReadAll();
    void WriteAll(Record[] records);
    void Append(Record record);
}