using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Devices.Pwm;

namespace CnE2PLC
{


    // FBD Classes
    public class FBD_Routine : Routine
    {
        FBD_Routine() { }
        public FBD_Routine(XmlNode node)
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                Type = node.Attributes.GetNamedItem("Type").Value;
                foreach (XmlNode node2 in node.SelectNodes("FBDContent"))
                {
                    foreach (XmlNode sheet in node2.ChildNodes) Sheets.Add(new Sheet(sheet));
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import ST Routine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<Sheet> Sheets { get; set; } = new();

        public int TagCount(string tag)
        {
            int r = 0;
            foreach (Sheet sheet in Sheets) r += sheet.TagCount(tag);
            return r;
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
                string s = node.Attributes.GetNamedItem("Number").Value;
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
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Sheet Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string? s = node.Attributes.GetNamedItem("ID").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    ID = n;
                }
                s = node.Attributes.GetNamedItem("X").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    X = n;
                }

                s = node.Attributes.GetNamedItem("Y").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    Y = n;
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Sheet Element Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        public int? ID { get; set; } = 0;
        public int? X { get; set; } = 0;
        public int? Y { get; set; } = 0;

        public virtual int TagCount(string tag) { return 0; }

        public override string ToString() { return $"ID: {ID} at  X:{X}, Y:{Y}"; }
    }

    public class IRef : SheetElement
    {
        public IRef(XmlNode node) : base(node) 
        {
            try
            {
                Operand = node.Attributes.GetNamedItem("Operand").Value;
                string s = node.Attributes.GetNamedItem("HideDesc").Value;
                HideDesc = s[0] == 'f' ? false : true;
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import IRef Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public string? Operand { get; set; }
        public bool? HideDesc { get; set; }

        public override int TagCount(string tag) { return Regex.Matches(Operand, tag).Count;}

        public override string ToString() { return $"{base.ToString()} - Operand: {Operand}"; }
    }

    public class ORef : SheetElement
    {
        public ORef() { }

        public ORef(XmlNode node) : base(node)
        {
            try
            {
                Operand = node.Attributes.GetNamedItem("Operand").Value;
                string s = node.Attributes.GetNamedItem("HideDesc").Value;
                HideDesc = s[0] == 'f' ? false : true;
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import ORef Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public string? Operand { get; set; }
        public bool? HideDesc { get; set; }

        public override int TagCount(string tag) { return Regex.Matches(Operand, tag).Count; }

        public override string ToString() { return $"{base.ToString()} - Operand: {Operand}"; }
    }

    public class Block : SheetElement 
    {
        public Block() { }
        public Block(XmlNode node) : base(node)
        {
            try
            {
                Type = node.Attributes.GetNamedItem("Type").Value;
                Operand = node.Attributes.GetNamedItem("Operand").Value;
                VisiblePins = node.Attributes.GetNamedItem("VisiblePins").Value;
                string s = node.Attributes.GetNamedItem("HideDesc").Value;
                HideDesc = s[0] == 'f' ? false : true;
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Block Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public string? Type { get; set; }
        public string? Operand { get; set; }
        public string? VisiblePins { get; set; }
        public bool? HideDesc { get; set; }

        public override int TagCount(string tag) { return Regex.Matches(Operand, tag).Count; }

        public override string ToString() { return $"{base.ToString()} - Operand: {Operand} Type: {Type}"; }
    }

    public class Wire : SheetElement
    {
        public Wire(XmlNode node)
        {
            try
            {
                string? s;
                s = node.Attributes.GetNamedItem("ToID").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    ToID = n;
                }

                s = node.Attributes.GetNamedItem("FromID").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    FromID = n;
                }
                try { ToParam = node.Attributes.GetNamedItem("ToParam").Value; } catch (Exception ex ){ }

                try { FromParam = node.Attributes.GetNamedItem("FromParam").Value; } catch (Exception ex) { }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Wire Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        public int? ToID { get; set; }
        public int? FromID { get; set; }
        public string? ToParam { get; set; }
        public string? FromParam { get; set; }

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
                Name = node.Attributes.GetNamedItem("Name").Value;
                Operand = node.Attributes.GetNamedItem("Operand").Value;
                VisiblePins = node.Attributes.GetNamedItem("VisiblePins").Value;
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import AOI Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public string? Name { get; set; }
        public string? Operand { get; set; }
        public string? VisiblePins { get; set; }

        public override int TagCount(string tag) { return Regex.Matches(Operand, tag).Count; }

        public override string ToString() { return $"{base.ToString()} - Operand: {Operand} Name: {Name}"; }
    }

    public class TextBox : SheetElement
    {
        public TextBox(XmlNode node) : base(node)
        {
            try
            {
                string? s;
                s = node.Attributes.GetNamedItem("Width").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    Width = n;
                }
                Text = node.Attributes.GetNamedItem("Text").Value;
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Textbox Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public int? Width { get; set; }
        public string? Text { get; set; }

        public override string ToString() { return $"{base.ToString()} - Text: {Text}"; }

    }

    public class Attachment : SheetElement
    {
        public Attachment(XmlNode node) : base(node)
        {
            try
            {
                string? s;
                s = node.Attributes.GetNamedItem("ToID").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    ToID = n;
                }

                s = node.Attributes.GetNamedItem("FromID").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    FromID = n;
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Attachment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public int? ToID { get; set; }
        public int? FromID { get; set; }
        public override string ToString()
        {
            string s = base.ToString();
            s += FromID == null ? "" : $" From: {FromID}";
            s += ToID == null ? "" : $" To: {ToID}";
            return s;
        }
    }


    // SFC classes
    public class SFC_Routine : Routine
    {
        public SFC_Routine() { }
        public SFC_Routine(XmlNode node) { }

        internal class Step { }
        internal class Transition { }
        internal class Branch { }
        internal class DirectedLink { }
    }

}
