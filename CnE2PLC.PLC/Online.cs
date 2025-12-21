using libplctag.DataTypes;
using libplctag;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CnE2PLC.PLC;

internal class Online : INotifyPropertyChanged
{
    public Online() { }

    public Online(string IP_Address)
    {
        Gateway = IP_Address;
    }

    #region Public Properties
    public string Gateway
    {
        get { return gateway; }
        set
        {
            gateway = value;
            ClearCaches();
            NotifyPropertyChanged();
        }
    }

    public string Path
    {
        get { return path; }
        set
        {
            path = value;
            ClearCaches();
            NotifyPropertyChanged();
        }
    }

    public int TagCount
    {
        get
        {
            if (tags == null) return 0;
            return tags.Value.Count();
        }
    }

    public int UDTCount
    {
        get
        {
            if (uniqueUdtTypeIds == null) return 0;
            return uniqueUdtTypeIds.Count();
        }
    }

    public Tag<TagInfoPlcMapper, TagInfo[]> Tags
    {
        get
        {
            if (tags == null)
            {
                tags = new Tag<TagInfoPlcMapper, TagInfo[]>()
                {
                    Gateway = this.Gateway,
                    Path = this.Path,
                    PlcType = this.plcType,
                    Protocol = this.protocol,
                    Name = "@tags"
                };
                tags.Read();
                if (tags.GetStatus() != Status.Ok)
                {
                    connected = false;
                    throw new Exception(($"PLC did not respond."));
                }
                Connected = true;
                NotifyPropertyChanged();
            }
            return tags;
        }
    }

    public Dictionary<int, string> UDT_ID_Pairs
    {
        get
        {
            if (udt_id_pairs == null)
            {
                udt_id_pairs = new Dictionary<int, string>();
                foreach (var udtId in UniqueUdtTypeIds)
                {
                    var udtTag = new Tag<UdtInfoPlcMapper, UdtInfo>()
                    {
                        Gateway = this.Gateway,
                        Path = this.Path,
                        PlcType = this.plcType,
                        Protocol = this.protocol,
                        Name = $"@udt/{udtId}",
                    };

                    udtTag.Read();

                    if (udtTag.GetStatus() != Status.Ok)
                    {
                        connected = false;
                        throw new Exception(($"PLC did not respond."));
                    }

                    UdtInfo udt = udtTag.Value;
                    udt_id_pairs.Add(udtId, udt.Name);
                    NotifyPropertyChanged();
                }
                Connected = true;
            }
            return udt_id_pairs;
        }
    }

    public IEnumerable<int> UniqueUdtTypeIds
    {
        get
        {
            if (uniqueUdtTypeIds == null)
            {
                uniqueUdtTypeIds = Tags.Value
               .Where(tagInfo => TagIsUdt(tagInfo))
               .Select(tagInfo => GetUdtId(tagInfo))
               .Distinct();
                NotifyPropertyChanged();
            }
            return uniqueUdtTypeIds;
        }
    }

    public bool Connected
    {
        get { return connected; }
        set
        {
            connected = value;
            NotifyPropertyChanged();
        }
    }

    public bool GetBoolTag(string tag)
    {
        return false;
    }

    public void SetBoolTag(string tag, bool value)
    {

    }


    #endregion

    #region Private Flieds

    private string gateway = "192.168.250.250";
    private string path = "1,2";
    private PlcType plcType = PlcType.ControlLogix;
    private Protocol protocol = Protocol.ab_eip;

    private Tag<TagInfoPlcMapper, TagInfo[]>? tags;
    private IEnumerable<int>? uniqueUdtTypeIds;
    private Dictionary<int, string>? udt_id_pairs;
    private bool connected = false;

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    static bool TagIsUdt(TagInfo tag)
    {
        const ushort TYPE_IS_STRUCT = 0x8000;
        const ushort TYPE_IS_SYSTEM = 0x1000;
        return ((tag.Type & TYPE_IS_STRUCT) != 0) && !((tag.Type & TYPE_IS_SYSTEM) != 0);
    }

    static int GetUdtId(TagInfo tag)
    {
        const ushort TYPE_UDT_ID_MASK = 0x0FFF;
        return tag.Type & TYPE_UDT_ID_MASK;
    }

    static bool TagIsProgram(TagInfo tag, out string prefix)
    {
        if (tag.Name.StartsWith("Program:"))
        {
            prefix = tag.Name;
            return true;
        }
        else
        {
            prefix = string.Empty;
            return false;
        }
    }

    public string GetUDTName(ushort id)
    {
        string MSG = id.ToString();
        if (UDT_ID_Pairs.ContainsKey(id))
        {
            return UDT_ID_Pairs[id];
        }
        return MSG;
    }

    public string PrintTags()
    {
        string MSG = "";
        var TagList = Tags.Value;
        UDT_ID_Pairs.Count();

        foreach (var f in TagList)
        {
            MSG += ($"Name={f.Name}");

            if (UDT_ID_Pairs.ContainsKey(f.Type))
            {
                MSG += ($" Type={UDT_ID_Pairs[f.Type]}");
            }
            else
            {
                MSG += ($" Type={f.Type}");
            }

            MSG += "\n";
        }
        return MSG;
    }

    public string PrintUDTs()
    {
        string MSG = "";
        foreach (var udtId in UniqueUdtTypeIds)
        {
            try
            {
                var udtTag = new Tag<UdtInfoPlcMapper, UdtInfo>()
                {
                    Gateway = this.Gateway,
                    Path = this.Path,
                    PlcType = this.plcType,
                    Protocol = this.protocol,
                    Name = $"@udt/{udtId}",
                };
                udtTag.Read();
                UdtInfo udt = udtTag.Value;
                MSG = MSG + ($"UDT Id={udt.Id} Name={udt.Name} NumFields={udt.NumFields} Size={udt.Size}\n");
                foreach (var f in udt.Fields) MSG = MSG + ($"\tName={f.Name} Offset={f.Offset} Metadata={f.Metadata} Type={f.Type}\n");
            }
            catch (Exception e)
            {
                MSG += ($"\n\tException: {e.Message}\n");
            }
        }
        return MSG;
    }

    private void ClearCaches()
    {
        connected = false;
        tags = null;
        uniqueUdtTypeIds = null;
        udt_id_pairs = null;
    }

    public override string ToString()
    {
        return ($"PLC at {this.Gateway} - Tags: {this.TagCount} - UDTs: {this.UDTCount}");
    }

}
