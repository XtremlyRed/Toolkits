namespace Toolkits.Controls;

/// <summary>
///
/// </summary>
public interface IFileSystemPicker
{
    /// <summary>
    /// Files the name picker.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="title">The title.</param>
    /// <param name="defaultFileName">Default name of the file.</param>
    /// <param name="rootFolder">The root folder.</param>
    /// <returns></returns>
    string? FileNamePicker(
        string filter = "all file|*.*",
        string title = "please input file name",
        string? defaultFileName = null,
        string? rootFolder = null
    );

    /// <summary>
    /// Files the name picker asynchronous.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="title">The title.</param>
    /// <param name="defaultFileName">Default name of the file.</param>
    /// <param name="rootFolder">The root folder.</param>
    /// <returns></returns>
    Task<string?> FileNamePickerAsync(
        string filter = "all file|*.*",
        string title = "please input file name",
        string? defaultFileName = null,
        string? rootFolder = null
    );

    /// <summary>
    /// Files the picker.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="title">The title.</param>
    /// <param name="rootFolder">The root folder.</param>
    /// <returns></returns>
    string? FilePicker(
        string filter = "all file|*.*",
        string title = "please select files",
        string? rootFolder = null
    );

    /// <summary>
    /// Files the picker asynchronous.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="title">The title.</param>
    /// <param name="rootFolder">The root folder.</param>
    /// <returns></returns>
    Task<string?> FilePickerAsync(
        string filter = "all file|*.*",
        string title = "please select files",
        string? rootFolder = null
    );

    /// <summary>
    /// Fileses the picker.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="title">The title.</param>
    /// <param name="rootFolder">The root folder.</param>
    /// <returns></returns>
    string[]? FilesPicker(
        string filter = "all file|*.*",
        string? title = "please select files",
        string? rootFolder = null
    );

    /// <summary>
    /// Fileses the picker asynchronous.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="title">The title.</param>
    /// <param name="rootFolder">The root folder.</param>
    /// <returns></returns>
    Task<string[]?> FilesPickerAsync(
        string filter = "all file|*.*",
        string title = "please select files",
        string? rootFolder = null
    );

    /// <summary>
    /// Folders the picker.
    /// </summary>
    /// <param name="defaultPath">The default path.</param>
    /// <param name="title">The title.</param>
    /// <param name="showNewFolderButton">if set to <c>true</c> [show new folder button].</param>
    /// <returns></returns>
    string? FolderPicker(
        string? defaultPath = null,
        string title = "please select folder",
        bool showNewFolderButton = false
    );

    /// <summary>
    /// Folders the picker asynchronous.
    /// </summary>
    /// <param name="defaultPath">The default path.</param>
    /// <param name="title">The title.</param>
    /// <param name="showNewFolderButton">if set to <c>true</c> [show new folder button].</param>
    /// <returns></returns>
    Task<string?> FolderPickerAsync(
        string? defaultPath = null,
        string title = "please select folder",
        bool showNewFolderButton = false
    );
}
