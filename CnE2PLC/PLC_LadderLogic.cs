﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
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

        public override int AOICount(string type, string tag)
        {
            int count = 0;
            foreach (Rung rung in Rungs) count += rung.AOICount(type, tag);
            return count;
        }

        public override List<string> GetIO(string type, string tag)
        {
            List<string> r = new();
            foreach (Rung rung in Rungs) r.AddRange(rung.GetIO(type, tag));
            return r;
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

        public int TagCount(string tag) { return Regex.Matches(Text, Regex.Escape(tag)).Count; }

        public int AOICount(string type, string tag)
        {
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
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<string> GetIO(string type, string tag)
        {
            List<string> r = new();
            try
            {
                if (AOICount(type, tag) == 0) return r;

                string ts = $"{type}({tag}";
                string[] rs = Text.Split(ts);

                foreach (string s in rs)
                {
                    if (s.Length == 0) continue;
                    if (s[0] == ')') continue;
                    if ( s[0] == ',') {
                        string[] s2 = s.Split(')');
                        string[] s3 = s2[0].Split(',');
                        r.Add(s3[1]);
                    }
                }
                return r;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public override string ToString() { return $"{Number}\t{Text}"; }

    }


}
