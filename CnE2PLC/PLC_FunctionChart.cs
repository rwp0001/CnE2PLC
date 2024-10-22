using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CnE2PLC
{
    // SFC classes
    public class SFC_Routine : Routine
    {
        public SFC_Routine() { }
        public SFC_Routine(XmlNode node) : base(node) 
        {
        }

        public string? Description { get; set; }

        public List<ChartElement> Elements { get; set; } = new();

        public override int TagCount(string tag)
        {
            int r = 0;
            foreach (ChartElement element in Elements) r += element.TagCount(tag);
            return r;
        }

        public override string ToString() { return $"Chart Name:{Name} Elements: {Elements.Count} Desc: {Description}"; }
    }
    public class ChartElement
    {
        public ChartElement() { }
        public ChartElement(XmlNode node)
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

        public virtual int TagCount(string tag) { throw new NotImplementedException(); }

        public override string ToString() { return $"ID: {ID} at  X:{X}, Y:{Y}"; }
    }


    public class Step : ChartElement 
    { 
        public Step() { }
        public Step(XmlNode node) : base(node)
        {
            try
            {
                Operand = node.Attributes.GetNamedItem("Operand").Value;

                string s = node.Attributes.GetNamedItem("HideDesc").Value;
                HideDesc = s[0] == 'f' ? false : true;


                s = node.Attributes.GetNamedItem("DescX").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    DescX = n;
                }

                s = node.Attributes.GetNamedItem("DescY").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    DescY = n;
                }

                s = node.Attributes.GetNamedItem("DescWidth").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    DescWidth = n;
                }

                s = node.Attributes.GetNamedItem("InitialStep").Value;
                InitialStep = s[0] == 'f' ? false : true;

                s = node.Attributes.GetNamedItem("PresetUsesExpr").Value;
                PresetUsesExpr = s[0] == 'f' ? false : true;

                s = node.Attributes.GetNamedItem("LimitHighUsesExpr").Value;
                LimitHighUsesExpr = s[0] == 'f' ? false : true;

                s = node.Attributes.GetNamedItem("LimitLowUsesExpr").Value;
                LimitLowUsesExpr = s[0] == 'f' ? false : true;

                s = node.Attributes.GetNamedItem("ShowActions").Value;
                ShowActions = s[0] == 'f' ? false : true;

            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Step Element Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string? Operand { get; set; }
        public bool? HideDesc { get; set; }
        public int? DescX { get; set; }
        public int? DescY { get; set; }
        public int? DescWidth { get; set; }
        public bool? InitialStep { get; set; }
        public bool? PresetUsesExpr { get; set; }
        public bool? LimitHighUsesExpr { get; set; }
        public bool? LimitLowUsesExpr { get; set; }
        public bool? ShowActions { get; set; }



    }
    public class Transition : ChartElement 
    {
        public Transition() { }
        public Transition(XmlNode node) : base(node) {
            try
            {
                Operand = node.Attributes.GetNamedItem("Operand").Value;

                string s = node.Attributes.GetNamedItem("HideDesc").Value;
                HideDesc = s[0] == 'f' ? false : true;


                s = node.Attributes.GetNamedItem("DescX").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    DescX = n;
                }

                s = node.Attributes.GetNamedItem("DescY").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    DescY = n;
                }

                s = node.Attributes.GetNamedItem("DescWidth").Value;
                if (s != null)
                {
                    int n = 0;
                    int.TryParse(s, out n);
                    DescWidth = n;
                }

                XmlNode? Content = node.SelectSingleNode("Condition");

                foreach (XmlNode node2 in Content.SelectNodes("STContent"))
                {
                    foreach (XmlNode line in node2.ChildNodes) Conditions.Add(new Line(line));
                }

            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Step Element Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public string? Operand { get; set; }
        public bool? HideDesc { get; set; }
        public int? DescX { get; set; }
        public int? DescY { get; set; }
        public int? DescWidth { get; set; }
        public List<Line> Conditions { get; set; } = new();
        
        public override int TagCount(string tag) 
        {
            int count = 0;
            foreach (Line line in Conditions) count += line.TagCount(tag);
            return count;
        }

    }


    public class Branch : ChartElement { }
    public class DirectedLink : ChartElement { }

    public class SFC_TextBox : ChartElement
    {
        public SFC_TextBox() { }
        public SFC_TextBox(XmlNode node) : base(node)
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
}
