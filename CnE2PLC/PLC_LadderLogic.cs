using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml;

namespace CnE2PLC
{

    /// <summary>
    /// Ladder Logic Classes
    /// </summary>
    internal class RLL_Routine : Routine
    {
        /// <summary>
        /// Rockwell Ladder Logic
        /// </summary>
        /// <param name="node"></param>
        public RLL_Routine(XmlNode node)
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                Type = node.Attributes.GetNamedItem("Type").Value;
                foreach (XmlNode node2 in node.SelectNodes("RLLContent"))
                {
                    foreach (XmlNode rung in node2.ChildNodes) Rungs.Add(new Rung(rung));
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import LL Routine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public List<Rung> Rungs { get; set; } = new();

        public override int TagCount(string tag)
        {
            int count = 0;
            foreach (Rung rung in Rungs)
            {
                if (!rung.Text.Contains(tag)) continue;
                count += Regex.Matches(rung.Text, Regex.Escape(tag)).Count;
            }
            return count;
        }

        public override string ToText()
        {
            string s = string.Empty;
            foreach (Rung rung in Rungs) s += $"{rung.Text}\n";
            return s;
        }

        public override string ToString() { return $"Name:{Name} Rungs: {Rungs.Count}"; }
    }

    /// <summary>
    /// Rung Class
    /// </summary>
    internal class Rung
    {
        public Rung() { }
        public Rung(XmlNode node)
        {
            try
            {
                string s = node.Attributes.GetNamedItem("Number").Value;
                int n;
                int.TryParse(s, out n);
                Number = n;
                Type = node.Attributes.GetNamedItem("Type").Value;

                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (node2.Name == "Text") Text = node2.InnerText;
                    if (node2.Name == "Comment") Comment = node2.InnerText;
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Rung Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #region Public Properties
        public int Number { get; set; } = -1;
        public string Type { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        #endregion

        public override string ToString() { return $"{Number}\t{Text}"; }

    }


}
