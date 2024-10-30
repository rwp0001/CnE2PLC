using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace CnE2PLC
{

    // Stuctured Text classes
    public class ST_Routine : Routine
    {
        ST_Routine() { }

        public ST_Routine(XmlNode node) : base(node)
        {
            try
            {
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
            foreach (Line line in Lines) count += line.TagCount(tag);
            return count;
        }

        public override int AOICount(string type, string tag)
        {
            int count = 0;
            foreach (Line line in Lines) count += line.AOICount(type,tag);
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
    public class Line
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

        public int Number { get; set; } = -1;

        public string Text { get; set; } = string.Empty;

        public int TagCount(string tag) { return Regex.Matches(Text, Regex.Escape(tag)).Count; }

        public int AOICount(string type, string tag) {
            try
            {
                string s = $"{type}({tag}";
                if (!Text.Contains(s)) return 0;
                int count = 0;
                string[] rs = Text.Split(s);
                foreach (string r in rs)
                {
                    if (r.Length == 0) continue;
                    if (r[0] == ')' || r[0] == ',') count += 1;
                }
                return count;
            }
            catch (Exception ex )
            {

                throw;
            }
            
        }

        public override string ToString() { return $"{Number}\t{Text}"; }

    }


}
