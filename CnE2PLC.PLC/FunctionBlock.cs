using CnE2PLC.Helpers;
using System.Xml;
using System.Xml.Linq;

namespace CnE2PLC.PLC;



// FBD Classes
public class FBD_Routine : Routine
{
    public FBD_Routine(XmlNode node) : base(node)
    {
        try
        {
            DateTime Start = DateTime.Now;
            XmlNodeList? FBDContent = node.SelectNodes("FBDContent");
            if (FBDContent != null)
            {
                foreach (XmlNode node2 in FBDContent)
                {
                    foreach (XmlNode sheet in node2.ChildNodes) Sheets.Add(new Sheet(sheet));
                }
            }
            DateTime End = DateTime.Now;
            LogHelper.DebugPrint($"INFO: FBD_Routine {ToString()} Time {(End - Start).TotalMilliseconds} ms.");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: FBD_Routine: Import node {node.Name} failed with {ex.Message}");
        }
    }

    public List<Sheet> Sheets { get; set; } = new();

    public override int RefCount(string tag)
    {
        int r = 0;
        foreach (Sheet sheet in Sheets) r += sheet.RefCount(tag);
        return r;
    }

    public override List<string> GetIO(string tag)
    {
        List<string> r = new();
        foreach (Sheet sheet in Sheets)
        {
            r.AddRange(sheet.GetIO(tag));
        }
        return r;
    }

    public override string ToText()
    {
        string s = string.Empty;
        foreach (Sheet sheet in Sheets) s += sheet.ToString();
        return s;
    }

    public override string ToString() { return $"Name: {Name} Sheets: {Sheets.Count}"; }


}

public class Sheet
{

    public Sheet(XmlNode node)
    {
        try
        {

            //Description = node.Attributes.GetNamedItem("Description").Value;
            string s = node.GetNamedAttributeItemValue("Number");
            if (s != null)
            {
                int n;
                int.TryParse(s, out n);
                Number = n;
            }

            foreach (XmlNode element in node.ChildNodes)
            {
                switch (element.Name)
                {
                    case "IRef":
                        Elements.Add(new IRef(element));
                        break;

                    case "ORef":
                        Elements.Add(new ORef(element));
                        break;

                    case "Block":
                        Elements.Add(new Block(element));
                        break;

                    case "Wire":
                        Elements.Add(new Wire(element));
                        break;

                    case "AddOnInstruction":
                        Elements.Add(new AddOnInstruction(element));
                        break;

                    case "TextBox":
                        Elements.Add(new FBD_TextBox(element));
                        break;

                    case "Attachment":
                        Elements.Add(new Attachment(element));
                        break;

                    default:
                        break;
                }

            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Sheet: Import node {node.Name} failed with {ex.Message}");
        }

    }

    public int Number { get; set; } = -1;   
    public string Description { get; set; } = string.Empty;

    public List<SheetElement> Elements { get; set; } = new();

    public string Name { get; set; } = string.Empty;



    public int RefCount(string tag)
    {
        int r = 0;
        if (tag.Contains('(')) // aoi count
        {
            string[] s = tag.Split('(');
            foreach (SheetElement element in Elements)
            {
                if (element is not AddOnInstruction) continue;
                if ( s[0] == ((AddOnInstruction)element).Operand & s[1] == ((AddOnInstruction)element).Name ) r++;
            }
        } 
        else // ref count
        {
            foreach (SheetElement element in Elements)
            {
                r += element.RefCount(tag);
            }
        }
        return r;
    }

    public List<string> GetIO(string tag)
    {


        List<string> r = new();
        foreach (SheetElement element in Elements)
        {
            //r.AddRange(element.GetIO(tag));
        }
        return r;
    }

    public override string ToString() { return $"Sheet Number:{Number} Element: {Elements.Count} Desc: {Description}"; }

}

public class SheetElement
{
    public SheetElement() { }

    public SheetElement(XmlNode node)
    {
        try
        {
            string s = node.GetNamedAttributeItemValue("ID");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                ID = n;
            }
            s = node.GetNamedAttributeItemValue("X");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                X = n;
            }

            s = node.GetNamedAttributeItemValue("Y");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                Y = n;
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: SheetElement: Import node {node.Name} failed with {ex.Message}");
        }

    }

    #region Public Propreties

    public int ID { get; set; } = 0;
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;

    #endregion

    public virtual int RefCount(string tag) 
    { 
        throw new NotImplementedException(); 
    }

    public override string ToString() { return $"ID: {ID} at  X:{X}, Y:{Y}"; }

}

public class IRef : SheetElement
{
    public IRef(XmlNode node) : base(node) 
    {
        try
        {
            Operand = node.GetNamedAttributeItemValue("Operand");
            string s = node.GetNamedAttributeItemValue("HideDesc");
            HideDesc = s[0] == 'f' ? false : true;
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: IRef: Import node {node.Name} failed with {ex.Message}");
        }

    }



    public string Operand { get; set; } = string.Empty;
    public bool HideDesc { get; set; } = false;



    public override int RefCount(string tag) 
    { 
        return Operand.Contains(tag) ? 1 : 0; 
    }

    public override string ToString() { return $"{base.ToString()} - Operand: {Operand}"; }
}

