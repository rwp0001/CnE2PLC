using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CnE2PLC
{

    // Stuctured Text classes
    internal class ST_Routine : Routine
    {
        ST_Routine() { }

        public ST_Routine(XmlNode node)
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                Type = node.Attributes.GetNamedItem("Type").Value;
                foreach (XmlNode node2 in node.SelectNodes("STContent"))
                {
                    foreach (XmlNode line in node2.ChildNodes) Lines.Add(new Line(line));
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import ST Routine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override int TagCount(string tag)
        {
            int count = 0;
            foreach (Line line in Lines)
            {
                if (!line.Text.Contains(tag)) continue;
                count += Regex.Matches(line.Text, tag).Count;
            }
            return count;
        }

        public List<Line> Lines { get; set; } = new();

        public override string ToText()
        {
            string s = string.Empty;
            foreach (Line line in Lines) s += $"{line.Text}\n";
            return s;
        }

        public override string ToString() { return $"Name:{Name} Rungs: {Lines.Count}"; }

    }
    internal class Line : INotifyPropertyChanged
    {
        public Line() { }
        public Line(XmlNode node)
        {
            try
            {
                string s = node.Attributes.GetNamedItem("Number").Value;
                int n;
                int.TryParse(s, out n);
                Number = n;
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (node2.Name == "Text") Text = node2.InnerText;
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Rung Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public int? Number { get; set; } = -1;

        public string Text { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() { return $"{Number}\t{Text}"; }

    }


}
