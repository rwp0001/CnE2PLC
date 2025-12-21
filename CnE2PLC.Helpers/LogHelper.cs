using System.Diagnostics;

namespace CnE2PLC.Helpers;

public static class LogHelper
{
    public static void DebugPrint(string message)
    {
        Debug.Print($"{DateTime.Now.ToString()}: {message}");
    }
}
