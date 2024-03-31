using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Toolkits;

/// <summary>
/// <see cref="IRelayCommandAsync"/>
/// </summary>
public interface IRelayCommandAsync : ICommand
{
    /// <summary>
    /// can execute
    /// </summary>
    /// <returns></returns>
    bool CanExecute();

    /// <summary>
    /// execute command async
    /// </summary>
    /// <returns></returns>
    Task ExecuteAsync();
}

/// <summary>
/// <see cref="RelayCommandAsync"/>
/// </summary>
public class RelayCommandAsync : CommandBase, IRelayCommandAsync
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<Task> execute;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<bool>? canExecute = null;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public RelayCommandAsync(Func<Task> execute, Func<bool>? canExecute = null)
    {
        this.execute = execute ?? throw new Exception(nameof(execute));
        this.canExecute = canExecute ??= () => true;
    }

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public RelayCommandAsync(string commandName, Func<Task> execute, Func<bool>? canExecute = null)
    {
        base.CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));

        this.execute = execute ?? throw new Exception(nameof(execute));
        this.canExecute = canExecute ??= () => true;
    }

    protected override bool CanExecute(object parameter)
    {
        return CanExecute();
    }

    protected override async void Execute(object parameter)
    {
        await ExecuteAsync();
    }

    /// <summary>
    /// can execute
    /// </summary>
    /// <returns></returns>
    public bool CanExecute()
    {
        return !IsExecuting && (canExecute?.Invoke() ?? true);
    }

    /// <summary>
    /// execute command async
    /// </summary>
    /// <returns></returns>
    public async Task ExecuteAsync()
    {
        try
        {
            IsExecuting = true;
            RaiseCanExecuteChanged();

            await execute();
        }
        catch (Exception ex)
        {
            if (RelayCommand.globalCommandExceptionCallback is null)
            {
                throw;
            }
            RelayCommand.globalCommandExceptionCallback.Invoke(CommandName, ex);
        }
        finally
        {
            IsExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator RelayCommandAsync(Func<Task> commandAction)
    {
        return new RelayCommandAsync(commandAction);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator RelayCommandAsync(
        (Func<Task> func, string commandName) commandAction
    )
    {
        return new RelayCommandAsync(commandAction.func)
        {
            CommandName = commandAction.commandName
        };
    }
}
