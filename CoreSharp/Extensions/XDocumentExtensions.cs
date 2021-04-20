﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace CoreSharp.Extensions
{
    //TODO: Add unit tests 
    /// <summary>
    /// XDocument extensions. 
    /// </summary>
    public static partial class XDocumentExtensions
    {
        /// <summary>
        /// Deserialize XDocument to T item. 
        /// </summary> 
        public static T Deserialize<T>(this XDocument document)
        {
            document = document ?? throw new ArgumentNullException(nameof(document));

            var xmlSerializer = new XmlSerializer(typeof(T));
            using var reader = document.Root.CreateReader();
            return (T)xmlSerializer.Deserialize(reader);
        }

        /// <summary>
        /// Get all XElements on given path. 
        /// </summary>
        public static IEnumerable<XElement> GetElements(this XDocument document, params string[] pathSections)
        {
            document = document ?? throw new ArgumentNullException(nameof(document));
            pathSections = pathSections ?? throw new ArgumentNullException(nameof(pathSections));

            //Build XPath
            var xpathExpression = string.Join("/", pathSections);

            //Get elements 
            var elements = document.XPathSelectElements(xpathExpression);

            return elements;
        }

        /// <summary>
        /// Filter XElement collection on AttributeValue. 
        /// </summary>
        public static IEnumerable<XElement> WhereAttribute(this IEnumerable<XElement> source, string attributeName, string attributeValue)
        {
            return source.WhereAttribute(attributeName, i => i == attributeValue);
        }

        /// <summary>
        /// Filter XElement collection on attribute value predicate. 
        /// </summary>
        public static IEnumerable<XElement> WhereAttribute(this IEnumerable<XElement> source, string attributeName, Predicate<string> attributeValueSelector)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            attributeValueSelector = attributeValueSelector ?? throw new ArgumentNullException(nameof(attributeValueSelector));
            if (string.IsNullOrWhiteSpace(attributeName))
                throw new ArgumentNullException(nameof(attributeName));

            return source
                     .Where(i => attributeValueSelector(i.Attribute(attributeName).Value))
                     .ToArray();
        }

        /// <summary>
        /// Filter XElement collection on children value. 
        /// </summary>
        public static IEnumerable<XElement> WhereChild(this IEnumerable<XElement> source, string childName, string childValue)
        {
            return source.WhereChild(childName, i => i == childValue);
        }

        /// <summary>
        /// Filter XElement collection on children predicate. 
        /// </summary>
        public static IEnumerable<XElement> WhereChild(this IEnumerable<XElement> source, string childName, Predicate<string> childValueSelector)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            childValueSelector = childValueSelector ?? throw new ArgumentNullException(nameof(childValueSelector));
            if (string.IsNullOrWhiteSpace(childName))
                throw new ArgumentNullException(nameof(childName));

            return source
                    .Where(i => childValueSelector(i.Element(childName).Value))
                    .ToArray();
        }
    }
}