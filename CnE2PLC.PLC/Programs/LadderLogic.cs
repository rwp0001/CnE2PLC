using CnE2PLC.Helpers;
using System.Text.RegularExpressions;
using System.Xml;

namespace CnE2PLC.PLC;


/// <summary>
/// Ladder Logic Classes
/// </summary>
internal class RLL_Routine : Routine
{

    /// <summary>
    /// Rockwell Ladder Logic
    /// </summary>
    /// <param name="node"></param>
    public RLL_Routine(XmlNode node) : base(node)
    {
        try
        {
            DateTime Start = DateTime.Now;
            Name = node.GetNamedAttributeItemValue("Name");
            Type = node.GetNamedAttributeItemValue("Type");
            XmlNodeList? RLLContent = node.SelectNodes("RLLContent");
            if (RLLContent != null)
            {
                foreach (XmlNode node2 in RLLContent)
                {
                    foreach (XmlNode rung in node2.ChildNodes) Rungs.Add(new Rung(rung));
                }
            }
            DateTime End = DateTime.Now;
            LogHelper.DebugPrint($"INFO: RLL_Routine {ToString()} Time {(End-Start).TotalMilliseconds} ms.");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: RLL_Routine: Import node {node.Name} failed with {ex.Message}");
        }


    }

    public List<Rung> Rungs { get; set; } = new();

    public override int RefCount(string tag)
    {
        int count = 0;
        foreach (Rung rung in Rungs)
        {
            if (!rung.Text.Contains(tag)) continue;
            count += Regex.Matches(rung.Text, Regex.Escape(tag)).Count;
        }
        return count;
    }

    public override List<string> GetIO(string tag)
    {
        List<string> r = new();
        foreach (Rung rung in Rungs)
        {
            r.AddRange(rung.GetIO(tag));
        }
        return r;
    }


    public override string ToText()
    {
        string s = string.Empty;
        foreach (Rung rung in Rungs) s += $"{rung.Text}\n";
        return s;
    }

    public override string ToString() { return $"Name: {Name} Rungs: {Rungs.Count}"; }
}

/// <summary>
/// Rung Class
/// </summary>
internal class Rung
{
    public Rung() { }
    public Rung(XmlNode node)
    {
        try
        {
            string s = node.GetNamedAttributeItemValue("Number");
            int n;
            int.TryParse(s, out n);
            Number = n;
            Type = node.GetNamedAttributeItemValue("Type");

            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Name == "Text") Text = node2.InnerText;
                if (node2.Name == "Comment") Comment = node2.InnerText;
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Rung: Import node {node.Name} failed with {ex.Message}");
        }

    }

    #region Public Properties
    public int Number { get; set; } = -1;
    public string Type { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    #endregion

    public int RefCount(string tag) 
    { 
        return Regex.Matches(Text, Regex.Escape(tag)).Count; 
    }



    public List<string> GetIO(string tag)
    {
        if (!Text.Contains(tag)) return new List<string>();

        List<string> r = new();
        string[] rs = Text.Split(tag);

        foreach (string s in rs)
        {
            if (s.Length == 0) continue;
            if (s[0] == ')') continue;
            if ( s[0] == ',') {
                string[] s2 = s.Split(')');
                string[] s3 = s2[0].Split(',');
                r.Add(s3[1]);
            }
        }
        return r;
        


    }

    public override string ToString() { return $"{Number}\t{Text}"; }

}
