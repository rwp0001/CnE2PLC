using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using CnE2PLC.Helpers;

namespace CnE2PLC.PLC;


// Stuctured Text classes
public class ST_Routine : Routine
{
    ST_Routine() { }

    public ST_Routine(XmlNode node) : base(node)
    {
        try
        {
            XmlNodeList? STContent = node.SelectNodes("STContent");
            if (STContent != null)
            {
                foreach (XmlNode node2 in STContent)
                {
                    foreach (XmlNode line in node2.ChildNodes) Lines.Add(new Line(line));
                }
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"Import ST Routine Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }
    }

    public override int TagCount(string tag)
    {
        int count = 0;
        foreach (Line line in Lines) count += line.TagCount(tag);
        return count;
    }

    public override int AOICount(string type, string tag)
    {
        int count = 0;
        foreach (Line line in Lines) count += line.AOICount(type,tag);
        return count;
    }

    public List<Line> Lines { get; set; } = new();

    public override string ToText()
    {
        string s = string.Empty;
        foreach (Line line in Lines) s += $"{line.Text}\n";
        return s;
    }

    public override string ToString() { return $"Name:{Name} Rungs: {Lines.Count}"; }

}
public class Line
{
    public Line() { }
    public Line(XmlNode node)
    {
        try
        {
            string s = node.GetNamedAttributeItemValue("Number");
            Number = int.TryParse(s, out int n) ? n : 0;
            Text = node.GetChildNodeInnerText("Text", true);
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"Import Rung Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public int Number { get; set; } = -1;

    public string Text { get; set; } = string.Empty;

    public int TagCount(string tag) { return Regex.Matches(Text, Regex.Escape(tag)).Count; }

    public int AOICount(string type, string tag) {
        try
        {
            string s = $"{type}({tag}";
            if (!Text.Contains(s)) return 0;
            int count = 0;
            string[] rs = Text.Split(s);
            foreach (string r in rs)
            {
                if (r.Length == 0) continue;
                if (r[0] == ')' || r[0] == ',') count += 1;
            }
            return count;
        }
        catch (Exception ex )
        {
            LogHelper.DebugPrint($"AOICount Error: {ex.Message}");
            throw;
        }
        
    }

    public override string ToString() { return $"{Number}\t{Text}"; }

}
