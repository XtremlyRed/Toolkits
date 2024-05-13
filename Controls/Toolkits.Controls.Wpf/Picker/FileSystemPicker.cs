using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;
using Toolkits.Core;

namespace Toolkits.Wpf;

/// <summary>
/// pick from file system
/// </summary>
public class FileSystemPicker : IFileSystemPicker
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly System.Windows.Forms.OpenFileDialog openFileDialog = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly System.Windows.Forms.SaveFileDialog saveFileDialog = new();

    /// <summary>
    /// pick folder from file system
    /// </summary>
    /// <param name="defaultPath"></param>
    /// <param name="title"></param>
    /// <param name="showNewFolderButton"></param>
    /// <returns></returns>
    public string? FolderPicker(string? defaultPath = null, string title = "please select folder", bool showNewFolderButton = false)
    {
        folderBrowserDialog.Description = title;
        folderBrowserDialog.ShowNewFolderButton = showNewFolderButton;
        folderBrowserDialog.SelectedPath = defaultPath!;
        if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            string? selectPath = folderBrowserDialog.SelectedPath;
            return selectPath;
        }

        return default;
    }

    /// <summary>
    /// pick folder from file system
    /// </summary>
    /// <param name="defaultPath"></param>
    /// <param name="title"></param>
    /// <param name="showNewFolderButton"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string?> FolderPickerAsync(string? defaultPath = null, string title = "please select folder", bool showNewFolderButton = false)
    {
        return await await (
            Application.Current?.Dispatcher?.InvokeAsync(async () =>
            {
                folderBrowserDialog.Description = title;
                folderBrowserDialog.ShowNewFolderButton = showNewFolderButton;
                folderBrowserDialog.SelectedPath = defaultPath!;

                try
                {
                    if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string selectPath = folderBrowserDialog.SelectedPath!;
                        return selectPath;
                    }

                    return default;
                }
                finally
                {
                    //make subsequent processing smoother
                    await Task.Delay(150);
                }
            }) ?? throw new Exception("The application dispatcher is empty")
        );
    }

    /// <summary>
    /// pick file name from file system
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="title"></param>
    /// <param name="defaultFileName"></param>
    /// <param name="rootFolder"></param>
    /// <returns></returns>
    public string? FileNamePicker(
        string filter = "all file|*.*",
        string title = "please input file name",
        string? defaultFileName = null,
        string? rootFolder = null
    )
    {
        rootFolder ??= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        saveFileDialog.Filter = filter;
        saveFileDialog.SupportMultiDottedExtensions = false;
        saveFileDialog.Title = title;
        saveFileDialog.InitialDirectory = rootFolder;

        if (string.IsNullOrWhiteSpace(defaultFileName) == false)
        {
            saveFileDialog.FileName = defaultFileName;
        }

        if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            string fileName = saveFileDialog.FileName!;
            return fileName;
        }

        return default;
    }

    /// <summary>
    /// pick file name from file system
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="title"></param>
    /// <param name="defaultFileName"></param>
    /// <param name="rootFolder"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string?> FileNamePickerAsync(
        string filter = "all file|*.*",
        string title = "please input file name",
        string? defaultFileName = null,
        string? rootFolder = null
    )
    {
        return await await (
            Application.Current?.Dispatcher?.InvokeAsync(async () =>
            {
                rootFolder ??= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.Filter = filter;
                saveFileDialog.SupportMultiDottedExtensions = false;
                saveFileDialog.Title = title;
                saveFileDialog.InitialDirectory = rootFolder;

                if (string.IsNullOrWhiteSpace(defaultFileName) == false)
                {
                    saveFileDialog.FileName = defaultFileName;
                }

                try
                {
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string? fileName = saveFileDialog.FileName;
                        return fileName;
                    }

                    return default;
                }
                finally
                {
                    //make subsequent processing smoother
                    await Task.Delay(150);
                }
            }) ?? throw new Exception("The application dispatcher is empty")
        );
    }

    /// <summary>
    ///  pick file from file system
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="title"></param>
    /// <param name="rootFolder"></param>
    /// <returns></returns>

    public string? FilePicker(string filter = "all file|*.*", string title = "please select files", string? rootFolder = null)
    {
        rootFolder ??= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        openFileDialog.Multiselect = false;
        openFileDialog.Filter = filter;
        openFileDialog.Title = title;
        openFileDialog.InitialDirectory = rootFolder;
        openFileDialog.FilterIndex = 0;

        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            string fileName = openFileDialog.FileName;
            return fileName;
        }

        return default;
    }

    /// <summary>
    ///  pick files from file system
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="title"></param>
    /// <param name="rootFolder"></param>
    /// <returns></returns>
    public string[]? FilesPicker(string filter = "all file|*.*", string? title = "please select files", string? rootFolder = null)
    {
        rootFolder ??= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        openFileDialog.Multiselect = true;
        openFileDialog.Filter = filter;
        openFileDialog.Title = title;
        openFileDialog.InitialDirectory = rootFolder;
        openFileDialog.FilterIndex = 0;

        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            string[] fileNames = openFileDialog.FileNames;
            return fileNames;
        }

        return new string[] { };
    }

    /// <summary>
    ///  pick file from file system
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="title"></param>
    /// <param name="rootFolder"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string?> FilePickerAsync(string filter = "all file|*.*", string title = "please select files", string? rootFolder = null)
    {
        return await await (
            Application.Current?.Dispatcher?.InvokeAsync(async () =>
            {
                rootFolder ??= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = filter;
                openFileDialog.Title = title;
                openFileDialog.InitialDirectory = rootFolder;
                openFileDialog.FilterIndex = 0;
                try
                {
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string fileName = openFileDialog.FileName;
                        return fileName;
                    }

                    return default;
                }
                finally
                {
                    //make subsequent processing smoother
                    await Task.Delay(150);
                }
            }) ?? throw new Exception("The application dispatcher is empty")
        );
    }

    /// <summary>
    /// pick files from file system
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="title"></param>
    /// <param name="rootFolder"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string[]?> FilesPickerAsync(string filter = "all file|*.*", string title = "please select files", string? rootFolder = null)
    {
        return await await (
            Application.Current?.Dispatcher?.InvokeAsync(async () =>
            {
                rootFolder ??= Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = filter;
                openFileDialog.Title = title;
                openFileDialog.InitialDirectory = rootFolder;
                openFileDialog.FilterIndex = 0;

                try
                {
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string[] fileNames = openFileDialog.FileNames;
                        return fileNames;
                    }

                    return new string[] { };
                }
                finally
                {
                    //make subsequent processing smoother
                    await Task.Delay(150);
                }
            }) ?? throw new Exception("The application dispatcher is empty")
        );
    }

    #region hide base function

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj"> The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
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

    #endregion
}
