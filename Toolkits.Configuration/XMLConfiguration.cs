using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Toolkits.Configuration;

/// <summary>
/// a class of <see cref="XMLConfiguration"/>
/// </summary>
/// <seealso cref="Toolkits.Configuration.IConfiguration" />
public class XMLConfiguration : IConfiguration
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly FileInfo configurationPath;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private XmlDocument xmlDocument;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly SemaphoreSlim asyncLocker = new SemaphoreSlim(1, 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly XmlWriterSettings serializerSettings;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly char[] splitChars = new[] { '.', ' ' };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string intervalChar = ".";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static Type rootValueType = typeof(RootValue<>);

    /// <summary>
    /// Initializes the <see cref="XMLConfiguration"/> class.
    /// </summary>
    static XMLConfiguration()
    {
        serializerSettings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            IndentChars = "    ",
            NewLineChars = Environment.NewLine,
            OmitXmlDeclaration = true
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XMLConfiguration"/> class.
    /// </summary>
    /// <param name="configurationPath">The configuration path.</param>
    public XMLConfiguration(FileInfo configurationPath)
    {
        this.configurationPath = configurationPath;

        xmlDocument = new XmlDocument();

        if (configurationPath.Exists)
        {
            using var fs = File.OpenRead(configurationPath.FullName);
            xmlDocument.Load(fs);
        }
        else
        {
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDocument.CreateElement("Root");
            xmlDocument.AppendChild(xmlDeclaration);
            xmlDocument.AppendChild(root);
        }
    }

    /// <summary>
    /// Gets the specified key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">key</exception>
    public T Get<T>(string key, T defaultValue = default!)
    {
        _ = string.IsNullOrWhiteSpace(key) ? throw new ArgumentNullException(nameof(key)) : 0;

        try
        {
            asyncLocker.Wait();

            XmlNode? valueToken = InternalGet(key);

            if (valueToken is null)
            {
                if (defaultValue is not null)
                {
                    IntervalSet(key, defaultValue);
                }

                return defaultValue;
            }

            var type = typeof(T);

            var xmlString =
                $"<?xml version=\"1.0\" encoding=\"utf-8\"?><RootValue><value>{valueToken.InnerXml}</value></RootValue>";

            var dataType = rootValueType.MakeGenericType(type);

            using StringReader stringReader = new StringReader(xmlString);

            XmlSerializer serializer = new XmlSerializer(dataType);

            var retvarurn = (IRootValue)serializer.Deserialize(stringReader)!;

            return (T)retvarurn.Value;
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="ArgumentNullException">
    /// key
    /// or
    /// value
    /// </exception>
    public void Set<T>(string key, T value)
    {
        _ = string.IsNullOrWhiteSpace(key) ? throw new ArgumentNullException(nameof(key)) : 0;
        _ = value ?? throw new ArgumentNullException(nameof(value));

        try
        {
            asyncLocker.Wait();

            IntervalSet(key, value);
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    /// <summary>
    /// Clears this instance.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Clear()
    {
        try
        {
            asyncLocker.Wait();

            xmlDocument = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDocument.CreateElement("Root");
            xmlDocument.AppendChild(xmlDeclaration);
            xmlDocument.AppendChild(root);

            WriteIn();
        }
        finally
        {
            asyncLocker.Release();
        }
    }

    private XmlNode? InternalGet(string key)
    {
        string[] keys = key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
        XmlNode? node = xmlDocument.DocumentElement;

        foreach (string k in keys)
        {
            node = node?.SelectSingleNode(k);
            if (node == null)
            {
                return null;
            }
        }

        return node;
    }

    private void IntervalSet<T>(string key, T value)
    {
        string[] keys = key.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
        XmlNode? node = xmlDocument.DocumentElement;

        for (int i = 0; i < keys.Length - 1; i++)
        {
            XmlNode? childNode = node?.SelectSingleNode(keys[i]);
            if (childNode == null)
            {
                XmlElement newElement = xmlDocument.CreateElement(keys[i]);
                node!.AppendChild(newElement);
                node = newElement;
            }
            else
            {
                node = childNode;
            }
        }

        XmlNode? targetNode = node?.SelectSingleNode(keys[keys.Length - 1]);
        if (targetNode == null)
        {
            XmlElement newElement = xmlDocument.CreateElement(keys[keys.Length - 1]);
            if (value != null)
            {
                XmlSerializer serializer = new XmlSerializer(value.GetType());
                using MemoryStream stream = new MemoryStream();

                serializer.Serialize(stream, value);
                stream.Position = 0;
                XmlDocument valueDoc = new XmlDocument();
                valueDoc.Load(stream);

                var length = valueDoc.DocumentElement!.ChildNodes.Count;

                for (int i = 0; i < length; i++)
                {
                    XmlNode xmlNode = valueDoc.DocumentElement!.ChildNodes[i]!;
                    XmlNode importedNode = xmlDocument.ImportNode(xmlNode, true);
                    newElement.AppendChild(importedNode);
                }

                valueDoc = null!;
                serializer = null!;
            }
            node!.AppendChild(newElement);
        }
        else
        {
            targetNode.RemoveAll();

            if (value != null)
            {
                XmlSerializer serializer = new XmlSerializer(value.GetType());
                using MemoryStream stream = new MemoryStream();

                serializer.Serialize(stream, value);
                stream.Position = 0;
                XmlDocument valueDoc = new XmlDocument();
                valueDoc.Load(stream);

                var length = valueDoc.DocumentElement!.ChildNodes.Count;

                for (int i = 0; i < length; i++)
                {
                    XmlNode xmlNode = valueDoc.DocumentElement!.ChildNodes[i]!;
                    XmlNode importedNode = xmlDocument.ImportNode(xmlNode, true);
                    targetNode.AppendChild(importedNode);
                }

                valueDoc = null!;
                serializer = null!;
            }
        }

        WriteIn();
    }

    private void WriteIn()
    {
        var clone = (XmlDocument)xmlDocument.Clone();

        using var writer = XmlWriter.Create(configurationPath.FullName, serializerSettings);

        clone.Save(writer);

        clone = null!;
    }

    private interface IRootValue
    {
        object Value { get; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Toolkits.Configuration.XMLConfiguration.IRootValue" />
    [XmlRoot("RootValue")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class RootValue<T> : IRootValue
    {
        /// <summary>
        /// current value.
        /// </summary>
        [XmlElement(ElementName = "value")]
        public T Value { get; set; } = default!;

        /// <summary>
        /// current value.
        /// </summary>
        [XmlIgnore]
        object IRootValue.Value => Value!;
    }
}
