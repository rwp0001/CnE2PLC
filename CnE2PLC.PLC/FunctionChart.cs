using CnE2PLC.Helpers;
using System.Diagnostics;
using System.Xml;

namespace CnE2PLC.PLC;

// SFC classes
public class SFC_Routine : Routine
{
    public SFC_Routine() { }
    public SFC_Routine(XmlNode node) : base(node) 
    {
    }

    public string? Description { get; set; }

    public List<ChartElement> Elements { get; set; } = new();

    public override int TagCount(string tag)
    {
        int r = 0;
        foreach (ChartElement element in Elements) r += element.TagCount(tag);
        return r;
    }

    public override int AOICount(string type,string tag)
    {
        int r = 0;
        foreach (ChartElement element in Elements) r += element.AOICount(type,tag);
        return r;
    }

    public override string ToText()
    {
        string s = string.Empty;
        foreach (ChartElement element in Elements) s += element.ToString();
        return s;
    }

    public override string ToString() { return $"Chart Name:{Name} Elements: {Elements.Count} Desc: {Description}"; }
}
public class ChartElement
{
    public ChartElement() { }
    public ChartElement(XmlNode node)
    {
        try
        {
            string? s = node.Attributes?.GetNamedItem("ID")?.Value;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                ID = n;
            }
            s = node.Attributes?.GetNamedItem("X")?.Value;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                X = n;
            }

            s = node.Attributes?.GetNamedItem("Y")?.Value;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                Y = n;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Sheet Element Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }
    public int? ID { get; set; } = 0;
    public int? X { get; set; } = 0;
    public int? Y { get; set; } = 0;

    public virtual int TagCount(string tag) { throw new NotImplementedException(); }
    public virtual int AOICount(string type, string tag) { throw new NotImplementedException(); }

    public override string ToString() { return $"ID: {ID} at  X:{X}, Y:{Y}"; }
}


public class Step : ChartElement 
{ 
    public Step() { }
    public Step(XmlNode node) : base(node)
    {
        try
        {
            Operand = node.Attributes?.GetNamedItem("Operand")?.Value;

            string s = node.Attributes?.GetNamedItem("HideDesc")?.Value ?? String.Empty;
            HideDesc = s[0] == 'f' ? false : true;


            s = node.Attributes?.GetNamedItem("DescX")?.Value ?? String.Empty;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                DescX = n;
            }

            s = node.Attributes?.GetNamedItem("DescY")?.Value ?? String.Empty;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                DescY = n;
            }

            s = node.Attributes?.GetNamedItem("DescWidth")?.Value ?? String.Empty;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                DescWidth = n;
            }

            s = node.Attributes?.GetNamedItem("InitialStep")?.Value ?? String.Empty;
            InitialStep = s[0] == 'f' ? false : true;

            s = node.Attributes?.GetNamedItem("PresetUsesExpr")?.Value ?? String.Empty;
            PresetUsesExpr = s[0] == 'f' ? false : true;

            s = node.Attributes?.GetNamedItem("LimitHighUsesExpr")?.Value ?? String.Empty;
            LimitHighUsesExpr = s[0] == 'f' ? false : true;

            s = node.Attributes?.GetNamedItem("LimitLowUsesExpr")?.Value ?? String.Empty;
            LimitLowUsesExpr = s[0] == 'f' ? false : true;

            s = node.Attributes?.GetNamedItem("ShowActions")?.Value ?? String.Empty;
            ShowActions = s[0] == 'f' ? false : true;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Step Element Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }
    }

    public string? Operand { get; set; }
    public bool? HideDesc { get; set; }
    public int? DescX { get; set; }
    public int? DescY { get; set; }
    public int? DescWidth { get; set; }
    public bool? InitialStep { get; set; }
    public bool? PresetUsesExpr { get; set; }
    public bool? LimitHighUsesExpr { get; set; }
    public bool? LimitLowUsesExpr { get; set; }
    public bool? ShowActions { get; set; }

    public override int TagCount(string tag) { throw new NotImplementedException(); }
    public override int AOICount(string type, string tag) { throw new NotImplementedException(); }

}
public class Transition : ChartElement 
{
    public Transition() { }
    public Transition(XmlNode node) : base(node) {
        try
        {
            Operand = node.Attributes?.GetNamedItem("Operand")?.Value ?? String.Empty;

            string s = node.Attributes?.GetNamedItem("HideDesc")?.Value ?? String.Empty;
            HideDesc = s[0] == 'f' ? false : true;


            s = node.Attributes?.GetNamedItem("DescX")?.Value ?? String.Empty;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                DescX = n;
            }

            s = node.Attributes?.GetNamedItem("DescY")?.Value ?? String.Empty;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                DescY = n;
            }

            s = node.Attributes?.GetNamedItem("DescWidth")?.Value ?? String.Empty;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                DescWidth = n;
            }

            XmlNode? Content = node.SelectSingleNode("Condition");

            if (Content != null)
            {
                XmlNodeList? STContent = Content.SelectNodes("STContent");
                if (STContent != null)
                {
                    foreach (XmlNode node2 in STContent)
                    {
                        foreach (XmlNode line in node2.ChildNodes) Conditions.Add(new Line(line));
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Step Element Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }
    }
    public string? Operand { get; set; }
    public bool? HideDesc { get; set; }
    public int? DescX { get; set; }
    public int? DescY { get; set; }
    public int? DescWidth { get; set; }
    public List<Line> Conditions { get; set; } = new();
    
    public override int TagCount(string tag) 
    {
        int count = 0;
        foreach (Line line in Conditions) count += line.TagCount(tag);
        return count;
    }

    public override int AOICount(string type, string tag) { return 0; }

}


public class Branch : ChartElement { }
public class DirectedLink : ChartElement { }

public class SFC_TextBox : ChartElement
{
    public SFC_TextBox() { }
    public SFC_TextBox(XmlNode node) : base(node)
    {
        try
        {
            string? s;

            s = node.Attributes?.GetNamedItem("Width")?.Value ?? String.Empty;
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                Width = n;
            }

            Text = node.Attributes?.GetNamedItem("Text")?.Value ?? String.Empty;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Textbox Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public int? Width { get; set; }
    public string? Text { get; set; }

    public override int TagCount(string tag) { return 0; }
    public override int AOICount(string type, string tag) { return 0; }

    public override string ToString() { return $"{base.ToString()} - Text: {Text}"; }

}
