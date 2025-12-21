using System;
using System.Diagnostics;
using System.Text;

namespace CnE2PLC.Helpers;

public class UiTraceListener : TraceListener
{
    // Define an event that the UI can subscribe to.
    // The event arguments will carry the message string.
    public event EventHandler<string>? TraceOutput;

    // A buffer to hold partial writes until a newline is encountered.
    private StringBuilder _buffer = new();

    public override void Write(string? message)
    {
        // Append the message to the buffer.
        if (message != null) _buffer.Append(message);
    }

    public override void WriteLine(string? message)
    {
        // Append the final line of the message.
        if (message != null) _buffer.Append(message);

        // Raise the event with the complete message (and a newline for clarity).
        OnTraceOutput(_buffer.ToString() + Environment.NewLine);

        // Clear the buffer for the next message.
        _buffer.Clear();
    }

    // A helper method to safely raise the event.
    protected virtual void OnTraceOutput(string output)
    {
        TraceOutput?.Invoke(this, output);
    }
}
