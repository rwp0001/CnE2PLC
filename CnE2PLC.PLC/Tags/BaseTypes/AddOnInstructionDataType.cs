using System.Xml;
using CnE2PLC.Helpers;

namespace CnE2PLC.PLC;

public class AddOnInstructionDataType : PLCTag
{
    public AddOnInstructionDataType() { }
    public AddOnInstructionDataType(XmlNode node) : base(node) { }

    /// <summary>
    /// all AOIs have this bit.
    /// </summary>
    public bool EnableIn { get; set; }

    /// <summary>
    /// all AOIs have this bit.
    /// </summary>
    public bool EnableOut { get; set; }


    public override void Set()
    {
        base.Set();
    }
    public override void Get()
    {
        base.Get();
    }
}


public class AddOnInstructionDefinition
{
    public AddOnInstructionDefinition() { }
    public AddOnInstructionDefinition(XmlNode node) 
    {
        Name = node.GetNamedAttributeItemInnerText("Name");
        Revision = node.GetNamedAttributeItemInnerText("Revision");
        Vendor = node.GetNamedAttributeItemInnerText("Vendor");
        ExecutePrescan = bool.TryParse(node.GetNamedAttributeItemInnerText("ExecutePrescan"), out bool prescan) ? prescan : false;
        ExecutePostscan = bool.TryParse(node.GetNamedAttributeItemInnerText("ExecutePostscan"), out bool postscan) ? postscan : false;
        ExecuteEnableInFalse = bool.TryParse(node.GetNamedAttributeItemInnerText("ExecuteEnableInFalse"), out bool enableInFalse) ? enableInFalse : false;
        CreatedDate = DateTime.TryParse(node.GetNamedAttributeItemInnerText("CreatedDate"), out DateTime createdDate) ? createdDate : DateTime.MinValue;
        CreatedBy = node.GetNamedAttributeItemInnerText("CreatedBy");
        EditedDate = DateTime.TryParse(node.GetNamedAttributeItemInnerText("EditedDate"), out DateTime editedDate) ? editedDate : DateTime.MinValue;
        EditedBy = node.GetNamedAttributeItemInnerText("EditedBy");
        SoftwareRevision = node.GetNamedAttributeItemInnerText("SoftwareRevision");

        var descNode = node.SelectSingleNode("Description");
        Description = descNode?.InnerText ?? string.Empty;

        var revNode = node.SelectSingleNode("RevisionNote");
        RevisionNote = revNode?.InnerText ?? string.Empty;

        foreach (XmlNode child in node.ChildNodes)
        {
            if (child.Name == "LocalTags")
            {
                foreach (XmlNode tagNode in child.ChildNodes)
                {
                    LocalTags.Add(new LocalTag(tagNode));
                }
            }
            else if (child.Name == "Parameters")
            {
                foreach (XmlNode paramNode in child.ChildNodes)
                {
                    Parameters.Add(new Parameter(paramNode));
                }
            }
            else if (child.Name == "Routines")
            {
                foreach (XmlNode routineNode in child.ChildNodes)
                {
                    Routine newRoutine = new Routine(routineNode);
                    switch (newRoutine.Type)
                    {
                        case "RLL":
                            Routines.Add(new RLL_Routine(routineNode));
                            break;

                        case "FBD":
                            Routines.Add(new FBD_Routine(routineNode));
                            break;

                        case "ST":
                            Routines.Add(new ST_Routine(routineNode));
                            break;

                        default:
                            Routines.Add(new Routine(routineNode));
                            LogHelper.DebugPrint($"WARNING: AddOnInstructionDefinition: {Name} Routine {newRoutine.Name} type {newRoutine.Type} is not recognized. Added as base Routine type.");
                            break;
                    }
                }
            }
        }
    }


    #region Parameters
    public string Name { get; set; } = string.Empty;
    public string Revision { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
    public bool ExecutePrescan { get; set; } = false;
    public bool ExecutePostscan { get; set; } = false;
    public bool ExecuteEnableInFalse { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.MinValue;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime EditedDate { get; set; } = DateTime.MinValue;
    public string EditedBy { get; set; } = string.Empty;
    public string SoftwareRevision { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RevisionNote { get; set; } = string.Empty;
    public List<LocalTag> LocalTags { get; set; } = new List<LocalTag>();
    public List<Parameter> Parameters { get; set; } = new List<Parameter>();
    public List<Routine> Routines { get; set; } = new List<Routine>();
    #endregion

    public override string ToString()
    {
        return $"Name: {Name} Revision: {Revision} Vendor: {Vendor} Description: {Description})";
    }

}

public class Parameter
{
    public Parameter() { }

    public Parameter(XmlNode node) 
    {
        Name = node.GetNamedAttributeItemInnerText("Name");
        TagType = node.GetNamedAttributeItemInnerText("TagType");
        DataType = node.GetNamedAttributeItemInnerText("DataType");
        Usage = Enum.TryParse(node.GetNamedAttributeItemInnerText("Usage"), out Usages usage) ? usage : Usages.Local;
        Radix = Enum.TryParse(node.GetNamedAttributeItemInnerText("Radix"), out Radixes radix) ? radix : Radixes.Decimal;
        Required = bool.TryParse(node.GetNamedAttributeItemInnerText("Required"), out bool req) ? req : false;
        Visable = bool.TryParse(node.GetNamedAttributeItemInnerText("Visable"), out bool vis) ? vis : false;
        ExternalAccess = Enum.TryParse(node.GetNamedAttributeItemInnerText("ExternalAccess"), out Accesses access) ? access : Accesses.ReadWrite;
        Description = node.GetNamedAttributeItemInnerText("Description");
    }

    #region Parameters
    public string Name { get; set; } = string.Empty;
    public string TagType { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public Usages Usage { get; set; } = Usages.Local;
    public Radixes Radix { get; set; } = Radixes.Decimal;
    public bool Required { get; set; } = false;
    public bool Visable { get; set; } = false;
    public Accesses ExternalAccess { get; set; } = Accesses.ReadWrite;
    public string Description { get; set; } = string.Empty;
    #endregion

    public override string ToString()
    {
        return $"Name: {Name} DataType: {DataType} Usage: {Usage} Description: {Description}";
    }
}


public class LocalTag
{
    public LocalTag() { }
    public LocalTag(XmlNode node) 
    { 
        Name = node.GetNamedAttributeItemInnerText("Name");
        DataType = node.GetNamedAttributeItemInnerText("DataType");
        ExternalAccess = Enum.TryParse(node.GetNamedAttributeItemInnerText("ExternalAccess"), out Accesses access) ? access : Accesses.ReadWrite;
        Description = node.GetNamedAttributeItemInnerText("Description");
    }

    #region Parameters
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public Accesses ExternalAccess { get; set; } = Accesses.ReadWrite;
    public string Description { get; set; } = string.Empty;
    #endregion

    public override string ToString()
    {
        return $"Name: {Name} TagType: {DataType} Description: {Description}";
    }

}
