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
            DateTime Start = DateTime.Now;

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

            DateTime End = DateTime.Now;
            LogHelper.DebugPrint($"INFO: Program created. {ToString()} Time {(End - Start).TotalMilliseconds} ms.");

        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Program: Import of node {node.Name} failed with {ex.Message}");
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

    public int RefCount(string tag)
    {
        int r = 0;
        foreach (Routine routine in Routines) r += routine.RefCount(tag);
        return r;
    }

    public List<string> GetIO(string tag)
    {
        List<string> r = new();
        foreach (Routine routine in Routines)
        {
            foreach (string s in routine.GetIO(tag))
            {
                if (!r.Contains(s)) r.Add(s);
            }
        }
        return r;
    }


}
