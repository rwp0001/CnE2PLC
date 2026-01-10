using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CnE2PLC.Helpers;

public static class XMLHelper
{
    public static XmlNode CreateGenericXmlNode()
    {
        XmlDocument document = new XmlDocument();
        return document.CreateNode(XmlNodeType.Element, "Generic", "");
    }

    public static string GetNamedAttributeItemValue(this XmlNode node, string name)
    {
        return node.Attributes?.GetNamedItem(name)?.Value ?? String.Empty;
    }

    public static string GetNamedAttributeItemInnerText(this XmlNode node, string name)
    {
        return node.Attributes?.GetNamedItem(name)?.InnerText ?? String.Empty;
    }

    public static int? GetNamedAttributeItemInnerTextAsInt(this XmlNode node, string name)
    {
        return int.TryParse(node.GetNamedAttributeItemInnerText(name), out var value) ? value : null;
    }

    public static bool? GetNamedAttributeItemInnerTextAsBool(this XmlNode node, string name)
    {
        string s = node.GetNamedAttributeItemInnerText(name);
        return s.ToLower() switch
        {
            "true" => true,
            "false" => false,
            _ => null,
        };

    }


    public static DateTime? GetNameAttributeItemInnerTextAsDateTime(this XmlNode node, string name)
    {
        string input = node.GetNamedAttributeItemInnerText(name);

        if(input.Contains('-'))
        {
            // 2015-03-05T15:24:52.183Z
            var sp = input.Split('T');
            var d = sp[0].Split('-');
            input = $"{d[1]}/{d[2]}/{d[0]} {sp[1].Substring(0,8)}";

        } else
        {
            // Sat May 11 10:43:06 2024
            var sp = input.Split(' ');
            input = $"{sp[1]} {sp[2]}, {sp[4]} {sp[3]}";
        }

        return DateTime.TryParse(input, out DateTime dt) ? dt : null;
    }

    public static XmlNode SelectSingleNode(this XmlNode node, string name, XmlNode defaultIfNull)
    {
        return node.SelectSingleNode(name) ?? defaultIfNull;
    }

    public static string GetChildNodeInnerText(this XmlNode node, string name, bool useLast)
    {
        XmlNode? child = useLast ? node.ChildNodes.Cast<XmlNode>().LastOrDefault(n => n?.Name == name, null) : node.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => n?.Name == name, null);
        return child == null ? String.Empty : child.InnerText;
    }

    public static bool AttributeExists(this XmlNode node, string name)
    {
        return node.Attributes?.GetNamedItem(name) != null;
    }

    public static bool AttributeExists(this XmlNode node, string name, out XmlNode attribute)
    {
        XmlNode? temp = node.Attributes?.GetNamedItem(name);
        if (temp != null)
        {
            attribute = temp;
            return true;
        }
        attribute = CreateGenericXmlNode();
        return false;
        
    }
}
