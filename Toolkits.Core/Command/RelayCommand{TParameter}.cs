using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Input;

namespace Toolkits.Core;

/// <summary>
/// <see cref="IRelayCommand{TParameter}"/>
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public interface IRelayCommand<in TParameter> : ICommand
{
    /// <summary>
    /// can execute command
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    bool CanExecute(TParameter parameter);

    /// <summary>
    /// execute command
    /// </summary>
    /// <param name="parameter"></param>
    void Execute(TParameter parameter);
}

/// <summary>
/// RelayCommand
/// </summary>
/// <typeparam name="TParameter"></typeparam>
public class RelayCommand<TParameter> : CommandBase, ICommand, IRelayCommand<TParameter>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<TParameter> execute;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<TParameter, bool> canExecute = null!;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public RelayCommand(Action<TParameter> execute, Func<TParameter, bool>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ??= i => true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{TParameter}"/> class.
    /// </summary>
    /// <param name="commandName">Name of the command.</param>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    /// <exception cref="ArgumentNullException">
    /// commandName
    /// or
    /// execute
    /// </exception>
    public RelayCommand(
        string commandName,
        Action<TParameter> execute,
        Func<TParameter, bool>? canExecute = null
    )
    {
        base.CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ??= (i) => true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>

    protected override bool CanExecute(object parameter)
    {
        return parameter is not TParameter parameter1 || CanExecute(parameter1);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>

    protected override void Execute(object parameter)
    {
        if (parameter is TParameter parameter2)
        {
            Execute(parameter2);
        }
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
    /// execute command with <typeparamref name="TParameter"/> <paramref name="parameter"/>
    /// </summary>
    /// <param name="parameter"></param>
    public void Execute(TParameter parameter)
    {
        try
        {
            IsExecuting = true;
            RaiseCanExecuteChanged();
            execute.Invoke(parameter);
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
    public static implicit operator RelayCommand<TParameter>(Action<TParameter> commandAction)
    {
        return new RelayCommand<TParameter>(commandAction);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator RelayCommand<TParameter>(
        (Action<TParameter> action, string commandName) commandAction
    )
    {
        return new RelayCommand<TParameter>(commandAction.action)
        {
            CommandName = commandAction.commandName
        };
    }
}
