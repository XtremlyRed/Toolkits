using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Toolkits;

/// <summary>
/// a class of <see cref="CommandBase"/>
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[DebuggerDisplay("{CommandName}")]
public abstract class CommandBase : ICommand, INotifyPropertyChanged
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly PropertyChangedEventArgs IsExecutingProperty = new(nameof(IsExecuting));

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly PropertyChangedEventArgs CommandNameProperty = new(nameof(CommandName));

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private bool isExecuting;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string commandName = string.Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected SynchronizationContext SynchronizationContext = SynchronizationContext.Current;

    /// <summary>
    ///
    /// </summary>

    public string CommandName
    {
        get => commandName;
        set
        {
            if (commandName != value)
            {
                commandName = value;
                PropertyChanged?.Invoke(this, CommandNameProperty);
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is executing.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is executing; otherwise, <c>false</c>.
    /// </value>
    public bool IsExecuting
    {
        get => isExecuting;
        protected set
        {
            if (isExecuting != value)
            {
                isExecuting = value;
                PropertyChanged?.Invoke(this, IsExecutingProperty);
            }
        }
    }

    /// <summary>
    /// Raises the can execute changed.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public virtual void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// can execute changed event
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    #region hide base function

    /// <summary>
    /// Determines whether the specified object is equal toType the current object.
    /// </summary>
    /// <param name="obj"> The object toType compare with the current object.</param>
    /// <returns>true if the specified object is equal toType the current object; otherwise, false.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    /// <summary>
    ///  Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString()
    {
        return base.ToString();
    }

    /// <summary>
    /// property changed event
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    ///
    /// <summary>
    /// raise property changed
    /// </summary>
    /// <param name="propertyName"></param>
    protected virtual void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    bool ICommand.CanExecute(object parameter)
    {
        return CanExecute(parameter);
    }

    void ICommand.Execute(object parameter)
    {
        Execute(parameter);
    }

    protected abstract bool CanExecute(object parameter);

    protected abstract void Execute(object parameter);
}
    #endregion
