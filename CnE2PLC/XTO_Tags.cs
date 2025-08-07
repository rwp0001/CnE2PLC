using System.ComponentModel;
using System.Diagnostics;
using System.Xml;


namespace CnE2PLC
{

    /// <summary>
    /// Base class for all XTO Add-on Instructions
    /// </summary>
    public class XTO_AOI : PLCTag
    {
        [DebuggerStepThrough]
        public XTO_AOI() { }

        /// <summary>
        /// Converts a node to a Tag, and fills out all the data.
        /// </summary>
        /// <param name="node"></param>
        [DebuggerStepThrough]
        public XTO_AOI(XmlNode node) : base(node)
        {
            try
            {
                DecodeL5K();

                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "Data")
                    {
                        try
                        {
                            // skip arrays for now.
                            XmlNode n1 = node.Attributes.GetNamedItem("Dimensions");
                            if (n1 != null)
                            {
                                throw new Exception("Array WTF.");
                            }

                            n1 = item.Attributes.GetNamedItem("Format");
                            if (n1 != null) if (n1.InnerText == "Decorated")
                            {
                                foreach (XmlNode DataValueMember in item.ChildNodes[0].ChildNodes)
                                {
                                    var n = DataValueMember.Attributes.GetNamedItem("Name");
                                    var d = DataValueMember.Attributes.GetNamedItem("DataType");
                                    var v = DataValueMember.Attributes.GetNamedItem("Value");
                                    if (n == null || d == null || v == null) continue;
                                    SetProperty(n.InnerText, d.InnerText, v.InnerText);
                                }
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}\n{item.InnerText}", 
                                "Import Child Node Exception", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}", "Import Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Public Properties
        /// <summary>
        /// Used to sort the devices
        /// </summary>
        [DisplayName("Equipment Number")]
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public int EquipNum
        {
            get
            {
                string returnvalue = string.Empty;
                bool found = false;
                foreach (char c in Name)
                {
                    if (!found & char.IsDigit(c)) found = true;

                    if (found & char.IsDigit(c)) returnvalue += c;

                    if (found & !char.IsDigit(c)) break;

                }
                int rv = 0;
                int.TryParse(returnvalue, out rv);
                if (rv < 20) rv = 0;
                return rv;
            }
        }

        /// <summary>
        /// all AOIs have this bit.
        /// </summary>
        public bool? EnableIn { get; set; }
        
        /// <summary>
        /// all AOIs have this bit.
        /// </summary>
        public bool? EnableOut { get; set; }

        /// <summary>
        /// used to indicate if a device is installed.
        /// </summary>
        public bool? InUse { get; set; }

        /// <summary>
        /// displayed on the HMI.
        /// </summary>
        [DisplayName("Equipment ID")]
        public string? Cfg_EquipID { get; set; }
        
        /// <summary>
        /// used to select the control displayed on the HMI. (normally blank)
        /// </summary>
        public string? Cfg_Faceplate { get; set; }

        /// <summary>
        /// displayed on the HMI.
        /// </summary>
        public string? Cfg_Detail { get; set; }

        /// <summary>
        /// string of connected IO points.
        /// </summary>
        [DisplayName("Connected IO")]
        public string IO
        {
            get
            {
                string s = string.Empty;
                foreach (string s2 in IOs) s += $"{s2}, ";
                if( s.Length > 3 ) s = s.Substring(0, s.Length - 2);
                return s;
            }
        }

        /// <summary>
        /// Description from HMI faceplate data.
        /// </summary>
        [DisplayName("Equipment Description")]
        public string Cfg_EquipDesc
        {
            get
            {
                return Cfg_EquipDescValue;
            }
            set
            {
                value.ReplaceLineEndings(" ");
                value = value.Replace('\n', ' ');
                value = value.Replace('\r', ' ');
                value = value.Trim();
                if (value.Length > 1)
                {
                    Cfg_EquipDescValue = value;
                }
            }
        }

        /// <summary>
        /// list of connected IO points.
        /// </summary>
        public List<string> IOs { get; set; } = new();

        /// <summary>
        /// AOI is called in code.
        /// </summary>
        [DisplayName("AOI Called")]
        public bool AOICalled {
            get 
            { 
                if(AOICalls == 0 ) return false;
                return true;
            }
        } 

        /// <summary>
        /// how many times the AOI is called in code.
        /// </summary>
        public int AOICalls { get; set; } = 0;


        // filters
        public virtual bool Alarmed { get { return false; } }
        public virtual bool Bypassed { get { return false; } }
        public virtual bool Simmed { get { return false; } }
        public virtual bool Placeholder { get { return false; } }
        public virtual bool NotInUse { get { return false; } }

        #endregion

        #region Private Values
        protected string Cfg_EquipDescValue = string.Empty;
        protected List<string> L5K_strings = new List<string>();
        #endregion

        #region Static Values
        public static string? AOI_Name { get; set; }
        public static string? AOI_Revision { get; set; }
        public static DateTime? AOI_CreatedDate { get; set; }
        public static string? AOI_CreatedBy { get; set; }
        public static DateTime? AOI_EditedDate { get; set; }
        public static string? AOI_EditedBy { get; set; }
        public static string? AOI_Description { get; set; }
        public static string? AOI_AdditionalHelpText { get; set; }
        #endregion


/*        /// <summary>
        /// converts XML to a list of Tags.
        /// </summary>
        /// <param name="XMLTags"></param>
        /// <returns></returns>
        public static List<XTO_AOI> ProcessL5XTags(XmlNodeList XMLTags)
        {
            List<XTO_AOI> Tags = new List<XTO_AOI>();
            try
            {
                foreach (XmlNode item in XMLTags)
                {
                try
                {
                    string Name, TagType, DataType;
                        Name = string.Empty;
                        DataType = string.Empty;
                        TagType = string.Empty;

                        if (item.Attributes.Count != 0)
                        {
                            Name = item.Attributes.GetNamedItem("Name").Value;
                            TagType = item.Attributes.GetNamedItem("TagType").Value;
                            if (TagType == "Alias")
                            {
                                DataType = TagType;
                            }
                            else
                            {
                                DataType = item.Attributes.GetNamedItem("DataType").Value;
                            }

                        }

                        switch (DataType)
                        {
                            case "AIData":
                                Tags.Add(new AIData(item));
                                break;

                            case "AOData":
                                Tags.Add(new AOData(item));
                                break;

                            case "DIData":
                                Tags.Add(new DIData(item));
                                break;

                            case "DOData":
                                Tags.Add(new DOData(item));
                                break;

                            case "TwoPositionValveV2":
                                Tags.Add(new TwoPositionValveV2(item));
                                break;

                            case "TwoPositionValve":
                                Tags.Add(new TwoPositionValve(item));
                                break;

                            case "ValveAnalog":
                                Tags.Add(new ValveAnalog(item));
                                break;

                            case "Intlk_8":
                                Tags.Add(new Intlk_8(item));
                                break;

                            case "Intlk_16":
                                Tags.Add(new Intlk_16(item));
                                break;

                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        var r = MessageBox.Show($"Error Processing node.\nEroor: {ex.Message}\nName: {item.Name}\nNode:\n{item.InnerText}\n", "Tag Import Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

                Tags.Sort(new TagComparer());
                return Tags;
            }

            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}", "Import L5X Tags Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Tags;
            }
        }
*/
        /// <summary>
        /// Finds strings inside of L5K and exports them to the L5K_strings array.
        /// </summary>
        [DebuggerStepThrough]
        private void DecodeL5K()
        {
            L5K_strings = new List<string>();
            try
            {
                foreach (string s1 in L5K.Split("["))
                {
                    if (!s1.Contains("'")) continue;
                    string L = s1.Split(",")[0];
                    string O = string.Empty;
                    int len = 0;
                    int.TryParse(L, out len);
                    if (len != 0)
                    {
                        string D = s1.Split(",")[1].Split("'")[1];
                        D = ReplaceDollar(D);
                        D = D.Trim();
                        O = D;
                    }
                    L5K_strings.Add(O);
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {this.Name}", "L5K Strings Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /// I hate AB strings...
            string ReplaceDollar(string str)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '$')
                    {
                        char c = CodeToChar(str.Substring(i + 1, 2));
                        string s1 = str.Substring(0, i);
                        string s2 = str.Substring(i + 3);
                        str = $"{s1}{c}{s2}";
                    }
                }
                str = str.Replace('\t', ' ');
                str = str.Replace('\r', ' ');
                str = str.Replace('\n', ' ');
                return str;
            }

            char CodeToChar(string code)
            {
                int c = 0;
                int.TryParse(code, out c);
                if (c == 0) return ' ';
                if (c == '\r') return ' ';
                if (c == '\n') return ' ';
                if (c == '\t') return ' ';
                return (char)c;
            }

        }

        [DebuggerStepThrough]
        public virtual void ClearCounts() { throw new NotImplementedException(); }

        /// <summary>
        /// This class used to sort tags.
        /// </summary>
        [DebuggerStepThrough]
        public class AOIComparer : IComparer<XTO_AOI>
        {
            public int Compare(XTO_AOI? first, XTO_AOI? second)
            {
                if (first != null && second != null)
                {
                    // Check EquipID first
                    int r = first.EquipNum.CompareTo(second.EquipNum);
                    if (r != 0) return r;

                    // next check the scope
                    r = first.Path.CompareTo(second.Path);
                    if (r != 0) return r;

                    // check the name
                    return first.Name.CompareTo(second.Name);
                }

                if (first == null && second == null)
                {
                    // We can't compare any properties, so they are essentially equal.
                    return 0;
                }

                if (first != null)
                {
                    // Only the first instance is not null, so prefer that.
                    return -1;
                }

                // Only the second instance is not null, so prefer that.
                return 1;
            }
        }

        public new class TagComparer : IComparer<PLCTag>
        {
            public int Compare(PLCTag? first, PLCTag? second)
            {
                if (first != null && second != null)
                {
                    int r;
                    if(first.GetType() == typeof(XTO_AOI) & first.GetType() == typeof(XTO_AOI) )
                    {
                        XTO_AOI t1 = (XTO_AOI)first;
                        XTO_AOI t2 = (XTO_AOI)second;
                        // Check EquipID first
                        r = t1.EquipNum.CompareTo(t2.EquipNum);
                        if (r != 0) return r;
                    }

                    // next check the scope
                    r = first.Path.CompareTo(second.Path);
                    if (r != 0) return r;

                    // check the name
                    return first.Name.CompareTo(second.Name);
                }

                if (first == null && second == null)
                {
                    // We can't compare any properties, so they are essentially equal.
                    return 0;
                }

                if (first != null)
                {
                    // Only the first instance is not null, so prefer that.
                    return -1;
                }

                // Only the second instance is not null, so prefer that.
                return 1;
            }
        }


        public override string ToString() { return base.ToString(); }




    }


