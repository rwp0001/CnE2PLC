using CnE2PLC.Helpers;
using libplctag.NativeImport;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CnE2PLC.PLC;

/// <summary>
/// Represents the I/O mapping for an ESEL16 device, providing access to input and output channel data and methods for
/// synchronizing values with a PLC.
/// </summary>
/// <remarks>This class encapsulates the input and output data for an ESEL16 module, allowing retrieval and update
/// of channel values from a connected PLC. It provides methods to read and write all channel data in a single
/// operation, and exposes properties for accessing individual input and output channels. The class is intended for use
/// in scenarios where structured interaction with PLC I/O tags is required.</remarks>
internal class ESEL16_IOMapUDT
{
    public ESEL16_IOMapUDT() { }

    /// <summary>
    /// Initializes a new instance of the ESEL16_IOMapUDT class with the specified tag name, creating input and output
    /// mappings for 16 channels.
    /// </summary>
    /// <remarks>Each input channel is initialized with a unique tag name derived from the provided base name.
    /// The output mapping is similarly named. This constructor ensures that all input and output data structures are
    /// properly set up for use immediately after instantiation.</remarks>
    /// <param name="tagName">The tag on the PLC.</param>
    public ESEL16_IOMapUDT(string tagName)
    {
        Name = tagName;

        // Inputs
        for (int i = 0; i < 16; i++) IN[i] = new ESEL16_IODataUDT($"{Name}.IN[{i}]");

        // Output
        Out = new ESEL16_IODataUDT($"{Name}.Out");
    }

    /// <summary>
    /// Gets or sets the name associated with the tag.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of input channel data for the device.
    /// </summary>
    /// <remarks>The array contains data for up to 16 input channels, with each element representing the tag
    /// and location information for a specific channel. The order of elements corresponds to the channel indices.
    /// Modifying the array affects the input configuration for the device.</remarks>
    public ESEL16_IODataUDT[] IN { get; set; } = new ESEL16_IODataUDT[16];

    /// <summary>
    /// Gets or sets the output device data for the ESEL16 module.
    /// </summary>
    public ESEL16_IODataUDT Out { get; set; } = new ESEL16_IODataUDT();

    /// <summary>
    /// Retrieves and updates values from the PLC for all input channels and the output channel.
    /// </summary>
    /// <remarks>This method attempts to read values from each input channel and the output channel. If any
    /// read operation fails, an exception is thrown indicating the failure for the current instance.</remarks>
    /// <exception cref="Exception">Thrown when an error occurs while reading tags from the PLC.</exception>
    public void GetValuesFromPLC()
    {
        try
        {
            LogHelper.DebugPrint($"INFO: Reading values from PLC for {Name}");
            for (int i = 0; i < 16; i++) IN[i].GetValuesFromPLC();
            Out.GetValuesFromPLC();
        }
        catch (Exception)
        {
            string s = $"ERROR: Failed to read tags for {Name}";
            LogHelper.DebugPrint(s);
            throw new Exception(s);
        }
    }

