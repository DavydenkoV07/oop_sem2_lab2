namespace App.UI;

public interface ICommand
{
    string Description { get; }
    void Execute();
}