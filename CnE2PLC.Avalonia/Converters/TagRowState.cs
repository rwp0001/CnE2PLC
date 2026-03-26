namespace CnE2PLC.Avalonia.Converters;

/// <summary>
/// Semantic display state for a tag row. Priority order: Error > Alarm > Warning > Disabled > Default.
/// </summary>
public enum TagRowState
{
    Default,
    Disabled,   // Placeholder, AOICalls == 0, NotInUse
    Warning,    // Hi/Lo alarm active
    Alarm,      // HiHi/LoLo alarm, DI alarm, interlock bypass active
    Error,      // Bad PV, simmed, interlock tripped
}