    /// <summary>
    /// Writes the current input and output values to the connected PLC device.
    /// </summary>
    /// <remarks>This method attempts to update all input channels and the output channel on the PLC. If any
    /// write operation fails, the method throws an exception containing the device name for easier
    /// troubleshooting.</remarks>
    /// <exception cref="Exception">Thrown when an error occurs while writing tags to the PLC. The exception message includes the name of the
    /// affected device.</exception>
    public void SetValuesToPLC()
    {
        try
        {
            LogHelper.DebugPrint($"INFO: Writing values to PLC for {Name}");
            for (int i = 0; i < 16; i++) IN[i].SetValuesToPLC();
            Out.SetValuesToPLC();
        }
        catch (Exception)
        {
            string s = $"ERROR: Failed to write tags for {Name}";
            LogHelper.DebugPrint(s);
            throw new Exception(s);
        }
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that contains the type name and the value of the Name property.</returns>
    public override string ToString()
    {
        return $"ESEL16_IOMapUDT: {Name}";
    }

    /// <summary>
    /// Returns a formatted string that represents the current state of the I/O map, including the name, input values,
    /// and output value.
    /// </summary>
    /// <returns>A string containing the name of the I/O map, the values of all input channels, and the output value, formatted
    /// for display or logging.</returns>
    public string ToDataString()
    {
        string s = $"ESEL16_IOMapUDT: {Name}\n";
        for (int i = 0; i < 16; i++) s += $"IN[{i:D2}]: \t{IN[i].ToString()}\n";
        s += $"Out: \t{Out.ToString()}";
        return s;
    }


}

/// <summary>
/// Represents a data structure for managing PLC tag information and facilitating read and write operations to a PLC
/// using low-level tag access.
/// </summary>
/// <remarks>This class encapsulates properties corresponding to PLC tag metadata, such as name, CPU address,
/// program scope, tag identifier, data type, bias, and unit of measure. It provides methods to read values from and
/// write values to the PLC, handling the necessary serialization and communication with the underlying PLC library.
/// Instances of this class are typically used in scenarios where direct interaction with PLC tags is required. Thread safety is not guaranteed; concurrent access to the
/// same instance should be synchronized by the caller if needed.</remarks>
internal class ESEL16_IODataUDT
{
    public ESEL16_IODataUDT() { }

    [DebuggerStepThrough]
    public ESEL16_IODataUDT(string tagName)
    {
        Name = tagName;
    }

    /// <summary>
    /// Gets the connection string used to establish a connection to the data source.
    /// </summary>
    private string ConnectionString
    {
        get
        {
            if (_ConnectionString == string.Empty)
            {
                _ConnectionString = $"{Controller.connection_string}&elem_ssize={_tagSize}&elem_count=1&&name={Name}";
            }
            return _ConnectionString;
        }
    }

    private string _ConnectionString = string.Empty;

    /// <summary>
    /// PLC Tag Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// IP Address of the PLC for this tag.
    /// </summary>
    public string CPU { get; set; } = string.Empty;

    /// <summary>
    /// Controller or Program scope for this tag.
    /// </summary>
    public string Pgm { get; set; } = string.Empty;

    /// <summary>
    /// Tag name of the object in the PLC. Ex PIC001
    /// </summary>
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data type associated with the tag.
    /// </summary>
    /// <remarks>The data type typically indicates the format or category of the tag, such as 'PIDE'. This
    /// property is initialized to an empty string by default.</remarks>
    public string DataType { get; set; } = string.Empty;

