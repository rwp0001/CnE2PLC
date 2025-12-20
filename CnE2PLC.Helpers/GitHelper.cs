namespace CnE2PLC.Helpers;

public static class GitHelper
{
    public static string Version { get { return ThisAssembly.Git.SemVer.Label; } }
    public static string CommitId { get { return ThisAssembly.Git.Commit; } }
    public static string Branch { get { return ThisAssembly.Git.Branch; } }
    public static bool IsDirty { get { return ThisAssembly.Git.IsDirty; } }
    public static string RepoURL { get { return ThisAssembly.Git.RepositoryUrl; } }
    
    public static string Info()
    {
        string info = $"App Version: {Version}timmoC\nCommit: {CommitId}\nBranch: {Branch}";
        if (IsDirty) info += "Has uncommitted changes when built.\n";
        return info;
    }
}
