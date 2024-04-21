using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Toolkits.Configuration.Internal;

namespace Toolkits.Configuration;

[DebuggerDisplay("{xmlDocument}")]
public class XMLConfiguration : IConfiguration
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly FileInfo configurationPath;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private XmlDocument xmlDocument;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly AsyncLocker asyncLocker = new AsyncLocker(1, 1);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly XmlWriterSettings serializerSettings;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static readonly char[] splitChars = new[] { '.', ' ' };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const string intervalChar = ".";

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

    public XMLConfiguration(FileInfo configurationPath)
    {
        this.configurationPath = configurationPath;

        xmlDocument = new XmlDocument();

        if (configurationPath.Exists)
        {
            xmlDocument.Load(configurationPath.FullName);
        }
        else
        {
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDocument.CreateElement("Root");
            xmlDocument.AppendChild(xmlDeclaration);
            xmlDocument.AppendChild(root);
        }
    }

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

            using StringReader stringReader = new StringReader(valueToken.InnerXml);

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader)!;
        }
        finally
        {
            asyncLocker.Release();
        }
    }

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

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Clear()
    {
        try
        {
            xmlDocument = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = xmlDocument.CreateElement("Root");
            xmlDocument.AppendChild(xmlDeclaration);
            xmlDocument.AppendChild(root);
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
                XmlNode importedNode = xmlDocument.ImportNode(valueDoc.DocumentElement!, true);
                newElement.AppendChild(importedNode);
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
                XmlNode importedNode = xmlDocument.ImportNode(valueDoc.DocumentElement!, true);
                targetNode.AppendChild(importedNode);
            }
        }

        var writer = XmlWriter.Create(configurationPath.FullName, serializerSettings);

        xmlDocument.Save(writer);
    }
}
