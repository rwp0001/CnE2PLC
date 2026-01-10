using CnE2PLC.Helpers;
using System.Xml;

namespace CnE2PLC.PLC;

public class Module
{
    public Module() { }

    public Module(XmlNode node)
    {
        try
        {
            Name = node.GetNamedAttributeItemInnerText("Name");
            CatalogNumber = node.GetNamedAttributeItemInnerText("CatalogNumber");
            Description = node.GetNamedAttributeItemInnerText("Description");
            ParentModule = node.GetNamedAttributeItemInnerText("ParentModule");
            
            Vendor = node.GetNamedAttributeItemInnerTextAsInt("Vendor") ?? 0;
            ProductType = node.GetNamedAttributeItemInnerTextAsInt("ProductType") ?? 0;
            ProductCode = node.GetNamedAttributeItemInnerTextAsInt("ProductCode") ?? 0;
            Major = node.GetNamedAttributeItemInnerTextAsInt("Major") ?? 0;
            Minor = node.GetNamedAttributeItemInnerTextAsInt("Minor") ?? 0;
            ParentModPortId = node.GetNamedAttributeItemInnerTextAsInt("ParentModPortId") ?? 0;

            Inhibited = node.GetNamedAttributeItemInnerTextAsBool("Inhibited") ?? false;
            MajorFault = node.GetNamedAttributeItemInnerTextAsBool("MajorFault") ?? false;

            foreach (XmlNode portNode in node.SelectSingleNode("Ports").ChildNodes)
            {
                Ports.Add(new Port(portNode));
            }


            LogHelper.DebugPrint($"INFO: Created Module: {ToString()}");

        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Module: Import node {node.Name} failed with {ex.Message}");
        }
    }

    #region Parameters

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CatalogNumber { get; set; } = string.Empty;
    public int Vendor { get; set; } = 0;
    public int ProductType { get; set; } = 0;
    public int ProductCode { get; set; } = 0;
    public int Major { get; set; } = 0;
    public int Minor { get; set; } = 0;
    public string ParentModule { get; set; } = string.Empty;
    public int ParentModPortId { get; set; } = 0;
    public bool Inhibited { get; set; } = false;
    public bool MajorFault { get; set; } = false;
    public EKey EKey { get; set; } = new EKey();
    public List<Port> Ports { get; set; } = new List<Port>();

    #endregion

    public override string ToString()
    {
        return $"Name: {Name}, CatalogNumber: {CatalogNumber}";
    }



}


public class EKey {
    public EKeyStates State { get; set; } = EKeyStates.CompatibleModule;


}


public class Port {

    public Port() { }
    public Port(XmlNode node)
    {
        try
        {
            Id = node.GetNamedAttributeItemInnerTextAsInt("Id") ?? 0;
            Address = node.GetNamedAttributeItemInnerText("Address");
            Upstream = node.GetNamedAttributeItemInnerTextAsBool("Upstream") ?? false;

            string s = node.GetNamedAttributeItemInnerText("Type");
            Type = Enum.TryParse<PortTypes>(s, out PortTypes result) ? result : PortTypes.None;

            LogHelper.DebugPrint($"INFO: Created Port: {ToString()}");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Port: Import node {node.Name} failed with {ex.Message}");
        }
    }

    #region Parameters

    public int Id { get; set; } = 0;
    public string Address { get; set; } = string.Empty;
    public PortTypes Type { get; set; } = PortTypes.None;
    public bool Upstream { get; set; } = false;
    public Bus Bus { get; set; } = new Bus();

    #endregion

    public override string ToString()
    {
        return $"Id: {Id}, Type: {Type}, Address: {Address}";
    }


}

public class Bus {
    public int Size { get; set; } = 0;
}   
