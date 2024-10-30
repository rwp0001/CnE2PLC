using System.ComponentModel;
using System.Diagnostics;
using System.Xml;


namespace CnE2PLC
{
    /// <summary>
    /// Base class for a Logix Tag.
    /// </summary>
    public class PLCTag
    {
        [DebuggerStepThrough]
        public PLCTag() { }

        [DebuggerStepThrough]
        public PLCTag(XmlNode node)
        {
            try
            {
                XmlNode? n;
                n = node.Attributes.GetNamedItem("Name");
                if(n != null) Name = n.InnerText;

                n = node.Attributes.GetNamedItem("DataType");
                if (n != null) DataType = n.InnerText;

                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "Description") Description = item.InnerText;
                    n = item.Attributes.GetNamedItem("Format");
                    if (n != null ) if(n.InnerText == "L5K") L5K = item.InnerText; 
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}", "Import Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #region Public Properties
        /// <summary>
        /// Tag Name from PLC Program
        /// </summary>
        [DisplayName("Tag Name")]
        public string Name { get; set; }


        [DisplayName("Tag Scope")]
        public string Path
        {
            get
            {
                if (PathValue == string.Empty) return "Controller";
                return PathValue;
            }
            set
            {
                PathValue = value;
            }
        }

        [DisplayName("Tag Data Type")]
        public string DataType { get; set; }


        /// <summary>
        /// PLC Program Tag Comment 
        /// </summary>
        [DisplayName("Tag Description")]
        public string Description
        {
            get
            {
                return DescriptionValue;
            }
            set
            {
                value.ReplaceLineEndings(" ");
                value = value.Replace('\n', ' ');
                value = value.Replace('\r', ' ');
                value = value.Trim();
                if (value.Length > 1)
                {
                    DescriptionValue = value;
                }
            }
        }

        /// <summary>
        /// How many time is the Tag used in the program.
        /// </summary>
        [DisplayName("References")]
        public int References { get; set; } = 0;

        public override string ToString() { return $"{Name}, {DataType}, {Description}"; }

        #endregion

        #region Private Values
        protected string NameValue = string.Empty;
        protected string DataTypeValue = string.Empty;
        protected string PathValue = string.Empty;
        protected string DescriptionValue = string.Empty;
        protected string L5K = string.Empty;
        #endregion

