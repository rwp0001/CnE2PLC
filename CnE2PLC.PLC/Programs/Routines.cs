using CnE2PLC.Helpers;
using System.Xml;

namespace CnE2PLC.PLC;

public class Routine
{

    public Routine(XmlNode node)
    {
        try
        {
            Name = node.GetNamedAttributeItemValue("Name");
            Type = node.GetNamedAttributeItemValue("Type");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Routine: Import of node {node.Name} failed with {ex.Message}.");
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
    public virtual int RefCount(string tag) 
    { 
        throw new NotImplementedException(); 
    }

    public virtual List<string> GetIO(string tag)
    {
        throw new NotImplementedException(); 
    }

    public virtual string ToText() 
    { 
        throw new NotImplementedException(); 
    }

}