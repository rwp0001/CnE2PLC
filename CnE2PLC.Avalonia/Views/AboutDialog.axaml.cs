using Avalonia.Controls;
using Avalonia.Interactivity;
using CnE2PLC.Helpers;
using System.Diagnostics;
using System.Reflection;

namespace CnE2PLC.Avalonia.Views;

public partial class AboutDialog : Window
{
    public AboutDialog()
    {
        InitializeComponent();

        var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

        ProductNameText.Text = asm.GetCustomAttribute<AssemblyProductAttribute>()?.Product
                               ?? asm.GetName().Name ?? "CnE Toolbox";
        VersionText.Text     = $"Version {asm.GetName().Version}";
        CopyrightText.Text   = asm.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? "";
        GitVersionText.Text  = $"Git Version: {GitHelper.Version}";
        CommitIdText.Text    = $"Commit ID: {GitHelper.CommitId}";
        BranchText.Text      = $"Git Branch: {GitHelper.Branch}";
        RepoLinkButton.Content = $"Git Repo: {GitHelper.RepoURL}";
        DescriptionText.Text = asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";

        if (GitHelper.IsDirty)
        {
            DirtyText.Text      = "Uncommitted Git Changes";
            DirtyText.IsVisible = true;
        }
    }

    private void RepoLink_Click(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo(GitHelper.RepoURL) { UseShellExecute = true });
    }
}

