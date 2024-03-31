using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Toolkits;

/// <summary>
/// <see cref="IRelayCommandAsync{TParameter}"/>
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public interface IRelayCommandAsync<in TParameter> : ICommand
{
    /// <summary>
    /// can execute command
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    bool CanExecute(TParameter parameter);

    /// <summary>
    /// execute command async
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    Task ExecuteAsync(TParameter parameter);
}

/// <summary>
/// <see cref="RelayCommandAsync{TParameter}"/>
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public class RelayCommandAsync<TParameter> : CommandBase, ICommand, IRelayCommandAsync<TParameter>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<TParameter, Task> execute;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<TParameter, bool>? canExecute = null;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public RelayCommandAsync(
        Func<TParameter, Task> execute,
        Func<TParameter, bool>? canExecute = null
    )
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ??= i => true;
    }

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public RelayCommandAsync(
        string commandName,
        Func<TParameter, Task> execute,
        Func<TParameter, bool>? canExecute = null
    )
    {
        base.CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));

        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ??= i => true;
    }

    protected override bool CanExecute(object parameter)
    {
        return parameter is not TParameter parameter1 || CanExecute(parameter1);
    }

    protected override async void Execute(object parameter)
    {
        if (parameter is TParameter parameter1)
        {
            await ExecuteAsync(parameter1);
        }

#if NET451
        await Task.FromResult(false);
#else
        await Task.CompletedTask;
#endif
    }

    /// <summary>
    /// can execute with <typeparamref name="TParameter"/> <paramref name="parameter"/>
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(TParameter parameter)
    {
        return !IsExecuting && (canExecute?.Invoke(parameter) ?? true);
    }

    /// <summary>
    /// execute command with <typeparamref name="TParameter"/> <paramref name="parameter"/> async
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public async Task ExecuteAsync(TParameter parameter)
    {
        try
        {
            IsExecuting = true;
            RaiseCanExecuteChanged();

            await execute(parameter);
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
    public static implicit operator RelayCommandAsync<TParameter>(
        Func<TParameter, Task> commandAction
    )
    {
        return new RelayCommandAsync<TParameter>(commandAction);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator RelayCommandAsync<TParameter>(
        (Func<TParameter, Task> func, string commandName) commandAction
    )
    {
        return new RelayCommandAsync<TParameter>(commandAction.func)
        {
            CommandName = commandAction.commandName
        };
    }
}
