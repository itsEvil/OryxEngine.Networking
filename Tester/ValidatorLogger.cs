using System.Text;
using OryxEngine.Memory.Optionals;
using OryxEngine.Networking.Validators;
using Serilog;

namespace Tester;
public class ValidatorLogger : IValidator
{
    private ushort _length;
    private ushort _id;
    public bool IsValid(Reader reader)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < reader.Length + 10; i++)
        {
            var b = reader.Buffer[i];
            sb.Append('[');
            sb.Append(b);
            sb.Append(']');
        }

        Log.Information("Bytes: {sb}", sb.ToString());
        
        _length = reader.ReadUInt16();
        Log.Information("Length: {length_received}, Reader Length: {length_reader}", _length, reader.Length);
        if (_length > reader.Length)
            return false;
                
        _id = reader.ReadUInt16();
        Log.Information("Id: {id_received}", _id);
        return IsValidId(_id);
    }
    private static bool IsValidId(ushort id)
    {
        return id == ushort.MaxValue;
    }
    public int GetLength()
    {
        return _length;
    }

    public int GetId()
    {
        return _id;
    }
}