using libplctag;
using libplctag.DataTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Collections;

namespace CnE2PLC
{

    public class PLCTag : INotifyPropertyChanged
    {
        public PLCTag()
        {
            if (Columns == null) AddDefaultColumns();
        }

        #region Public Properties
        [DisplayName("Tag Name")]
        public string Name
        {
            get
            {
                return NameValue;
            }
            set
            {
                NameValue = value;
                NotifyPropertyChanged();
            }
        }
        [DisplayName("Tag Path")]
        public string Path
        {
            get
            {
                return PathValue;
            }
            set
            {
                PathValue = value;
                NotifyPropertyChanged();
            }
        }
        [DisplayName("Data Type")]
        public string DataType
        {
            get
            {
                return DataTypeValue;
            }
            set
            {
                DataTypeValue = value;
                NotifyPropertyChanged();
            }
        }
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return DescriptionValue;
            }
            set
            {
                DescriptionValue = value;
                NotifyPropertyChanged();
            }
        }

        public override string ToString() { return string.Format("{0} {1} {2}", Name, DataType, Description); }

        static public IDictionary<int, string>? Columns;

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Private Values
        private string NameValue = string.Empty;
        private string DataTypeValue = string.Empty;
        private string PathValue = string.Empty;
        private string DescriptionValue = string.Empty;
        private string L5K = string.Empty;
        #endregion

        public void Import(XmlNode node)
        {
            if (Columns == null) AddDefaultColumns();

            try
            {
                Name = node.Attributes.GetNamedItem("Name").Value;
                DataType = node.Attributes.GetNamedItem("DataType").Value;

                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "Description")
                    {
                        Description = item.InnerText;
                        continue;
                    }

                    if (item.Attributes.GetNamedItem("Format").Value == "Decorated")
                    {
                        foreach (XmlNode DataValueMember in item.ChildNodes[0].ChildNodes)
                        {
                            SetProperty(
                                DataValueMember.Attributes.GetNamedItem("Name").Value.Trim(),
                                DataValueMember.Attributes.GetNamedItem("DataType").Value.Trim(), 
                                DataValueMember.Attributes.GetNamedItem("Value").Value.Trim()
                                );
                        }
                        continue;
                    }

                    if (item.Attributes.GetNamedItem("Format").Value == "L5K")
                    {
                        // need to add processing the L5K to get the local strings.
                        L5K = item.InnerText;
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create object.");
            }


        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddDefaultColumns()
        {
            Columns = new Dictionary<int, string>
            {
                { 1, "Name" },
                { 2 , "Path" },
                { 3 , "Data Type" },
                { 4 , "Description" }
            };
        }

        public static List<object> DecodeL5K(string Input)
        {
            List<object> ReturnValue = new List<object>();
            string[] splitDoubleBrackets = Input.Split("]]");

            foreach (string splitDoubleBracket in splitDoubleBrackets)
            {
                string pattern1 = @"\[(?'index1'[-0-9]+),\[(?'data'.*)";
                Match index1Match = Regex.Match(splitDoubleBracket, pattern1, RegexOptions.Singleline);
                if (index1Match.Success)
                {
                    int index1 = int.Parse(index1Match.Groups["index1"].Value);
                    string doubleBracketData = index1Match.Groups["data"].Value;

                    string pattern2 = @"\[(?'index2'[-0-9]+),'(?'data'[^']+)";
                    MatchCollection index2Matches = Regex.Matches(doubleBracketData, pattern2, RegexOptions.Singleline);

                    foreach (Match match in index2Matches.Cast<Match>())
                    {
                        int index2 = int.Parse(match.Groups["index2"].Value);
                        string data2 = match.Groups["data"].Value;

                        ReturnValue.Add(new object[] { index1, index2, data2 });
                    }
                }
            }

            return ReturnValue;
        }




        public void SetProperty(string DLM_Name, string DLM_DataType, string DLM_Value)
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
                throw new Exception("Failed to set property.");
            }


        }

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
    public class ABString : PLCTag
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