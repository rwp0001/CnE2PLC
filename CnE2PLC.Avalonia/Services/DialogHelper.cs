using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace CnE2PLC.Avalonia.Services;

public static class DialogHelper
{
    public static async Task ShowMessage(string title, string message, Window owner)
    {
        var okButton = new Button
        {
            Content = "OK",
            HorizontalAlignment = HorizontalAlignment.Center,
            MinWidth = 80
        };

        var panel = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 16,
            Children =
            {
                new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 360
                },
                okButton
            }
        };

        var dialog = new Window
        {
            Title = title,
            Width = 420,
            SizeToContent = SizeToContent.Height,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = panel
        };

        okButton.Click += (_, _) => dialog.Close();
        await dialog.ShowDialog(owner);
    }
}
