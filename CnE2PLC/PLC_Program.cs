using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace CnE2PLC
{
    public class PLC_Program : INotifyPropertyChanged
    {
        public PLC_Program() { }

        public PLC_Program(XmlNode node) 
        {
            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                TestEdits = node.Attributes.GetNamedItem("TestEdits").Value;
                Disabled = node.Attributes.GetNamedItem("Disabled").Value;
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
                            foreach (XTO_AOI tag in XTO_AOI.ProcessL5XTags(node2.ChildNodes))
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
        public string? Disabled { get; set; }
        public string? UseAsFolder { get; set; }
        public List<Routine> Routines { get; set; } = new();
        public List<PLCTag> LocalTags { get; set; } = new();

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() { return $"{Name} Routines: {Routines.Count} Tags: {LocalTags.Count}"; }

        public int TagCount(string tag)
        {
            int r = 0;
            foreach (Routine routine in Routines) r += routine.TagCount(tag);
            return r;
        }


    }

    public class Routine : INotifyPropertyChanged
    {
        public Routine() { }

        public Routine(XmlNode node) 
        {
            Import(node);
        }

        #region Public Properties
        public string Name {  get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        #endregion

        public override string ToString() { return $"Name: {Name} Type: {Type}"; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        public virtual int TagCount(string tag) { return 0; }

        public virtual string ToText() { return string.Empty; }

        public void Import(XmlNode node)
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

    }



}