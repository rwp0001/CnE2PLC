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

        /// <summary>
        /// Convert a XML base tag to a class. 
        /// </summary>
        /// <param name="node">Node to be processed.</param>
        [DebuggerStepThrough]
        public PLCTag(XmlNode node)
        {
            try
            {
                XmlNode? n;
                n = node.Attributes.GetNamedItem("Name");
                if (n != null) Name = n.InnerText;

                n = node.Attributes.GetNamedItem("TagType");
                if (n != null)
                {
                    switch (n.InnerText)
                    {
                        case "Base": 
                            TagType = TagTypes.Base; 
                            break;

                        case "Alias":
                            TagType = TagTypes.Alias;
                            break;

                        case "Consumed":
                            TagType = TagTypes.Consumed;
                            break;

                        case "Produced":
                            TagType = TagTypes.Produced;
                            break;
                    }
                }

                n = node.Attributes.GetNamedItem("DataType");
                if (n != null) DataType = n.InnerText;

                n = node.Attributes.GetNamedItem("Constant");
                if (n != null) Constant = GetBool(n.InnerText);

                n = node.Attributes.GetNamedItem("Dimensions");
                if (n != null) GetDimensions(n.InnerText);

                n = node.Attributes.GetNamedItem("ExternalAccess");
                if (n != null)
                {
                    switch (n.InnerText)
                    {
                        case "Read/Write":
                            ExternalAccess = TagAccess.ReadWrite;
;                           break;

                        case "Read Only":
                            ExternalAccess = TagAccess.ReadOnly;
                            break;

                        case "None":
                            ExternalAccess = TagAccess.None;
                            break;
                    }
                }

                n = node.Attributes.GetNamedItem("Radix");
                if (n != null)
                {
                    switch (n.InnerText)
                    {
                        case "Decimal":
                            Radix = TagRadix.Decimal;
                            break;

                        case "Float":
                            Radix = TagRadix.Float;
                            break;

                        case "Hex":
                            Radix = TagRadix.Hex;
                            break;

                        case "ASCII":
                            Radix = TagRadix.ASCII;
                            break;

                        case "Octal":
                            Radix = TagRadix.Octal;
                            break;

                        case "Exponential":
                            Radix = TagRadix.Exponential;
                            break;

                        //case "DateTime":
                        //    Radix = TagRadix.DateTime;
                        //    break;

                        //case "DateTime":
                        //    Radix = TagRadix.DateTimeNS;
                        //    break;
                    }
                }


                n = node.Attributes.GetNamedItem("Dimensions");
                if (n != null) GetDimensions(n.InnerText);


                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "Description") Description = item.InnerText;
                }
            }
            catch (System.Exception ex)
            {
                var r = MessageBox.Show($"Error: {ex.Message}\nNode: {node.Name}", "Import Node Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #region Public Properties
        /// <summary>
        /// Tag Name from PLC Program
        /// </summary>
        [DisplayName("Tag Name")]
        public string Name { get; set; } = "Name not set.";


        [DisplayName("Tag Scope")]
        public string Path
        {
            get
            {
                if (PathValue == string.Empty) return Controller.ControllerScopeName;
                return PathValue;
            }
            set
            {
                PathValue = value;
            }
        }

        [DisplayName("Tag DataType")]
        public string DataType { get; set; } = "DataType not set.";

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
                value = CleanString(value);
                if (value.Length > 1)
                {
                    DescriptionValue = value;
                }
            }
        }

        public int[]? Dimension { get; set; } = { 0, 0, 0 };

        /// <summary>
        /// Base or Alias
        /// </summary>
        public TagTypes? TagType { get; set; }

        public TagUsages? Usage { get; set; } 

        public TagRadix? Radix { get; set; }

        public TagAccess? ExternalAccess { get; set; }

        public TagClasses? TagClass { get; set; }

        /// <summary>
        /// Is this tag modifable?
        /// </summary>
        public bool? Constant { get; set; }

        /// <summary>
        /// Used to select the base tag and qualifier that the tag references.
        /// </summary>
        public string? AliasFor { get; set; }


        /// <summary>
        /// How many time is the Tag used in the program.
        /// </summary>
        [DisplayName("References")]
        public int References { get; set; } = 0;

        /// <summary>
        /// Tag is used in code.
        /// </summary>
        public bool Referenced
        {
            [DebuggerStepThrough]
            get { 
                if (References > 0) return true;
                return false;
            } 
        }


        /// <summary>
        /// Used to store the Type ID from the online PLC.
        /// </summary>
        static int? TypeID {  get; set; }

        public override string ToString()
        {
            string c = $"Name: {Name}\n";
            c += $"PLC Tag Description: {Description}\n";
            c += $"PLC DataType: {DataType}\n";
            return c;
        }

        /// <summary>
        /// How is a tag used.
        /// </summary>
        public enum TagUsages
        {
            Local,
            Input,
            Output,
            InOut,
            Public
        }

        /// <summary>
        /// Types of tags.
        /// </summary>
        public enum TagTypes
        {
            Base,
            Alias,
            Produced,
            Consumed
        }

        /// <summary>
        /// How to display the data.
        /// </summary>
        public enum TagRadix
        {
            Decimal,
            Float,
            Hex,
            ASCII,
            Octal,
            Exponential,
            DateTime,
            DateTimeNS
        }

        /// <summary>
        /// Used to select the access allowed to the tag from external applications such as HMIs. 
        /// </summary>
        public enum TagAccess
        {
            ReadWrite,
            ReadOnly,
            None
        }

        /// <summary>
        /// Indicates whether the tag is a Standard tag or a Safety tag.
        /// </summary>
        public enum TagClasses
        {
            Standard,
            Safety
        }

        #endregion

        #region Helper Functions
        /// <summary>
        /// Cleans strings for use for the program.
        /// </summary>
        /// <param name="input">string to be cleaned up.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string CleanString(string input)
        {
            input.ReplaceLineEndings(" ");
            input = input.Replace('\n', ' ');
            input = input.Replace('\r', ' ');
            input = input.Trim();
            return input;
        }

        /// <summary>
        /// Converts the string to three ints.
        /// </summary>
        /// <param name="input">Dimensions string.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static int[] GetDimensions(string input)
        {
            string[] values = input.Split(' ');
            int index = 0;
            int[] Dimension = new int[3];
            foreach (var value in values)
            {
                int d = 0;
                int.TryParse(value, out d);
                Dimension[index] = d;
                index++;
            }
            return Dimension;
        }

        /// <summary>
        /// Get the index value from a string.
        /// </summary>
        /// <param name="input">Index string.</param>
        /// <returns>The index value.</returns>
        ///[DebuggerStepThrough]
        public static int[] GetIndex(string input)
        {
            input = input.Substring(1, input.Length - 2);
            string[] values = input.Split(',');
            int index = 0;
            int[] Dimension = new int[3];
            foreach (var value in values)
            {
                int d = 0;
                int.TryParse(value, out d);
                Dimension[index] = d;
                index++;
            }
            return Dimension;
        }

        /// <summary>
        /// Convert string to a bool.
        /// </summary>
        /// <param name="input">String to Convert.</param>
        /// <returns>A bool.</returns>
        [DebuggerStepThrough]
        public static bool GetBool(string input)
        {
            if (input.ToLower().Contains("true")) return true;
            return false;
        }

        /// <summary>
        /// Used to sort the tags.
        /// </summary>
        /// <param name="first">First tag.</param>
        /// <param name="second">Second tag.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static int Compare(PLCTag? first, PLCTag? second)
        {
            try
            {
                if (first != null && second != null)
                {
                    int r;
                    if (first is XTO_AOI & second is XTO_AOI)
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
            catch (Exception)
            {
                string s = $"Failed to compare {first.Name} to {second.Name}.";
                var r = MessageBox.Show(s, "Unable to Compare Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0;

        }

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

                    case "sint":
                    case "int":
                    case "dint":
                        int NewInt = 0;
                        int.TryParse(DLM_Value, out NewInt);
                        prop.SetValue(this, NewInt);
                        break;

                    case "lint":
                        long NewLong = 0;
                        long.TryParse(DLM_Value, out NewLong);
                        prop.SetValue(this, NewLong);
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
            catch (System.Exception e)
            {
                var r = MessageBox.Show(
                    $"Failed to set property {DLM_Name} to value {DLM_Value} with ex: {e.Message}",
                    "Set Property Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }


        }

        #endregion

        #region Private Values
        protected string NameValue = string.Empty;
        protected string DataTypeValue = string.Empty;
        protected string PathValue = string.Empty;
        protected string DescriptionValue = string.Empty;
        #endregion

       

        /// <summary>
        /// Needed for the class to display correctly on the datagridview. Should be overridden by the derived class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DebuggerStepThrough]
        public virtual void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //throw new NotImplementedException(); 
        }

    }

    public class PLC_Bool : PLCTag
    {
        public PLC_Bool() { }
        public PLC_Bool(XmlNode node) : base(node) {
            Value = false;
            XmlNode n = node.LastChild.LastChild.Attributes.GetNamedItem("Value");
            if (n != null) if (n.Value == "1") Value = true;
        }
        public bool? Value { get; set; }
        public override string ToString() { return $"{base.ToString()}Value: {Value}\n"; }
    }
    public class PLC_Int : PLCTag
    {
        public PLC_Int() { }
        public PLC_Int(XmlNode node) : base(node) {
            Value = 0;
            XmlNode n = node.LastChild.LastChild.Attributes.GetNamedItem("Value");
            if (n != null)int.TryParse(n.InnerText, out int Value);
        }
        public int? Value { get; set; }
        public override string ToString() { return $"{base.ToString()}Value: {Value}\n"; }
    }
    public class PLC_Real : PLCTag
    {
        public PLC_Real() { }
        public PLC_Real(XmlNode node) : base(node) {
            Value = 0;
            XmlNode n = node.LastChild.LastChild.Attributes.GetNamedItem("Value");
            if (n != null) float.TryParse(n.InnerText, out float Value);
        }
        public float? Value { get; set; }
        public override string ToString() { return $"{base.ToString()}Value: {Value}\n"; }
    }
    public class ABString : PLCTag
    {
        /// <summary>
        /// Max Lenght of a string.
        /// </summary>
        private static int Max_Length = 500;

        /// <summary>
        /// Standard lenght of a string.
        /// </summary>
        private static int Std_Length = 82;

        #region PLC Fields
        /// <summary>
        /// Lenght of the stored string.
        /// </summary>
        public Int32 LEN = 0;

        /// <summary>
        /// Byte array of the stored string.
        /// </summary>
        public char[] DATA = new char[Std_Length];
        #endregion

        public ABString()
        {
            Length = Std_Length;
        }

        /// <summary>
        /// Create a string of lenght size and store the input in it.
        /// </summary>
        /// <param name="Input">String to be stored.</param>
        /// <param name="Length">Max lenght of the string.</param>
        public ABString(string Input, int Length )
        {
            this.Length = Length;
            Set(Input);
        }

        public ABString(int Length)
        {
            this.Length = Length;
        }

        public ABString(XmlNode node) : base(node)
        {
            Set(node.LastChild.InnerText);
        }

        public ABString(int[] Index, XmlNode node) : base(node)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Store a new value in the string.
        /// If the new value is longer then the max size than the value will be truncated.
        /// </summary>
        /// <param name="Input">String to be stored.</param>
        public void Set(string Input)
        {
            DATA = new char[DATA.Length];
            if (Input.Length > Length) Input = Input.Substring(0, Length);
            for (int i = 0; i < Input.Length; i++) DATA[i] = (char)Input[i];
            LEN = Input.Length;
            if (LEN > Length) LEN = Length;
            DataTypeValue = Input;
        }

        /// <summary>
        /// Change the size of the string.
        /// If the new size is shorter then the old size than the value will be truncated.
        /// </summary>
        public int Length 
        { 
            get { return DATA.Length; }
            set
            {
                if (value < 0) throw new System.Exception("Must be greater then zero.");
                if (value > Max_Length) throw new OverflowException();
                string s = DataTypeValue;
                DATA = new char[value];
                Set(s);
                DataType = "STRING";
                if (value != Std_Length) DataType = $"STRING_{value}";
            }
        }

        //public override string ToString()
        //{
        //    string value = "";
        //    for (int i = 0; i < LEN; i++)
        //    {
        //        if (DATA[i] == 0) break;
        //        char c = (char)DATA[i];
        //        value.Append(c);
        //    }
        //    return value;
        //}

        public override string ToString() { return $"{base.ToString()}Value: {DataTypeValue}\n"; }
    }

    public class ABTimer : PLCTag
    {
        // Standard AB Timer Data Type
        // 12 bytes on PLC
        public Int32 PRE;
        public Int32 ACC;
        public bool EN;
        public bool TT;
        public bool DN;

    }
}
