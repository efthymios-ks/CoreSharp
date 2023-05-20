using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace CoreSharp.Extensions;

/// <summary>
/// <see cref="XDocument"/> extensions.
/// </summary>
public static class XDocumentExtensions
{
    /// <inheritdoc cref="StreamExtensions.FromXmlAsync{TEntity}(Stream, CancellationToken)"/>
    public static TEntity ToEntity<TEntity>(this XDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var xmlSerializer = new XmlSerializer(typeof(TEntity));
        using var reader = document.Root?.CreateReader();
        return (TEntity)xmlSerializer.Deserialize(reader!);
    }

    /// <summary>
    /// Get all XElements on given path.
    /// </summary>
    public static IEnumerable<XElement> GetElements(this XDocument document, params string[] pathSections)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(pathSections);

        // Build XPath
        var xpathExpression = string.Join("/", pathSections);

        // Get elements 
        return document.XPathSelectElements(xpathExpression);
    }

    /// <inheritdoc cref="WhereAttribute(IEnumerable{XElement}, string, Predicate{string})"/>
    public static IEnumerable<XElement> WhereAttribute(this IEnumerable<XElement> source, string attributeName, string attributeValue)
       => source.WhereAttribute(attributeName, i => i == attributeValue);

    /// <summary>
    /// Filter XElement collection on attribute value predicate.
    /// </summary>
    public static IEnumerable<XElement> WhereAttribute(this IEnumerable<XElement> source, string attributeName, Predicate<string> attributeValueSelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(attributeValueSelector);
        ArgumentException.ThrowIfNullOrEmpty(attributeName);

        return source
                 .Where(i => attributeValueSelector(i.Attribute(attributeName)?.Value))
                 .ToArray();
    }

    /// <inheritdoc cref="WhereChild(IEnumerable{XElement}, string, Predicate{string})"/>
    public static IEnumerable<XElement> WhereChild(this IEnumerable<XElement> source, string childName, string childValue)
       => source.WhereChild(childName, i => i == childValue);

    /// <summary>
    /// Filter XElement collection on children predicate.
    /// </summary>
    public static IEnumerable<XElement> WhereChild(this IEnumerable<XElement> source, string childName, Predicate<string> childValueSelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(childValueSelector);
        ArgumentException.ThrowIfNullOrEmpty(childName);

        return source.Where(i => childValueSelector(i.Element(childName)?.Value))
                     .ToArray();
    }
}
