using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Toolkits.Core;

/// <summary>
/// <see cref="IRelayCommand"/>
/// </summary>
public interface IRelayCommand : ICommand
{
    /// <summary>
    /// can execute
    /// </summary>
    /// <returns></returns>
    bool CanExecute();

    /// <summary>
    /// execute command
    /// </summary>
    void Execute();
}

/// <summary>
/// RelayCommand
/// </summary>
public class RelayCommand : CommandBase, IRelayCommand, ICommand
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<bool> canExecute;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action execute;

    /// <summary>
    /// create a new command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ??= () => true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="commandName">Name of the command.</param>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    /// <exception cref="ArgumentNullException">
    /// commandName
    /// or
    /// execute
    /// </exception>
    public RelayCommand(string commandName, Action execute, Func<bool>? canExecute = null)
    {
        base.CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ??= () => true;
    }

    /// <summary>
    /// Determines whether this instance can execute the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>
    ///   <c>true</c> if this instance can execute the specified parameter; otherwise, <c>false</c>.
    /// </returns>
    protected override bool CanExecute(object parameter)
    {
        return CanExecute();
    }

    /// <summary>
    /// Executes the specified parameter.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    protected override void Execute(object parameter)
    {
        Execute();
    }

    /// <summary>
    ///  can cexcute command
    /// </summary>
    /// <returns></returns>
    public bool CanExecute()
    {
        return !IsExecuting && (canExecute?.Invoke() ?? true);
    }

    /// <summary>
    /// execute sync command
    /// </summary>
    public void Execute()
    {
        try
        {
            base.IsExecuting = true;

            RaiseCanExecuteChanged();
            execute.Invoke();
        }
        catch (Exception ex)
        {
            if (globalCommandExceptionCallback is null)
            {
                throw;
            }
            globalCommandExceptionCallback.Invoke(CommandName, ex);
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

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static Action<string, Exception>? globalCommandExceptionCallback;

    /// <summary>
    /// set global command exception callback
    /// </summary>
    /// <param name="globalCommandExceptionCallback"></param>
    public static void SetGlobalCommandExceptionCallback(
        Action<string, Exception> globalCommandExceptionCallback
    )
    {
        RelayCommand.globalCommandExceptionCallback = globalCommandExceptionCallback;
    }

    /// <summary>
    /// create relaycommand from  <see cref="Action"/> <paramref name="commandAction"/>
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator RelayCommand((Action action, string commandName) commandAction)
    {
        return new RelayCommand(commandAction.action) { CommandName = commandAction.commandName };
    }

    /// <summary>
    /// create relaycommand from  <see cref="Action"/> <paramref name="commandAction"/>
    /// </summary>
    /// <param name="commandAction"></param>
    public static implicit operator RelayCommand(Action commandAction)
    {
        return new RelayCommand(commandAction);
    }
}
