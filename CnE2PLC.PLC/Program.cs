using System.Diagnostics;
using System.Xml;
using CnE2PLC.Helpers;
using CnE2PLC.PLC.XTO;

namespace CnE2PLC.PLC;

public class Program
{
    public Program() { }

    public Program(XmlNode node) 
    {
        try
        {
            Name = node.GetNamedAttributeItemValue("Name");
            TestEdits = node.GetNamedAttributeItemValue("TestEdits");
            Disabled = node.GetNamedAttributeItemValue("Disabled");
            UseAsFolder = node.GetNamedAttributeItemValue("UseAsFolder");

            // optional
            MainRoutineName = node.GetNamedAttributeItemValue("MainRoutineName");

            LocalTags = new();
            Routines = new();


            foreach (XmlNode node2 in node.ChildNodes)
            {
                switch (node2.Name)
                {
                    case "Tags":
                        foreach (XTO_AOI tag in Controller.ProcessTags(node2))
                        {
                            tag.Path = Name;
                            LocalTags.Add(tag);
                        }
                        break;

                    case "Routines":
                        if (node2 != null)
                        {
                            foreach (XmlNode Routine in node2.ChildNodes)
                            {
                                string type = Routine.GetNamedAttributeItemValue("Type");
                                switch (type)
                                {
                                    case "RLL":
                                        Routines.Add(new RLL_Routine(Routine));
                                        break;

                                    case "ST":
                                        Routines.Add(new ST_Routine(Routine));
                                        break;

                                    case "FBD":
                                        Routines.Add(new FBD_Routine(Routine));
                                        break;

                                    case "SFC":
                                        Routines.Add(new SFC_Routine(Routine));
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Program Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }


    }



    #region Public Properties
    public string? Name {  get; set; }
    public string? TestEdits { get; set; }
    public string? MainRoutineName { get; set; }
    public string? Disabled { get; set; }
    public string? UseAsFolder { get; set; }
    public List<Routine> Routines { get; set; } = new();
    public List<PLCTag> LocalTags { get; set; } = new();

    #endregion

    public override string ToString() { return $"{Name} Routines: {Routines.Count} Tags: {LocalTags.Count}"; }

    public int TagCount(string tag)
    {
        int r = 0;
        foreach (Routine routine in Routines) r += routine.TagCount(tag);
        return r;
    }

    public int AOICount(string type,string tag)
    {
        int r = 0;
        foreach (Routine routine in Routines) r += routine.AOICount(type,tag);
        return r;
    }

    public List<string> GetIO(string type,string tag)
    {
        List<string> r = new();
        foreach (Routine routine in Routines) foreach (string s in routine.GetIO(type, tag)) if(!r.Contains(s)) r.Add(s);
        return r;
    }


}

public class Routine
{
    public Routine() { }

    public Routine(XmlNode node)
    {
        try
        {
            Name = node.GetNamedAttributeItemValue("Name");
            Type = node.GetNamedAttributeItemValue("Type");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Routine Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }
    }

    #region Public Properties
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    #endregion

    public override string ToString() { return $"Name: {Name} Type: {Type}"; }

    /// <summary>
    /// Used to find the number of times a tag is used in the program.
    /// </summary>
    /// <param name="tag">Tag name to count.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual int TagCount(string tag) { throw new NotImplementedException(); }

    public virtual int AOICount(string type, string tag) { throw new NotImplementedException(); }

    public virtual List<string> GetIO(string type, string tag) {
        return new List<string>();
        //throw new NotImplementedException(); 
    }

    public virtual string ToText() { throw new NotImplementedException(); }

}