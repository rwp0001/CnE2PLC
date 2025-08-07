using Microsoft.Office.Interop.Excel;
using System.Security.Policy;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Excel = Microsoft.Office.Interop.Excel;

namespace CnE2PLC
{
    class AI_CTRL : PLCTag
    {
        /// <summary>
        /// Analog Input
        /// </summary>
        public AI_CTRL() { }

        /// <summary>Enable Input - System Defined Parameter</summary>
        public bool? EnableIn {  get; set; }

        /// <summary>Enable Output - System Defined Parameter</summary>
        public bool? EnableOut { get; set; }

        /// <summary>Value in Tick of the Channel Data as defined in the I/O Hardware Configuration</summary>
        public float? Ch_Data_Raw { get; set; }

        /// <summary>
        /// Predefined per Module
        /// 1 = 4 to 20mA
        /// 2 = 0 to 20mA
        /// 3 = 0 to 5V
        /// 4 = 0 to 10V
        /// 5 = -10V to 10V
        /// </summary>  
        public int? Ch_Data_Type { get; set; } 

        ///<summary>Value of the Minimum Raw Input in Ticks as defined in the I/O Hardware Configuration</summary>
        public float? Ch_Data_Raw_Min {  get; set; }

        ///<summary>Value of the Maximum Raw Input in Ticks as defined in the I/O Hardware Configuration</summary>
        public float? Ch_Data_Raw_Max {  get; set; }

        ///<summary>
        ///0 = Disabled
        ///1 = XMIT Shutdown while Out of Range Condition exists
        ///2 = XMIT Alarm while Out of Range Condition exists
        ///</summary>
        public int? Ch_Data_OOR_Fault_Type {  get; set; }

        ///<summary>The time in seconds, used to calculate the deviation between two consecutive samples.</summary>
        public float? Ch_Data_Low_Pass_Fillter_Sample_Rate {  get; set; }

        ///<summary>The Maximum Value the Channel Data may change per sample rate before failing the Low Pass Filter</summary>
        public float? Ch_Data_Low_Pass_Fillter_Max_Deviation {  get; set; }

        ///<summary></summary>
        public float? User_EU_Multiplier {  get; set; }

        ///<summary>
        ///0 = Disabled, 
        ///1 = Class A,
        ///2 = Class B1,
        ///3 = Class B2,
        ///4 = Class C, 
        ///5 = Class C / P,
        ///6 = Class P, 
        ///7 = Class S
        ///</summary>
        public int? LL_Fault_Class {  get; set; }

        ///<summary>
        ///0 = Disabled, 
        ///1 = Class A,
        ///2 = Class B1,
        ///3 = Class B2,
        ///4 = Class C,
        ///5 = Class C / P,
        ///6 = Class P,
        ///7 = Class S
        ///</summary>
        public int? L_Fault_Class { get; set; }

        ///<summary>
        ///0 = Disabled,
        ///1 = Class A,
        ///2 = Class B1,
        ///3 = Class B2,
        ///4 = Class P,
        ///5 = Vibration,
        ///6 = Class S
        ///</summary>
        public int? H_Fault_Class { get; set; }

        ///<summary>
        ///  0 = Disabled, 
        ///  1 = Class A,
        ///  2 = Class B1, 
        ///  3 = Class B2,
        ///  4 = Class P,
        ///  5 = Vibration,
        ///  6 = Class S
        ///</summary>
        public int? HH_Fault_Class { get; set; }

        ///<summary>
        /// Enable Input to allow the arming of Class S alarm and shutdown faults
        ///</summary>
        public bool? Class_S_Fault_Enable { get; set; }

        ///<summary>
        /// Maximium Amount of time allowed for Analog Scaled Value to meet Annunciation Setpoint for Class S alarm and shutdown faults
        ///</summary>
        public int? Class_S_Fault_Delay_Seconds { get; set; }

        ///<summary>
        /// Low Setpoint in Engineering Units as entered via the HMI for use with Low Low Alarm or Low Shutdown Annunciation
        ///</summary>

        public float? LL_SP { get; set; }

        ///<summary>
        /// Low Setpoint in Engineering Units as entered via the HMI for use with Low Alarm Annunciation
        ///</summary>
        public float? L_SP { get; set; }

        ///<summary>
        /// High Setpoint in Engineering Units as entered via the HMI for use with High Alarm Annunciation
        ///</summary>
        public float? H_SP { get; set; }