public class ORef : SheetElement
{

    public ORef(XmlNode node) : base(node)
    {
        try
        {
            Operand = node.GetNamedAttributeItemValue("Operand");
            string s = node.GetNamedAttributeItemValue("HideDesc");
            HideDesc = s[0] == 'f' ? false : true;
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: ORef: Import node {node.Name} failed with {ex.Message}");
        }

    }



    public string Operand { get; set; } = string.Empty ;
    public bool HideDesc { get; set; } = false;



    public override int RefCount(string tag)
    {
        return Operand.Contains(tag) ? 1 : 0;
    }

    public override string ToString() { return $"{base.ToString()} - Operand: {Operand}"; }
}

public class Block : SheetElement 
{
    public Block(XmlNode node) : base(node)
    {
        try
        {
            Type = node.GetNamedAttributeItemValue("Type");
            Operand = node.GetNamedAttributeItemValue("Operand");
            VisiblePins = node.GetNamedAttributeItemValue("VisiblePins");
            string s = node.GetNamedAttributeItemValue("HideDesc");
            HideDesc = s[0] == 'f' ? false : true;
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Block: Import node {node.Name} failed with {ex.Message}");
        }

    }



    public string Type { get; set; } = string.Empty;
    public string Operand { get; set; } = string.Empty;
    public string VisiblePins { get; set; } = string.Empty;
    public bool HideDesc { get; set; } = false;



    public override int RefCount(string tag)
    {
        return Operand.Contains(tag) ? 1 : 0;
    }

    public override string ToString() { return $"{base.ToString()} - Operand: {Operand} Type: {Type}"; }
}

public class Wire : SheetElement
{
    public Wire(XmlNode node) : base(node)
    {
        try
        {
            string s;

            s = node.GetNamedAttributeItemValue("ToID");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                ToID = n;
            }

            s = node.GetNamedAttributeItemValue("FromID");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                FromID = n;
            }

            try { 
                ToParam = node.GetNamedAttributeItemValue("ToParam"); 
            } 
            catch (Exception ex )
            { 
                LogHelper.DebugPrint($"ERROR: Wire: ToParam: Error: {ex.Message}"); 
            }

            try { 
                FromParam = node.GetNamedAttributeItemValue("FromParam"); 
            } 
            catch (Exception ex) 
            { 
                LogHelper.DebugPrint($"ERROR: Wire: FromParam: Error: {ex.Message}"); 
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Wire: Import node {node.Name} failed with {ex.Message}");
        }



    }



    public int ToID { get; set; } = -1;
    public int FromID { get; set; } = -1;
    public string ToParam { get; set; } = string.Empty;
    public string FromParam { get; set; } = string.Empty;



    public override int RefCount(string tag)
    { 
        return 0; 
    }

    public override string ToString()
    {
        return $"{base.ToString()} Wire From: {FromID} {FromParam} to {ToID} {ToParam}";
    }
}

public class AddOnInstruction : SheetElement 
{
    public AddOnInstruction(XmlNode node) : base(node)
    {
        try
        {
            Name = node.GetNamedAttributeItemValue("Name");
            Operand = node.GetNamedAttributeItemValue("Operand");
            VisiblePins = node.GetNamedAttributeItemValue("VisiblePins");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: AddOnInstruction: Import node {node.Name} Failed with {ex.Message}");
        }

    }



    public string Name { get; set; } = string.Empty;
    public string Operand { get; set; } = string.Empty;
    public string VisiblePins { get; set; } = string.Empty;



    public override int RefCount(string tag)
    {
        return Operand.Contains(tag) ? 1 : 0;
    }

    public override string ToString() { return $"{base.ToString()} - Operand: {Operand} Name: {Name}"; }
}

public class FBD_TextBox : SheetElement
{
    public FBD_TextBox(XmlNode node) : base(node)
    {
        try
        {
            string? s;
            s = node.GetNamedAttributeItemValue("Width");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                Width = n;
            }
            Text = node.GetNamedAttributeItemValue("Text");
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: FBD_TextBox: Import node {node.Name} Failed with {ex.Message}");
        }

    }



    public int Width { get; set; } = -1;
    public string Text { get; set; } = string.Empty;



    public override int RefCount(string tag) 
    { 
        return 0; 
    }

    public override string ToString() { return $"{base.ToString()} - Text: {Text}"; }

}

public class Attachment : SheetElement
{
    public Attachment(XmlNode node) : base(node)
    {
        try
        {
            string? s;
            s = node.GetNamedAttributeItemValue("ToID");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                ToID = n;
            }

            s = node.GetNamedAttributeItemValue("FromID");
            if (s != null)
            {
                int n = 0;
                int.TryParse(s, out n);
                FromID = n;
            }
        }
        catch (Exception ex)
        {
            LogHelper.DebugPrint($"ERROR: Attachment: Import node {node.Name} Failed with {ex.Message}");
        }

    }



    public int ToID { get; set; } = -1;
    public int FromID { get; set; } = -1;



    public override int RefCount(string tag) 
    { 
        return 0; 
    }

    public override string ToString()
    {
        return $"{base.ToString()} From: {FromID} To: {ToID}";
    }
}
