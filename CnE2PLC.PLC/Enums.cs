

namespace CnE2PLC.PLC;

#region Enums

/// <summary>
/// How is a tag used.
/// </summary>
public enum Usages
{
    Local,
    Input,
    Output,
    InOut,
    Public,
}

/// <summary>
/// Types of tags.
/// </summary>
public enum TagTypes
{
    Base,
    Alias,
    Produced,
    Consumed,
}

/// <summary>
/// How to display the data.
/// </summary>
public enum Radixes
{
    Decimal,
    Float,
    Hex,
    ASCII,
    Octal,
    Exponential,
    DateTime,
    DateTimeNS,
    NullType,
}

/// <summary>
/// Used to select the access allowed to the tag from external applications such as HMIs. 
/// </summary>
public enum Accesses
{
    ReadWrite,
    ReadOnly,
    None,
}

/// <summary>
/// Indicates whether the tag is a Standard tag or a Safety tag.
/// </summary>
public enum TagClasses
{
    Standard,
    Safety,
}

public enum FamilyTypes
{
    NoFamily,
    StringFamily,
}

public enum ClassTypes
{
    User,
}

public enum EKeyStates
{
    ExactMatch,
    CompatibleModule,
    Disabled,
}

public enum PortTypes
{
    Ethernet,
    Compact,
    ICP,
    Flex,
    None
}


#endregion