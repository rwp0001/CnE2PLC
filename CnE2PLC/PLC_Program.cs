using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace CnE2PLC
{
    public class PLC_Program
    {
        public PLC_Program() { }

        public PLC_Program(XmlNode node) 
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                TestEdits = node.Attributes.GetNamedItem("TestEdits").Value;
                Disabled = node.Attributes.GetNamedItem("Disabled").Value == "true";
                UseAsFolder = node.Attributes.GetNamedItem("UseAsFolder").Value;

                // optional
                if(node.Attributes["MainRoutineName"] != null) MainRoutineName = node.Attributes.GetNamedItem("MainRoutineName").Value;

                LocalTags = new();
                Routines = new();


                foreach (XmlNode node2 in node.ChildNodes)
                {
                    switch (node2.Name)
                    {
                        case "Tags":
                            foreach (XTO_AOI tag in Controller.ProcessTags(node2))
                            {
                                tag.Path = Name;
                                LocalTags.Add(tag);
                            }
                            break;

                        case "Routines":
                            foreach (XmlNode Routine in node2.ChildNodes) { 
                                string type = Routine.Attributes.GetNamedItem("Type").Value;
                                switch (type)
                                {
                                    case "RLL":
                                        Routines.Add(new RLL_Routine(Routine));
                                        break;

                                    case "ST":
                                        Routines.Add(new ST_Routine(Routine));
                                        break;

                                    case "FBD":
                                        Routines.Add(new FBD_Routine(Routine));
                                        break;

                                    case "SFC":
                                        Routines.Add(new SFC_Routine(Routine));
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Program Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }



        #region Public Properties
        public string? Name {  get; set; }
        public string? TestEdits { get; set; }
        public string? MainRoutineName { get; set; }
        public bool Disabled { get; set; }
        public string? UseAsFolder { get; set; }
        public List<Routine> Routines { get; set; } = new();
        public List<PLCTag> LocalTags { get; set; } = new();

        #endregion

        public override string ToString() { return $"{Name} Routines: {Routines.Count} Tags: {LocalTags.Count}"; }

        public int TagCount(string tag)
        {
            int r = 0;
            foreach (Routine routine in Routines) r += routine.TagCount(tag);
            return r;
        }

        public int AOICount(string type,string tag)
        {
            int r = 0;
            foreach (Routine routine in Routines) r += routine.AOICount(type,tag);
            return r;
        }

        public List<string> GetIO(string type,string tag)
        {
            List<string> r = new();
            foreach (Routine routine in Routines) foreach (string s in routine.GetIO(type, tag)) if(!r.Contains(s)) r.Add(s);
            return r;
        }


    }

    public class Routine
    {
        public Routine() { }

        public Routine(XmlNode node)
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                Type = node.Attributes.GetNamedItem("Type").Value;
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Name: {node.Name}\nError: {ex.Message}\n{node.InnerText}", "Import Routine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public virtual int TagCount(string tag) { throw new NotImplementedException(); }

        public virtual int AOICount(string type, string tag) { throw new NotImplementedException(); }

        public virtual List<string> GetIO(string type, string tag) {
            return new List<string>();
            //throw new NotImplementedException(); 
        }

        public virtual string ToText() { throw new NotImplementedException(); }

    }



}