        /// <summary>
        /// Used to set the property of class without cast to the class.
        /// </summary>
        /// <param name="DLM_Name">Property Name</param>
        /// <param name="DLM_DataType">Property Data Type</param>
        /// <param name="DLM_Value">Property Value</param>
        /// <exception cref="Exception"></exception>
        [DebuggerStepThrough]
        protected void SetProperty(string DLM_Name, string DLM_DataType, string DLM_Value)
        {
            try
            {
                // Use reflection to set
                var prop = this.GetType().GetProperty(DLM_Name);
                if (prop == null) return;

                switch (DLM_DataType.ToLower())
                {
                    case "bool":
                        bool NewBool = false;
                        if (DLM_Value[0] == '1') NewBool = true;
                        prop.SetValue(this, NewBool);
                        break;

                    case "dint":
                        int NewInt32 = 0;
                        int.TryParse(DLM_Value, out NewInt32);
                        prop.SetValue(this, NewInt32);
                        break;

                    case "int":
                        int NewInt16 = 0;
                        int.TryParse(DLM_Value, out NewInt16);
                        prop.SetValue(this, NewInt16);
                        break;

                    case "real":
                        float NewFloat = 0;
                        float.TryParse(DLM_Value, out NewFloat);
                        prop.SetValue(this, NewFloat);
                        break;


                    default:
                        break;
                }

                return;
            }
            catch (Exception e)
            {
                string s = string.Format("Failed to set property {0} to value {1} with ex: {2}", DLM_Name, DLM_Value, e.Message);
                var r = MessageBox.Show(s, "Set Property Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        [DebuggerStepThrough]
        public virtual void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) { throw new NotImplementedException(); }

        /// <summary>
        /// This class used to sort tags.
        /// </summary>
        [DebuggerStepThrough]
        public class TagComparer : IComparer<PLCTag>
        {
            public int Compare(PLCTag first, PLCTag second)
            {
                if (first != null && second != null)
                {
                    // next check the scope
                    int r = first.Path.CompareTo(second.Path);
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

    }

    /// <summary>
    /// Base class for all XTO Add-on Instructions
    /// </summary>
    public class XTO_AOI : PLCTag
    {
        [DebuggerStepThrough]
        public XTO_AOI() { }

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
                            XmlNode n1 = item.Attributes.GetNamedItem("Format");
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
                            var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}\n{item.InnerText}", "Import Child Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        #endregion

        #region Private Values
        protected string Cfg_EquipDescValue = string.Empty;
        protected List<string> L5K_strings = new List<string>();
        #endregion

        /// <summary>
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
        public class TagComparer : IComparer<XTO_AOI>
        {
            public int Compare(XTO_AOI first, XTO_AOI second)
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

        public override string ToString() { return base.ToString(); }

    }

    public class PLC_Base : PLCTag
    {
        public PLC_Base() { }
        public PLC_Base(XmlNode node) :base(node) {
            try
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "Data")
                    {
                        try
                        {

                            if (item.Attributes.GetNamedItem("Format").Value == "Decorated")
                            {
                                foreach (XmlNode DataValue in item.ChildNodes)
                                {
                                    SetProperty(
                                        "Value",
                                        DataValue.Attributes.GetNamedItem("DataType").Value.Trim(),
                                        DataValue.Attributes.GetNamedItem("Value").Value.Trim()
                                        );
                                }
                                continue;
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}\n{item.InnerText}", "Import Child Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}\n", "Import Child Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [DebuggerStepThrough]
        public override void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {  }
    }
    
    public class PLC_Bool : PLC_Base
    {
        public PLC_Bool() { //DataType = "BOOL";
                            }
        public PLC_Bool(XmlNode node) : base(node) { //DataType = "BOOL";
                                                     }
        public bool? Value { get; set; }
    }
    public class PLC_Int : PLC_Base
    {
        public PLC_Int() { //DataType = "Int";
                           }
        public PLC_Int(XmlNode node) : base(node) { //DataType = "Int";
                                                    }
        public int? Value { get; set; }
    }
    public class PLC_Real : PLC_Base
    {
        public PLC_Real() { DataType = "REAL"; }
        public PLC_Real(XmlNode node) : base(node) { DataType = "REAL"; }
        public float? Value { get; set; }
    }
    public class ABTimer : PLC_Base
    {
        // Standard AB Timer Data Type
        // 12 bytes on PLC
        public Int32 PRE;
        public Int32 ACC;
        public bool EN;
        public bool TT;
        public bool DN;

    }
    public class ABString : PLC_Base
    {
        private int Max_Length = 82;
        public Int32 LEN = 0;
        public byte[] DATA;
        public string test;

        public ABString()
        {
            DATA = new byte[Max_Length];
        }
        public ABString(string Input, int Length = 82)
        {
            SetLength(Length);
            Set(Input);
        }
        public ABString(int Length)
        {
            SetLength(Length);
            DATA = new byte[Max_Length];
        }

        public void Set(string Input)
        {
            DATA = new byte[Max_Length];
            if (Input.Length > Max_Length) Input = Input.Substring(0, Max_Length);
            for (int i = 0; i < Input.Length; i++) DATA[i] = (byte)Input[i];
            LEN = Input.Length;
        }
        public void SetLength(int Input)
        {
            if (Input < 0) throw new Exception("Must be greater then zero.");
            string Old_Value = ToString();
            Max_Length = Input;
            Set(Old_Value);
        }

        public override string ToString()
        {
            string value = "";
            for (int i = 0; i < LEN; i++)
            {
                if (DATA[i] == 0) break;
                value.Append((char)DATA[i]);
            }
            return value;
        }

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