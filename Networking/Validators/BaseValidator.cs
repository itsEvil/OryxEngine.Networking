using OryxEngine.Memory.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Validators;
public class BaseValidator : IValidator
{
    private ushort _length;
    private ushort _id;
    public bool IsValid(Reader reader)
    {
        _length = reader.ReadUInt16();
        if (_length > reader.Length)
            return false;
                
        _id = reader.ReadUInt16();
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