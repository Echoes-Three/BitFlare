using System.Windows.Input;

namespace BitFlare.MVVM;

public class RelayCommand : ICommand
{
    public Action<object> execute;
    public Func<object, bool>? canExecute;
    
    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<object> execute,  Func<object, bool>? canExecute = null)
    {
        this.execute =  execute;
        this.canExecute = canExecute;
    }
    
    public bool CanExecute(object? parameter)
    {
        return canExecute == null || canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        execute(parameter);
    }

}