    /// <summary>
    /// Tag Bias(if applies) (typ.GOR / WOR / or 'blank')
    /// </summary>
    public string Bias { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unit of measure associated with the value.
    /// </summary>
    public string UOM { get; set; } = string.Empty;

    /// <summary>
    /// The size of the tag on the PLC
    /// </summary>
    private readonly static int _tagSize = 144; // Size of the UDT in bytes

    /// <summary>
    /// Retrieves the latest values from the PLC and updates the corresponding properties with the received data.
    /// </summary>
    /// <remarks>This method communicates with the PLC to obtain current data for the configured tag. The properties
    /// CPU, Pgm, Tag, DataType, Bias, and UOM are updated with the values received from the PLC. Ensure that the connection
    /// parameters and tag configuration are valid before calling this method.</remarks>
    /// <exception cref="Exception">Thrown when the PLC tag cannot be created or read, or if an error occurs during communication with the PLC.</exception>
    [DebuggerStepThrough]
    public void GetValuesFromPLC()
    {
        int timeOut = (int)Controller.Timeout.TotalMilliseconds;

        byte[] data = new byte[_tagSize];

        // Create the tag
        LogHelper.DebugPrint($"INFO: Connecting to tag: {Name}");
        int tagId = plctag.plc_tag_create(ConnectionString, timeOut);
        int rc = plctag.plc_tag_status(tagId);
        if (rc != 0)
        {
            string s = $"ERROR: Failed to crate tag {Name} with code {rc}: {plctag.plc_tag_decode_error(rc)}";
            LogHelper.DebugPrint(s);
            throw new Exception(s);
        }

        // fill out buffer with current plc values
        LogHelper.DebugPrint($"INFO: Reading tag: {Name}");
        plctag.plc_tag_get_raw_bytes(tagId, 0, data, _tagSize);
        rc = plctag.plc_tag_status(tagId);
        if (rc != 0)
        {
            string s = $"ERROR: Failed to read tag {Name} with code {rc}: {plctag.plc_tag_decode_error(rc)}";
            LogHelper.DebugPrint(s);
            throw new Exception(s);
        }

        // Destroy the tag
        plctag.plc_tag_destroy(tagId);

        ESEL16_IODataUDT_Struct newdata = new ESEL16_IODataUDT_Struct().fromBytes(data);

        CPU = newdata.CPU;
        Pgm = newdata.Pgm;
        Tag = newdata.Tag;
        DataType = newdata.DataType;
        Bias = newdata.Bias;
        UOM = newdata.UOM;

    }

    /// <summary>
    /// Writes the current set of values to the PLC by creating a tag, populating it with data, and performing a write
    /// operation.
    /// </summary>
    /// <remarks>This method uses a low-level approach to interact with the PLC, bypassing higher-level
    /// abstractions for string tags. It creates a tag, sets its data, writes the tag to the PLC, and then destroys the
    /// tag. Any errors encountered during tag creation or writing will result in an exception being thrown.</remarks>
    /// <exception cref="Exception">Thrown if the tag cannot be created or written to the PLC due to an error returned by the underlying PLC
    /// library.</exception>
    [DebuggerStepThrough]
    public void SetValuesToPLC()
    {
        // I got tried of fighting writing STRING tags with libplctag, so going back to the C-style way.

        int timeOut = (int)Controller.Timeout.TotalMilliseconds;

        ESEL16_IODataUDT_Struct data = new ESEL16_IODataUDT_Struct
        {
            CPULen = CPU.Length,
            CPU = CPU,
            PgmLen = Pgm.Length,
            Pgm = Pgm,
            TagLen = Tag.Length,
            Tag = Tag,
            DataTypeLen = DataType.Length,
            DataType = DataType,
            BiasLen = Bias.Length,
            Bias = Bias,
            UOMLen = UOM.Length,
            UOM = UOM
        };

        // Create the tag
        LogHelper.DebugPrint($"INFO: Connecting to tag: {Name}");
        int tagId = plctag.plc_tag_create(ConnectionString, timeOut);
        int rc = plctag.plc_tag_status(tagId);
        if (rc != 0)
        {
            string s = $"ERROR: Failed to crate tag {Name} with code {rc}: {plctag.plc_tag_decode_error(rc)}";
            LogHelper.DebugPrint(s);
            throw new Exception(s);
        }

        // fill out buffer with current plc values
        plctag.plc_tag_set_raw_bytes(tagId, 0, data.getBytes(), _tagSize);

        // Write the tag to the PLC.
        LogHelper.DebugPrint($"Writing tag: {Name}");
        plctag.plc_tag_write(tagId, timeOut);
        rc = plctag.plc_tag_status(tagId);
        if (rc != 0)
        {
            string s = $"ERROR: Failed to write tag {Name} with code {rc}: {plctag.plc_tag_decode_error(rc)}";
            LogHelper.DebugPrint(s);
            throw new Exception(s);
        }

        // Destroy the tag
        plctag.plc_tag_destroy(tagId);

    }

    /// <summary>
    /// Returns a formatted string that represents the current object, including its name, CPU, program, tag, data type,
    /// bias, and unit of measure.
    /// </summary>
    /// <remarks>The returned string is intended for display or logging purposes and provides a human-readable
    /// summary of the object's key properties.</remarks>
    /// <returns>A multi-line string containing the values of the Name, CPU, Pgm, Tag, DataType, Bias, and UOM properties, each
    /// formatted for readability.</returns>
    [DebuggerStepThrough]
    public override string ToString()
    {
        if (string.IsNullOrEmpty(CPU) & string.IsNullOrEmpty(Pgm) & string.IsNullOrEmpty(Tag) &
            string.IsNullOrEmpty(DataType) & string.IsNullOrEmpty(Bias) & string.IsNullOrEmpty(UOM))
        {
            return $"Name: {Name} - All Fields Blank.";
        }

        return $"Name: {Name}\n\tCPU     : {CPU,20}\n\tPgm     : {Pgm,20}\n\tTag     : {Tag,20}\n\tDataType: {DataType,20}\n\tBias    : {Bias,20}\n\tUOM     : {UOM,20}";
    }

    /// <summary>
    /// Represents a sequentially laid out structure containing I/O data fields for ESEL16, designed for interoperability
    /// with unmanaged code.
    /// </summary>
    /// <remarks>This structure defines fixed-length fields for CPU, program, tag, data type, bias, and unit of
    /// measure information, along with their respective length indicators. It is intended for scenarios where precise
    /// memory layout and binary compatibility are required, such as interoperation with native APIs or binary
    /// serialization. The structure uses explicit marshaling attributes to ensure consistent field sizes and ordering in
    /// unmanaged memory.</remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct ESEL16_IODataUDT_Struct
    {
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public Int32 CPULen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string CPU;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public Int32 PgmLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Pgm;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public Int32 TagLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Tag;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public Int32 DataTypeLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string DataType;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public Int32 BiasLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string Bias;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public Int32 UOMLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string UOM;

        /// <summary>
        /// Returns a byte array that represents the current value of this instance.
        /// </summary>
        /// <remarks>The returned byte array reflects the unmanaged memory layout of the instance,
        /// including any padding or field ordering imposed by the structure's definition. This method is useful for
        /// interoperability scenarios where a raw memory representation is required, such as passing data to native
        /// APIs.</remarks>
        /// <returns>A byte array containing the serialized representation of this instance. The array length corresponds to the
        /// size of the structure in unmanaged memory.</returns>
        [DebuggerStepThrough]
        public readonly byte[] getBytes()
        {
            int size = Marshal.SizeOf(this);
            byte[] arr = new byte[size];

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(this, ptr, true);
                Marshal.Copy(ptr, arr, 0, size);
            }
            catch (Exception ex)
            {
                string s = $"ERROR: Failed to convert structure to bytes: {ex.Message}";
                LogHelper.DebugPrint(s);
                throw new Exception(s);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return arr;
        }

        /// <summary>
        /// Creates an instance of the ESEL16_IODataUDT_Struct type from its binary representation.
        /// </summary>
        /// <remarks>The method uses marshaling to convert the byte array into a structure. The caller
        /// must ensure that the byte array accurately represents a valid ESEL16_IODataUDT_Struct; otherwise, the
        /// resulting structure may contain invalid data. This method is intended for scenarios where binary
        /// serialization or interop with unmanaged code is required.</remarks>
        /// <param name="arr">A byte array containing the binary data to be converted into an ESEL16_IODataUDT_Struct instance. The array
        /// must contain at least as many bytes as the size of the structure.</param>
        /// <returns>An ESEL16_IODataUDT_Struct instance populated with data from the specified byte array.</returns>
        [DebuggerStepThrough]
        public readonly ESEL16_IODataUDT_Struct fromBytes(byte[] arr)
        {
            ESEL16_IODataUDT_Struct str = new ESEL16_IODataUDT_Struct();

            int size = Marshal.SizeOf(str);
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(size);

                Marshal.Copy(arr, 0, ptr, size);

                object? obj = Marshal.PtrToStructure(ptr, str.GetType());
                if (obj is ESEL16_IODataUDT_Struct safeStruct)
                {
                    str = safeStruct;
                }
            }
            catch (Exception ex)
            {
                string s = $"ERROR: Failed to convert bytes to structure: {ex.Message}";
                LogHelper.DebugPrint(s);
                throw new Exception(s);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return str;
        }

    }


}