        ///<summary>
        /// High Setpoint in Engineering Units as entered via the HMI for use with High High Alarm or High Shutdown Annunciation
        ///</summary>
        public float? HH_SP { get; set; }

        ///<summary>
        /// Transmitter Minimum Value in Engineering Units as entered via the HMI
        ///</summary>
        public float? Input_Scaled_Min { get; set; }

        ///<summary>
        /// Transmitter Maximum Value in Engineering Units as entered via the HMI
        ///</summary>
        public float? Input_Scaled_Max { get; set; }

        ///<summary>
        /// Maximum Time Allowed before trip when alarm and shutdown fault = vibration
        ///</summary>
        public float? VIbration_Debounce { get; set; }

        ///<summary>
        /// Global AOI Control Word Bit Mapping
        /// .0  Class B1 Timer Done - INPUT
        /// .1  Class B2 Timer Done - INPUT
        /// .2  Class C Disable Arming - INPUT
        /// .3  Class P Timer Done - INPUT
        /// .6  First Out Shutdown Exist - INPUT
        /// .7  Fault Reset - INPUT
        /// .8  Individual Test Mode Enabled - INPUT
        /// .9  Global Test Mode Enabled - INPUT
        /// .10 Shutdown Exist in Test Mode - OUTPUT
        /// .13 Channel Bypass Enabled - INPUT / OUTPUT
        /// .14 Channel Bypass Disabled - INPUT / OUTPUT
        /// .15 Any Fault Horn Trigger - INPUT / OUTPUT
        ///</summary>
        public int? AOI_Fault_Control { get; set; }

        ///<summary>
        /// 0 = Channel Faults Enabled
        /// 1 = Channel Faults Disabled
        ///</summary>
        public bool? Ch_Bypass_Fault_Enable { get; set; }

        ///<summary>
        /// Selected Primary Engineering Units via HMI
        ///</summary>
        public int? Primary_EU { get; set; }

        ///<summary>
        /// Selected Secondary Engineering Units via HMI
        ///</summary>
        public int? Secondary_EU { get; set; }




