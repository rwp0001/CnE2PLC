using libplctag;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using CnE2PLC.Helpers;

namespace CnE2PLC.PLC;



// FBD Classes
public class FBD_Routine : Routine
{
    FBD_Routine() { }
    public FBD_Routine(XmlNode node) : base(node)
    {
        try
        {
            XmlNodeList? FBDContent = node.SelectNodes("FBDContent");
            if (FBDContent != null)
            {
                foreach (XmlNode node2 in FBDContent)
                {
                    foreach (XmlNode sheet in node2.ChildNodes) Sheets.Add(new Sheet(sheet));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import ST Routine Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }
    }

    public List<Sheet> Sheets { get; set; } = new();

    public override int TagCount(string tag)
    {
        int r = 0;
        foreach (Sheet sheet in Sheets) r += sheet.TagCount(tag);
        return r;
    }

    public override int AOICount(string type, string tag)
    {
        int r = 0;
        foreach (Sheet sheet in Sheets) r += sheet.AOICount(type,tag);
        return r;
    }

    public override string ToText()
    {
        string s = string.Empty;
        foreach (Sheet sheet in Sheets) s += sheet.ToString();
        return s;
    }

    public override string ToString() { return $"Name:{Name} Sheets: {Sheets.Count}"; }


}

public class Sheet
{

    public Sheet() { }

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

                    //case "Wire":
                    //    Elements.Add(new Wire(element));
                    //    break;

                    case "AddOnInstruction":
                        Elements.Add(new AddOnInstruction(element));
                        break;

                    //case "TextBox":
                    //    Elements.Add(new TextBox(element));
                    //    break;

                    //case "Attachment":
                    //    Elements.Add(new Attachment(element));
                    //    break;

                    default:
                        break;
                }

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Sheet Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public int? Number { get; set; }        
    public string? Description { get; set; }

    public List<SheetElement> Elements { get; set; } = new();

    public int TagCount(string tag) {
        int r = 0;
        foreach(SheetElement element in Elements) r += element.TagCount(tag);
        return r;
    }

    public int AOICount(string type,string tag)
    {
        int r = 0;
        foreach (SheetElement element in Elements) r += element.AOICount(type,tag);
        return r;
    }

    public string? Name { get; set; }

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
            Debug.WriteLine($"Import Sheet Element Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    #region Public Propreties
    public int ID { get; set; } = 0;
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
    #endregion

    public virtual int TagCount(string tag) { throw new NotImplementedException(); }
    public virtual int AOICount(string type, string tag) { throw new NotImplementedException(); }
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
            Debug.WriteLine($"Import IRef Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public string? Operand { get; set; }
    public bool? HideDesc { get; set; }

    public override int TagCount(string tag) { return Operand.ContainsAsBit(tag); }
    public override int AOICount(string type, string tag) { return 0; }

    public override string ToString() { return $"{base.ToString()} - Operand: {Operand}"; }
}

public class ORef : SheetElement
{
    public ORef() { }

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
            Debug.WriteLine($"Import ORef Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public string? Operand { get; set; }
    public bool? HideDesc { get; set; }

    public override int TagCount(string tag) { return Operand.ContainsAsBit(tag); }

    public override int AOICount(string type, string tag) { return 0; }

    public override string ToString() { return $"{base.ToString()} - Operand: {Operand}"; }
}

public class Block : SheetElement 
{
    public Block() { }
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
            Debug.WriteLine($"Import Block Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public string? Type { get; set; }
    public string? Operand { get; set; }
    public string? VisiblePins { get; set; }
    public bool? HideDesc { get; set; }

    public override int TagCount(string tag) { return Operand.ContainsAsBit(tag); }

    public override int AOICount(string type, string tag) { return 0; }

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

            try { ToParam = node.GetNamedAttributeItemValue("ToParam"); } catch (Exception ex ){ Debug.Print($"Wire Error: {ex.Message}"); }

            try { FromParam = node.GetNamedAttributeItemValue("FromParam"); } catch (Exception ex) { Debug.Print($"Wire Error: {ex.Message}"); }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Import Wire Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }



    }

    public int? ToID { get; set; }
    public int? FromID { get; set; }
    public string? ToParam { get; set; }
    public string? FromParam { get; set; }

    public override int TagCount(string tag) { return 0; }
    public override int AOICount(string type, string tag) { return 0; }

    public override string ToString()
    {
        string s = base.ToString();
        s += FromID == null ? "" : $" From: {FromID}";
        s += FromParam == null ? "" : $" From: {FromParam}";
        s += ToID == null ? "" : $" To: {ToID}";
        s += ToParam == null ? "" : $" To: {ToParam}";
        return s;
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
            Debug.WriteLine($"Import AOI Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public string? Name { get; set; }
    public string? Operand { get; set; }
    public string? VisiblePins { get; set; }

    public override int TagCount(string tag) { return Operand.ContainsAsBit(tag); }

    public override int AOICount(string type, string tag) { 
        if( Name != type ) return 0;
        return Operand.ContainsAsBit(tag); 
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
            Debug.WriteLine($"Import Textbox Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public int? Width { get; set; }
    public string? Text { get; set; }

    public override int TagCount(string tag) { return 0; }
    public override int AOICount(string type, string tag) { return 0; }

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
            Debug.WriteLine($"Import Attachment Error: Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}");
        }

    }

    public int? ToID { get; set; }
    public int? FromID { get; set; }
    public override int TagCount(string tag) { return 0; }
    public override int AOICount(string type, string tag) { return 0; }

    public override string ToString()
    {
        string s = base.ToString();
        s += FromID == null ? "" : $" From: {FromID}";
        s += ToID == null ? "" : $" To: {ToID}";
        return s;
    }
}
