using System.Diagnostics;

namespace CnE2PLC.Helpers;

public static class LogHelper
{
    [DebuggerStepThrough]
    public static void DebugPrint(string message)
    {
        Debug.Print($"{DateTime.Now.ToString()}: {message}");
    }
}
