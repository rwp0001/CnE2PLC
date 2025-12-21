using CnE2PLC.Helpers;
using System.Diagnostics;
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
    public RLL_Routine(XmlNode node)
    {
        try
        {
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
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"Import LL Routine Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }


    }

    public List<Rung> Rungs { get; set; } = new();

    public override int TagCount(string tag)
    {
        int count = 0;
        foreach (Rung rung in Rungs)
        {
            if (!rung.Text.Contains(tag)) continue;
            count += Regex.Matches(rung.Text, Regex.Escape(tag)).Count;
        }
        return count;
    }

    public override int AOICount(string type, string tag)
    {
        int count = 0;
        foreach (Rung rung in Rungs) count += rung.AOICount(type, tag);
        return count;
    }

    public override List<string> GetIO(string type, string tag)
    {
        List<string> r = new();
        foreach (Rung rung in Rungs) r.AddRange(rung.GetIO(type, tag));
        return r;
    }


    public override string ToText()
    {
        string s = string.Empty;
        foreach (Rung rung in Rungs) s += $"{rung.Text}\n";
        return s;
    }

    public override string ToString() { return $"Name:{Name} Rungs: {Rungs.Count}"; }
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
            LogHelper.DebugPrint($"Import Rung Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    #region Public Properties
    public int Number { get; set; } = -1;
    public string Type { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    #endregion

    public int TagCount(string tag) { return Regex.Matches(Text, Regex.Escape(tag)).Count; }

    public int AOICount(string type, string tag)
    {
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
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"AOICount Error: {ex.Message}");
            throw;
        }

    }

    public List<string> GetIO(string type, string tag)
    {
        List<string> r = new();
        try
        {
            if (AOICount(type, tag) == 0) return r;

            string ts = $"{type}({tag}";
            string[] rs = Text.Split(ts);

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
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"GetIO Error: {ex.Message}");
            throw;
        }

    }

    public override string ToString() { return $"{Number}\t{Text}"; }

}
