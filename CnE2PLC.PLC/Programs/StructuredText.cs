using System.Text.RegularExpressions;
using System.Xml;
using CnE2PLC.Helpers;

namespace CnE2PLC.PLC;


// Stuctured Text classes
public class ST_Routine : Routine
{

    public ST_Routine(XmlNode node) : base(node)
    {
        try
        {
            DateTime Start = DateTime.Now;
            XmlNodeList? STContent = node.SelectNodes("STContent");
            if (STContent != null)
            {
                foreach (XmlNode node2 in STContent)
                {
                    foreach (XmlNode line in node2.ChildNodes) Lines.Add(new Line(line));
                }
            }
            DateTime End = DateTime.Now;
            LogHelper.DebugPrint($"INFO: ST_Routine {ToString()} Time {(End - Start).TotalMilliseconds} ms.");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: ST_Routine: Import node {node.Name} failed with {ex.Message}");
        }
    }

    public override int RefCount(string tag)
    {
        int count = 0;
        foreach (Line line in Lines) count += line.RefCount(tag);
        return count;
    }

    public List<Line> Lines { get; set; } = new();

    public override string ToText()
    {
        string s = string.Empty;
        foreach (Line line in Lines) s += $"{line.Text}\n";
        return s;
    }

    public override string ToString() { return $"Name: {Name} Rungs: {Lines.Count}"; }

}
public class Line
{
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
            LogHelper.DebugPrint($"ERROR: Line: Import node {node.Name} Failed with {ex.Message}");
        }

    }

    public int Number { get; set; } = -1;

    public string Text { get; set; } = string.Empty;

    public int RefCount(string tag) 
    { 
        return Regex.Matches(Text, Regex.Escape(tag)).Count; 
    }

    public override string ToString() { return $"{Number}\t{Text}"; }

}