    public class AlmData : PLCTag
    {
        // 80 bytes on PLC
        static readonly int Length = 80;

        //Parameters
        public bool? EnableIn { get; }
        public bool? EnableOut { get; }
        public bool? Enable { get; set; }
        public bool? Status { get; set; }
        public bool? Active { get; set; }
        public bool? AutoAck { get; set; }
        public bool? Ack { get; set; }
        public bool? Alm { get; set; }
        public bool? SD { get; set; }
        public bool? AckCmd { get; set; }
        public bool? ResetCmd { get; set; }
        public bool? StartupDlyCond { get; set; }
        public Int32? Cfg_DlyTmr { get; set; }
        public Int32? Cfg_OnTmr { get; set; }
        public Int32? Cfg_OffTmr { get; set; }
        public Int32? Cfg_SDTmr { get; set; }

        // Local Tags
        public ABTimer? AckTmr { get; set; }
        public ABTimer? DlyTmr { get; set; }
        public ABTimer? OffTmr { get; set; }
        public ABTimer? OnTmr { get; set; }
        public ABTimer? SDTmr { get; set; }
    }

    public class BypData : PLCTag
    {
        // 24 bytes on PLC
        static readonly int Length = 24;

        //Parameters
        public bool? EnableIn { get; }
        public bool? EnableOut { get; }
        public bool? TimedReq { get; set; }
        public bool? SupvReq { get; set; }
        public bool? StopReq { get; set; }
        public bool? Cmd { get; set; }
        public bool? Active { get; set; }
        public Int32? Tmr_PRE { get; set; }
        public Int32? Tmr_ACC { get; set; }


        // Local Tags
        public ABTimer? Tmr { get; set; }
    }


}