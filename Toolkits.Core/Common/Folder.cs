using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Toolkits.Core;

/// <summary>
/// <see cref="Folder"/>
/// </summary>

[DebuggerDisplay("{folder}")]
public readonly struct Folder : IEquatable<object>, IEquatable<Folder>
{
    /// <summary>
    /// <see cref="Environment.SpecialFolder.Desktop"/> folder
    /// </summary>
    public static Folder Desktop = new(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Programs"/> folder
    /// </summary>
    public static Folder Programs = new(Environment.GetFolderPath(Environment.SpecialFolder.Programs));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.MyDocuments"/> folder
    /// </summary>
    public static Folder MyDocuments = new(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Favorites"/> folder
    /// </summary>
    public static Folder Favorites = new(Environment.GetFolderPath(Environment.SpecialFolder.Favorites));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Startup"/> folder
    /// </summary>
    public static Folder Startup = new(Environment.GetFolderPath(Environment.SpecialFolder.Startup));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Recent"/> folder
    /// </summary>
    public static Folder Recent = new(Environment.GetFolderPath(Environment.SpecialFolder.Recent));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.SendTo"/> folder
    /// </summary>
    public static Folder SendTo = new(Environment.GetFolderPath(Environment.SpecialFolder.SendTo));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.StartMenu"/> folder
    /// </summary>
    public static Folder StartMenu = new(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.MyMusic"/> folder
    /// </summary>
    public static Folder MyMusic = new(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.MyVideos"/> folder
    /// </summary>
    public static Folder MyVideos = new(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.DesktopDirectory"/> folder
    /// </summary>
    public static Folder DesktopDirectory = new(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.MyComputer"/> folder
    /// </summary>
    public static Folder MyComputer = new(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.NetworkShortcuts"/> folder
    /// </summary>
    public static Folder NetworkShortcuts = new(Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Fonts"/> folder
    /// </summary>
    public static Folder Fonts = new(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Templates"/> folder
    /// </summary>
    public static Folder Templates = new(Environment.GetFolderPath(Environment.SpecialFolder.Templates));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonStartMenu"/> folder
    /// </summary>
    public static Folder CommonStartMenu = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonPrograms"/> folder
    /// </summary>
    public static Folder CommonPrograms = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonStartup"/> folder
    /// </summary>
    public static Folder CommonStartup = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonDesktopDirectory"/> folder
    /// </summary>
    public static Folder CommonDesktopDirectory = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.ApplicationData"/> folder
    /// </summary>
    public static Folder ApplicationData = new(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.PrinterShortcuts"/> folder
    /// </summary>
    public static Folder PrinterShortcuts = new(Environment.GetFolderPath(Environment.SpecialFolder.PrinterShortcuts));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.LocalApplicationData"/> folder
    /// </summary>
    public static Folder LocalApplicationData = new(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.InternetCache"/> folder
    /// </summary>
    public static Folder InternetCache = new(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Cookies"/> folder
    /// </summary>
    public static Folder Cookies = new(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.History"/> folder
    /// </summary>
    public static Folder History = new(Environment.GetFolderPath(Environment.SpecialFolder.History));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonApplicationData"/> folder
    /// </summary>
    public static Folder CommonApplicationData = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Windows"/> folder
    /// </summary>
    public static Folder Windows = new(Environment.GetFolderPath(Environment.SpecialFolder.Windows));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.System"/> folder
    /// </summary>
    public static Folder System = new(Environment.GetFolderPath(Environment.SpecialFolder.System));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.ProgramFiles"/> folder
    /// </summary>
    public static Folder ProgramFiles = new(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.MyPictures"/> folder
    /// </summary>
    public static Folder MyPictures = new(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.UserProfile"/> folder
    /// </summary>
    public static Folder UserProfile = new(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.SystemX86"/> folder
    /// </summary>
    public static Folder SystemX86 = new(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.ProgramFilesX86"/> folder
    /// </summary>
    public static Folder ProgramFilesX86 = new(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonProgramFiles"/> folder
    /// </summary>
    public static Folder CommonProgramFiles = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonProgramFilesX86"/> folder
    /// </summary>
    public static Folder CommonProgramFilesX86 = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonTemplates"/> folder
    /// </summary>
    public static Folder CommonTemplates = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonTemplates));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonDocuments"/> folder
    /// </summary>
    public static Folder CommonDocuments = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonAdminTools"/> folder
    /// </summary>
    public static Folder CommonAdminTools = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonAdminTools));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.AdminTools"/> folder
    /// </summary>
    public static Folder AdminTools = new(Environment.GetFolderPath(Environment.SpecialFolder.AdminTools));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonMusic"/> folder
    /// </summary>
    public static Folder CommonMusic = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonPictures"/> folder
    /// </summary>
    public static Folder CommonPictures = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonVideos"/> folder
    /// </summary>
    public static Folder CommonVideos = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.Resources"/> folder
    /// </summary>
    public static Folder Resources = new(Environment.GetFolderPath(Environment.SpecialFolder.Resources));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.LocalizedResources"/> folder
    /// </summary>
    public static Folder LocalizedResources = new(Environment.GetFolderPath(Environment.SpecialFolder.LocalizedResources));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CommonOemLinks"/> folder
    /// </summary>
    public static Folder CommonOemLinks = new(Environment.GetFolderPath(Environment.SpecialFolder.CommonOemLinks));

    /// <summary>
    /// <see cref="Environment.SpecialFolder.CDBurning"/> folder
    /// </summary>
    public static Folder CDBurning = new(Environment.GetFolderPath(Environment.SpecialFolder.CDBurning));

    /// <summary>
    /// <see cref="Environment.CurrentDirectory"/> folder
    /// </summary>
    public static Folder Current = new(Environment.CurrentDirectory);

    /// <summary>
    /// auto create folder when not exist
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static bool AutoCreateFolder = true;

    /// <summary>
    /// share a new <see cref="Folder"/> folder
    /// </summary>
    public Folder(string folder)
    {
        this.folder = folder;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string folder;

    /// <summary>
    /// combines four strings into a path.
    /// </summary>
    /// <param name="paths">an array of parts of the path.</param>
    /// <Exception cref="System.ArgumentException">one of the strings in the array contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars.</Exception>
    /// <Exception cref="System.ArgumentNullException">one of the strings in the array is null.</Exception>
    /// <returns>The combined paths.</returns>
    public Folder Combine(params string[] paths)
    {
        if (paths is null || paths.Length == 0)
        {
            throw new ArgumentNullException(nameof(paths));
        }

        var path2s = Enumerable.Repeat(folder, 1).Concat(paths)!.ToArray();

        return new Folder(Path.Combine(path2s));
    }

    /// <summary>
    /// combines four strings into a path.
    /// </summary>
    /// <param name="paths">an array of parts of the path.</param>
    /// <Exception cref="System.ArgumentException">one of the strings in the array contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars.</Exception>
    /// <Exception cref="System.ArgumentNullException">one of the strings in the array is null.</Exception>
    /// <returns>The combined paths.</returns>
    public static Folder CombinePaths(params string[] paths)
    {
        if (paths is null || paths.Length == 0)
        {
            throw new ArgumentNullException(nameof(paths));
        }

        return new Folder(Path.Combine(paths));
    }

    /// <summary>
    /// get folder string from <paramref name="folder"/>
    /// </summary>
    /// <param name="folder"></param>
    public static implicit operator string(Folder folder)
    {
        if (AutoCreateFolder)
        {
            folder.TryCreateFolder();
        }
        return folder.folder;
    }

    /// <summary>
    ///  Create Directory If Not Exists
    /// </summary>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public Folder TryCreateFolder()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folder);

        if (directoryInfo.Exists == false)
        {
            directoryInfo.Create();
        }

        directoryInfo = default!;

        return this;
    }

    /// <summary>
    /// create <see cref="Folder"/> from string
    /// </summary>
    /// <param name="path"></param>
    public static implicit operator Folder(string path)
    {
        return new Folder(path);
    }

    /// <summary>
    /// get <see cref="Folder"/> hash code
    /// </summary>
    /// <returns>hash code</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode()
    {
        return folder.GetHashCode();
    }

    /// <summary>
    /// compare two objects for equality
    /// </summary>
    /// <param name="obj">compare object</param>
    /// <returns>compare result</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj)
    {
        return obj is Folder easyFolder && string.Compare(easyFolder.folder, folder, true) == 0;
    }

    /// <summary>
    /// Converts to string.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString()
    {
        return folder;
    }

    /// <summary>
    /// compare two objects for equality
    /// </summary>
    /// <param name="obj">compare object</param>
    /// <returns>compare result</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool Equals(Folder obj)
    {
        return string.Compare(obj.folder, folder, true) == 0;
    }

    /// <summary>
    /// compare two objects for equality
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Folder left, Folder right)
    {
        return string.Compare(left.folder, right.folder, true) == 0;
    }

    /// <summary>
    /// compare two objects for not equality
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Folder left, Folder right)
    {
        return string.Compare(left.folder, right.folder, true) != 0;
    }
}
