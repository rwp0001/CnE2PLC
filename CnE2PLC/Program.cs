using CnE2PLC.Helpers;
using System.Diagnostics;

namespace CnE2PLC;

internal static class Program
{
    public static UiTraceListener UITraceListener = new();

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Trace.Listeners.Add( UITraceListener );
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new frmMain());
    }

}