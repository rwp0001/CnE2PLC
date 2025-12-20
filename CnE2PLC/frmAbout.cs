using CnE2PLC.Helpers;
using Microsoft.VisualBasic.ApplicationServices;
using System.Reflection;

namespace CnE2PLC;

partial class frmAbout : Form
{
    public frmAbout()
    {
        InitializeComponent();
        this.Text = $"About {AssemblyTitle}";
        this.lblProductName.Text = AssemblyProduct;
        this.lblVersion.Text = $"Version {AssemblyVersion}";
        this.lblCopyright.Text = AssemblyCopyright;
        this.lblGitVersion.Text = $"Git Version: {GitHelper.Version}";
        this.lblGitCommitID.Text = $"Commit ID: {GitHelper.CommitId}";
        this.lblGitBranch.Text = $"Git Branch: {GitHelper.Branch}";
        this.lblGitRepoLink.Text = $"Git Repo: {GitHelper.RepoURL}";
        this.lblGitBranchIsDirty.Text = $"{(GitHelper.IsDirty ? "Uncommitted Git Changes" : "")}";
        this.lblGitBranchIsDirty.Visible = GitHelper.IsDirty;
        this.textBoxDescription.Text = AssemblyDescription;
    }

    #region Assembly Attribute Accessors

    public string AssemblyTitle
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                if (titleAttribute.Title != "")
                {
                    return titleAttribute.Title;
                }
            }
            return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }
    }

    public string AssemblyVersion
    {
        get
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    public string AssemblyDescription
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }

    public string AssemblyProduct
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyProductAttribute)attributes[0]).Product;
        }
    }

    public string AssemblyCopyright
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    public string AssemblyCompany
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
    #endregion

    private void lblGitRepoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        var processStartInfo = new System.Diagnostics.ProcessStartInfo(GitHelper.RepoURL)
        {
            UseShellExecute = true // Ensures it opens in the default browser/app
        };
        System.Diagnostics.Process.Start(processStartInfo);
    }
}
