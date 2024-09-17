﻿using libplctag;
using libplctag.DataTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;

namespace CnE2PLC
{
    internal class CnE_Device : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddDefaultColumns(){
            Columns = new Dictionary<int, string>
            {
                { 1, "Equipment" },
                { 2 , "Description" },
                { 3 , "Device_Tag_Name" },
                { 4 , "PLC_Tag_Name" },
                { 5 , "IO_Details" },
                { 6 , "Status" },
                { 7 , "SetPoint" },
                { 8 , "Unit" },
                { 9 , "Alarm_On_Delay" },
                { 10 , "Alarm_Off_Delay" },
                { 11 , "Notes" },
                { 12 , "SCADA_Indicator" },
                { 13 , "SCADA_Alarm" },
                { 14 , "Callout_Code" },
                { 15 , "Auto_Acknowledge" }
            };
        }

        public CnE_Device() 
        {
            if (Columns == null) AddDefaultColumns();
        }

        public CnE_Device(Microsoft.Office.Interop.Excel.Range row)
        {
            try
            {
                if (Columns == null) AddDefaultColumns();

                foreach (int i in Columns.Keys)
                {
                    if (row.Cells[1, i].Value != null)
                    {
                        // Use reflection to set
                        var prop = this.GetType().GetProperty(Columns[i]);
                        prop.SetValue(this, row.Cells[1, i].Value.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create object.");
            }

        }

        #region PrivateData
        private string EquipmentValue = String.Empty;
        private string DescriptionValue = String.Empty;
        private string Device_Tag_NameValue = String.Empty;
        private string PLC_Tag_NameValue = String.Empty;
        private string IO_DetailsValue = String.Empty;
        private string StatusValue = String.Empty;
        private string SetPointValue = String.Empty;
        private string UnitValue = String.Empty;
        private string Alarm_On_DelayValue = String.Empty;
        private string Alarm_Off_DelayValue = String.Empty;
        private string NotesValue = String.Empty;
        private string SCADA_IndicatorValue = String.Empty;
        private string SCADA_AlarmValue = String.Empty;
        private string Callout_CodeValue = String.Empty;
        private string Auto_AcknowledgeValue = String.Empty;
        private bool EnabledValue = true;
        private bool WildcardValue = false;
        #endregion

        #region Public Properties

        [DisplayName("Equipment")]
        public string Equipment
        {
            get
            {
                return this.EquipmentValue;
            }
            set
            {
                this.EquipmentValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return this.DescriptionValue;
            }
            set
            {
                this.DescriptionValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Device Tag Name")]
        public string Device_Tag_Name
        {
            get
            {
                return this.Device_Tag_NameValue;
            }
            set
            {
                this.Device_Tag_NameValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("PLC Tag Name")]
        public string PLC_Tag_Name
        {
            get
            {
                return this.PLC_Tag_NameValue;
            }
            set
            {
                this.PLC_Tag_NameValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("IO Details")]
        public string IO_Details
        {
            get
            {
                return this.IO_DetailsValue;
            }
            set
            {
                this.IO_DetailsValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Status")]
        public string Status
        {
            get
            {
                return this.StatusValue;
            }
            set
            {
                this.StatusValue = value;
                if (value.ToLower().Contains("not in use")) this.Enabled = false;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("SetPoint")]
        public string SetPoint
        {
            get
            {
                return this.SetPointValue;
            }
            set
            {
                this.SetPointValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Unit")]
        public string Unit
        {
            get
            {
                return this.UnitValue;
            }
            set
            {
                this.UnitValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Alarm On Delay")]
        public string Alarm_On_Delay
        {
            get
            {
                return this.Alarm_On_DelayValue;
            }
            set
            {
                this.Alarm_On_DelayValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Alarm Off Delay")]
        public string Alarm_Off_Delay
        {
            get
            {
                return this.Alarm_Off_DelayValue;
            }
            set
            {
                this.Alarm_Off_DelayValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Notes")]
        public string Notes
        {
            get
            {
                return this.NotesValue;
            }
            set
            {
                this.NotesValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("SCADA Indicator")]
        public string SCADA_Indicator
        {
            get
            {
                return this.SCADA_IndicatorValue;
            }
            set
            {
                this.SCADA_IndicatorValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("SCADA Alarm")]
        public string SCADA_Alarm
        {
            get
            {
                return this.SCADA_AlarmValue;
            }
            set
            {
                this.SCADA_AlarmValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Callout Code")]
        public string Callout_Code
        {
            get
            {
                return this.Callout_CodeValue;
            }
            set
            {
                this.SCADA_AlarmValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Auto Acknowledge")]
        public string Auto_Acknowledge
        {
            get
            {
                return this.Auto_AcknowledgeValue;
            }
            set
            {
                this.Auto_AcknowledgeValue = value;
                NotifyPropertyChanged();
            }
        }

        [DisplayName("Enabled"),Browsable(false)]
        public bool Enabled
        {
            get
            {
                return this.EnabledValue;
            }
            set
            {
                this.EnabledValue = value;
                NotifyPropertyChanged();
            }
        }


        static public IDictionary<int, string>? Columns;

        public override string ToString() { return string.Format( "{0} - {1} - {2}", PLC_Tag_Name, Equipment, Description ); }
        #endregion

    }


}
