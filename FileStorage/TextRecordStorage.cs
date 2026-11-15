using System.Text;

namespace FileStorage;

public sealed class TextRecordStorage : IRecordStorage
{
    private readonly string _path;

    public TextRecordStorage(string path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public Record[] ReadAll()
    {
        Record[] result = new Record[0];
        if (!File.Exists(_path))
            return result;

        using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        Record? record;
        while ((record = ReadRecord(reader)) is not null)
        {
            Array.Resize(ref result, result.Length + 1);
            result[^1] = record;
        }
        return result;
    }

    public void WriteAll(Record[] records)
    {
        using var stream = new FileStream(_path, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        for (int i = 0; i < records.Length; i++)
            WriteRecord(writer, records[i]);
    }

    public void Append(Record record)
    {
        using var stream = new FileStream(_path, FileMode.Append, FileAccess.Write);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false));
        WriteRecord(writer, record);
    }


    private static void WriteRecord(TextWriter writer, Record record)
    {
        writer.WriteLine($"{record.TypeName} {record.ObjectName}");
        for (int i = 0; i < record.Keys.Length; i++)
        {
            string prefix = i == 0 ? "{ " : "  ";
            string suffix = i == record.Keys.Length - 1 ? "};" : ",";
            writer.WriteLine($"{prefix}\"{Escape(record.Keys[i])}\": \"{Escape(record.Values[i])}\"{suffix}");
        }
    }

    private static string Escape(string s) => s.Replace("\\", "\\\\").Replace("\"", "\\\"");


    private static Record? ReadRecord(StreamReader reader)
    {
        string? header;
        do
        {
            header = reader.ReadLine();
        } while (header is not null && header.Trim().Length == 0);

        if (header is null)
            return null;

        header = header.Trim();
        int space = header.IndexOf(' ');
        if (space <= 0)
            throw new FormatException($"Invalid record header: '{header}'.");

        string typeName = header[..space];
        string objectName = header[(space + 1)..].Trim();

        string[] keys = new string[0];
        string[] values = new string[0];
        bool finished = false;

        while (!finished)
        {
            string? line = reader.ReadLine();
            if (line is null)
                throw new FormatException($"Record '{objectName}' is not terminated with '}};'.");

            line = line.Trim();
            if (line.StartsWith('{'))
                line = line[1..];

            if (line.EndsWith("};"))
            {
                line = line[..^2];
                finished = true;
            }
            else if (line.EndsWith(','))
            {
                line = line[..^1];
            }

            line = line.Trim();
            if (line.Length == 0)
                continue;

            ParsePair(line, out string key, out string value);
            Array.Resize(ref keys, keys.Length + 1);
            Array.Resize(ref values, values.Length + 1);
            keys[^1] = key;
            values[^1] = value;
        }

        return new Record(typeName, objectName, keys, values);
    }

    private static void ParsePair(string line, out string key, out string value)
    {
        int pos = 0;
        key = ReadQuoted(line, ref pos);

        while (pos < line.Length && char.IsWhiteSpace(line[pos])) pos++;
        if (pos >= line.Length || line[pos] != ':')
            throw new FormatException($"Expected ':' in: {line}");
        pos++;

        value = ReadQuoted(line, ref pos);
    }

    private static string ReadQuoted(string s, ref int pos)
    {
        while (pos < s.Length && char.IsWhiteSpace(s[pos])) pos++;
        if (pos >= s.Length || s[pos] != '"')
            throw new FormatException($"Expected '\"' in: {s}");
        pos++;

        var sb = new StringBuilder();
        while (pos < s.Length && s[pos] != '"')
        {
            if (s[pos] == '\\' && pos + 1 < s.Length)
                pos++;
            sb.Append(s[pos]);
            pos++;
        }

        if (pos >= s.Length)
            throw new FormatException($"Unterminated string in: {s}");
        pos++;
        return sb.ToString();
    }
}