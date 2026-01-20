namespace NbgDev.Pst.Projects.Contract.Mediator;

/// <summary>
/// Represents a void type for requests that don't return a value
/// </summary>
public readonly struct Unit
{
    public static readonly Unit Value = new();
}
