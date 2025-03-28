using OryxEngine.Memory.Optionals;

// ReSharper disable once CheckNamespace
namespace OryxEngine.Networking.Validators;

public interface IValidator
{
    /// <summary>
    /// Validate the packet header
    /// </summary>
    public bool IsValid(Reader reader);
    /// <summary>
    /// Return the valid length in the header
    /// </summary>
    /// <returns></returns>
    public int GetLength();
    /// <summary>
    /// Return the valid packet id in the header
    /// </summary>
    /// <returns></returns>
    public int GetId();
}