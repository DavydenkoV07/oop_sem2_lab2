namespace App.UI;

/// <summary>
/// Interface for implementing the Command pattern.
/// Encapsulates the request as an object, allowing clients to be parameterized with different requests.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Text description of the command to display in the interface (menu).
    /// </summary>
    string Description { get; }

    /// <summary>
    /// A method that triggers the execution of an encapsulated action.
    /// </summary>
    void Execute();
}