        /*








  <Parameter Name = "HMI_EU_Edits_Enabled" TagType="Base" DataType="INT" Usage="InOut" Radix="Decimal" Required="true" Visible="true" Constant="false" />
  <Parameter Name = "SI_EU_Multiplier_Array" TagType="Base" DataType="REAL" Dimensions="50" Usage="InOut" Radix="Float" Required="true" Visible="true" Constant="false" />
  <Parameter Name = "Non_Latching_Alarm_Enable" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="Read/Write">
    <Description><![CDATA[0 = Disabled(All Alarms require Reset)
1 = Enabled(All Alarms Self Clear)]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Non_Latching_Alarm_Debounce_Seconds" TagType="Base" DataType="INT" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="Read/Write">
    <Description><![CDATA[Minimum Time Required for Non-Latching Alarms to be healthy before self clear.]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "INT" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "OOR_Fault_First_Out_Shutdown_Inhibit" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="Read/Write">
    <Description><![CDATA[When Seletected(value = 1)
the Transmitter Open Circuit will be First Out inhibited via the First_Out_Shutdown Input]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "SD_LL_Alarm_Function_Selected" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="None">
    <Description><![CDATA[0 = Disabled(LL_SP trip equals shutdown)
1 = Enabled(LL_SP trip equals alarm)]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "SD_HH_Alarm_Function_Selected" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="None">
    <Description><![CDATA[0 = Disabled(HH_SP trip equals shutdown)
1 = Enabled(HH_SP trip equals alarm)]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "FIrst_Out_SD_Inhibit" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="None">
    <Description><![CDATA[0 = First Out Disabled
1 = First Out Enabled]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "EU_Conversion_Active" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="true" Visible="true" ExternalAccess="Read/Write">
    <Description><![CDATA[0 = Units are English / 1 = Units are Metric]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "EU_Conversion_Enable" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="Read/Write">
    <Description><![CDATA[0 = Unit Conversion Disabled / 1 = Unit Conversion Enabled]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[1]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="1" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Alarm_Group" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="true" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[Storage Word
of all Alarm Faults
LL(Alarm), L, H, HH(Alarm) and OC]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Shutdown_Group" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="true" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[Storage Word
of all Shutdown Faults
LL, HH and OC]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Input_Scaled_Shutdown_Snapshot" TagType="Base" DataType="REAL" Usage="Output" Radix="Float" Required="true" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[Value of Anlog Input Scaled when First Out Shutdown initially becomes true.]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0.00000000e+000]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "REAL" Radix="Float" Value="0.0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "HMI_Fault_Bit_Status_Word" TagType="Base" DataType="INT" Usage="Output" Radix="Decimal" Required="true" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[BIT 0=AL_L, BIT 1=AL_H,
BIT2=AL_OC, BIT 3=SD_LL,
 BIT 4=SD_HH, BIT 5=SD_OC,
BIT 6=AL_LL, BIT 7=AL_HH
(Allows all alarm and shutdown
functions to be placed in one word
for HMI software annunciations)]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "INT" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "HMI_Fault_Bit_Status_Decimal" TagType="Base" DataType="INT" Usage="Output" Radix="Decimal" Required="true" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[
0 = No Annunciatoin
1 = AL_L, 2 = AL_H,
  3 = AL_OC, 4 = SD_LL,
 5 = SD_HH, 6 = SD_OC,
7 = AL_LL, 8 = AL_HH]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "INT" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "SP_Clip_Enable" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="Read/Write">
    <Description><![CDATA[0 = Setpoint Clipped Disabled / 1 = Setpoint Clipped Enabled]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "HMI_Keypad_Entry_Min_EU" TagType="Base" DataType="REAL" Usage="Output" Radix="Float" Required="true" Visible="true" ExternalAccess="Read Only">
    <DefaultData Format = "L5K" >< ! [CDATA[0.00000000e+000]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "REAL" Radix="Float" Value="0.0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "HMI_Keypad_Entry_Max_EU" TagType="Base" DataType="REAL" Usage="Output" Radix="Float" Required="true" Visible="true" ExternalAccess="Read Only">
    <DefaultData Format = "L5K" >< ! [CDATA[0.00000000e+000]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "REAL" Radix="Float" Value="0.0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Current_EU_Pointer" TagType="Base" DataType="DINT" Usage="Output" Radix="Decimal" Required="true" Visible="true" ExternalAccess="Read Only">
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "DINT" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Input_Scaled" TagType="Base" DataType="REAL" Usage="Output" Radix="Float" Required="true" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[Value in Engineering Units from the linear scaling of Anlg_Raw_Input between the Xmit_Min_SP and  Xmit_Max_SP]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0.00000000e+000]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "REAL" Radix="Float" Value="0.0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "SD_LL" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
while a Low Shutdown
Fault Condition Exists
 for any Fault Class]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "AL_LL" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
while a High HIgh Alarm
Fault Condition Exists
for any Fault Class]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "AL_L" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
 while a Low Alarm
Fault Condition Exists
for any Fault Class]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "AL_H" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
while a High Alarm
Fault Condition Exists
for any Fault Class]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "AL_HH" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
while a High HIgh Alarm
Fault Condition Exists
for any Fault Class]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "SD_HH" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
while a High Shutdown
Fault Condition Exists
for any Fault Class]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "AL_OOR" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
 while an Out of Range
Fault Condition Exists
or Xmit_Fault_Type=2
(Alarm)]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "SD_OOR" TagType="Base" DataType="BOOL" Usage="Output" Radix="Decimal" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[ENERGIZED
 while an Out of Range
Fault Condition Exists
for Xmit_Fault_Type=1
(Shutdown)]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Class_C_One_Shot_Disable" TagType="Base" DataType="BOOL" Usage="Input" Radix="Decimal" Required="false" Visible="false" ExternalAccess="None">
    <DefaultData Format = "L5K" >< ! [CDATA[0]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "BOOL" Radix="Decimal" Value="0" />
    </DefaultData>
  </Parameter>
  <Parameter Name = "Ch_Data_Raw_EU_Mult" TagType="Base" DataType="REAL" Usage="Input" Radix="Float" Required="false" Visible="true" ExternalAccess="Read Only">
    <Description><![CDATA[HMI Channel Data Raw Multiplier for Input Status Screen]]></Description>
    <DefaultData Format = "L5K" >< ! [CDATA[1.00000000e-003]] ></ DefaultData >
    < DefaultData Format="Decorated">
      <DataValue DataType = "REAL" Radix="Float" Value="0.001" />
    </DefaultData>
  </Parameter>
</Parameters>




        */




    }

}