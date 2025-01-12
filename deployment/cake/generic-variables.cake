#l "buildserver.cake"

//-------------------------------------------------------------

public class GeneralContext : BuildContextWithItemsBase
{
    public GeneralContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public string Target { get; set; }
    public string RootDirectory { get; set; }
    public string OutputRootDirectory { get; set; }

    public bool IsCiBuild { get; set; }
    public bool IsAlphaBuild { get; set; }
    public bool IsBetaBuild { get; set; }
    public bool IsOfficialBuild { get; set; }
    public bool IsLocalBuild { get; set; }
    public bool UseVisualStudioPrerelease { get; set; }
    public bool VerifyDependencies { get; set; }

    public VersionContext Version { get; set; }
    public CopyrightContext Copyright { get; set; }
    public NuGetContext NuGet { get; set; }
    public SolutionContext Solution { get; set; }
    public SourceLinkContext SourceLink { get; set; }
    public CodeSignContext CodeSign { get; set; }
    public RepositoryContext Repository { get; set; }
    public SonarQubeContext SonarQube { get; set; }

    public List<string> Includes { get; set; }
    public List<string> Excludes { get; set; }

    protected override void ValidateContext()
    {
    }
    
    protected override void LogStateInfoForContext()
    {
        CakeContext.Information($"Running target '{Target}'");
        CakeContext.Information($"Using output directory '{OutputRootDirectory}'");
    }
}

//-------------------------------------------------------------

public class VersionContext : BuildContextBase
{
    private GitVersion _gitVersionContext;

    public VersionContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public GitVersion GetGitVersionContext(GeneralContext generalContext)
    {
        if (_gitVersionContext is null)
        {
            var gitVersionSettings = new GitVersionSettings
            {
                UpdateAssemblyInfo = false,
                Verbosity = GitVersionVerbosity.Debug
            };

            var gitDirectory = ".git";
            if (!CakeContext.DirectoryExists(gitDirectory))
            {
                CakeContext.Information("No local .git directory found, treating as dynamic repository");

                //// TEMP CODE - START
                //
                //CakeContext.Warning("Since dynamic repositories do not yet work correctly, we clear out the cloned temp directory (which is slow, but should be fixed in 5.0 beta)");
                //
                //// Make a *BIG* assumption that the solution name == repository name
                //var repositoryName = generalContext.Solution.Name;
                //var tempDirectory = $"{System.IO.Path.GetTempPath()}\\{repositoryName}";
                //
                //if (CakeContext.DirectoryExists(tempDirectory))
                //{
                //    CakeContext.DeleteDirectory(tempDirectory, new DeleteDirectorySettings
                //    {
                //        Force = true,
                //        Recursive = true
                //    });
                //}
                //
                //// TEMP CODE - END

                // Dynamic repository
                gitVersionSettings.UserName = generalContext.Repository.Username;
                gitVersionSettings.Password = generalContext.Repository.Password;
                gitVersionSettings.Url = generalContext.Repository.Url;
                gitVersionSettings.Branch = generalContext.Repository.BranchName;
                gitVersionSettings.Commit = generalContext.Repository.CommitId;
                gitVersionSettings.NoFetch = false;
                gitVersionSettings.WorkingDirectory = generalContext.RootDirectory;
            }

            _gitVersionContext = CakeContext.GitVersion(gitVersionSettings);
        }

        return _gitVersionContext;
    }

    public string MajorMinorPatch { get; set; }
    public string FullSemVer { get; set; }
    public string NuGet { get; set; }
    public string CommitsSinceVersionSource { get; set; }

    protected override void ValidateContext()
    {
    
    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

public class CopyrightContext : BuildContextBase
{
    public CopyrightContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public string Company { get; set; }
    public string StartYear { get; set; }

    protected override void ValidateContext()
    {
        if (string.IsNullOrWhiteSpace(Company))
        {
            throw new Exception($"Company must be defined");
        }    
    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

public class NuGetContext : BuildContextBase
{
    public NuGetContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public string PackageSources { get; set; }
    public string Executable { get; set; }
    public string LocalPackagesDirectory { get; set; }

    protected override void ValidateContext()
    {
    
    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

public class SolutionContext : BuildContextBase
{
    public SolutionContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public string Name { get; set; }
    public string AssemblyInfoFileName { get; set; }
    public string FileName { get; set; }

    public string PublishType { get; set; }
    public string ConfigurationName { get; set; }

    protected override void ValidateContext()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new Exception($"SolutionName must be defined");
        }
    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

public class SourceLinkContext : BuildContextBase
{
    public SourceLinkContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public bool IsDisabled { get; set; }

    protected override void ValidateContext()
    {
    
    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

public class CodeSignContext : BuildContextBase
{
    public CodeSignContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public string WildCard { get; set; }
    public string CertificateSubjectName { get; set; }
    public string TimeStampUri { get; set; }

    protected override void ValidateContext()
    {
    
    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

public class RepositoryContext : BuildContextBase
{
    public RepositoryContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public string Url  { get; set; }
    public string BranchName  { get; set; }
    public string CommitId  { get; set; }
    public string Username  { get; set; }
    public string Password  { get; set; }

    protected override void ValidateContext()
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            throw new Exception($"RepositoryUrl must be defined");
        }
    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

public class SonarQubeContext : BuildContextBase
{
    public SonarQubeContext(IBuildContext parentBuildContext)
        : base(parentBuildContext)
    {
    }

    public bool IsDisabled { get; set; }
    public string Url { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Project { get; set; }

    protected override void ValidateContext()
    {

    }
    
    protected override void LogStateInfoForContext()
    {
    
    }
}

//-------------------------------------------------------------

private GeneralContext InitializeGeneralContext(BuildContext buildContext, IBuildContext parentBuildContext)
{
    var data = new GeneralContext(parentBuildContext)
    {
        Target = buildContext.BuildServer.GetVariable("Target", "Default", showValue: true),
    };

    data.Version = new VersionContext(data)
    {
        MajorMinorPatch = buildContext.BuildServer.GetVariable("GitVersion_MajorMinorPatch", "unknown", showValue: true),
        FullSemVer = buildContext.BuildServer.GetVariable("GitVersion_FullSemVer", "unknown", showValue: true),
        NuGet = buildContext.BuildServer.GetVariable("GitVersion_NuGetVersion", "unknown", showValue: true),
        CommitsSinceVersionSource = buildContext.BuildServer.GetVariable("GitVersion_CommitsSinceVersionSource", "unknown", showValue: true)
    };

    data.Copyright = new CopyrightContext(data)
    {
        Company = buildContext.BuildServer.GetVariable("Company", showValue: true),
        StartYear = buildContext.BuildServer.GetVariable("StartYear", showValue: true)
    };

    data.NuGet = new NuGetContext(data)
    {
        PackageSources = buildContext.BuildServer.GetVariable("NuGetPackageSources", showValue: true),
        Executable = "./tools/nuget.exe",
        LocalPackagesDirectory = "c:\\source\\_packages"
    };

    var solutionName = buildContext.BuildServer.GetVariable("SolutionName", showValue: true);

    data.Solution = new SolutionContext(data)
    {
        Name = solutionName,
        AssemblyInfoFileName = "./src/SolutionAssemblyInfo.cs",
        FileName = string.Format("./src/{0}", string.Format("{0}.sln", solutionName)),
        PublishType = buildContext.BuildServer.GetVariable("PublishType", "Unknown", showValue: true),
        ConfigurationName = buildContext.BuildServer.GetVariable("ConfigurationName", "Release", showValue: true)
    };

    data.IsCiBuild = buildContext.BuildServer.GetVariableAsBool("IsCiBuild", false, showValue: true);
    data.IsAlphaBuild = buildContext.BuildServer.GetVariableAsBool("IsAlphaBuild", false, showValue: true);
    data.IsBetaBuild = buildContext.BuildServer.GetVariableAsBool("IsBetaBuild", false, showValue: true);
    data.IsOfficialBuild = buildContext.BuildServer.GetVariableAsBool("IsOfficialBuild", false, showValue: true);
    data.IsLocalBuild = data.Target.ToLower().Contains("local");
    data.UseVisualStudioPrerelease = buildContext.BuildServer.GetVariableAsBool("UseVisualStudioPrerelease", false, showValue: true);
    data.VerifyDependencies = !buildContext.BuildServer.GetVariableAsBool("DependencyCheckDisabled", false, showValue: true);

    // If local, we want full pdb, so do a debug instead
    if (data.IsLocalBuild)
    {
        parentBuildContext.CakeContext.Warning("Enforcing configuration 'Debug' because this is seems to be a local build, do not publish this package!");
        data.Solution.ConfigurationName = "Debug";
    }

    // Important: do *after* initializing the configuration name
    data.RootDirectory = System.IO.Path.GetFullPath(".");
    data.OutputRootDirectory = System.IO.Path.GetFullPath(buildContext.BuildServer.GetVariable("OutputRootDirectory", string.Format("./output/{0}", data.Solution.ConfigurationName), showValue: true));

    data.SourceLink = new SourceLinkContext(data)
    {
        IsDisabled = buildContext.BuildServer.GetVariableAsBool("SourceLinkDisabled", false, showValue: true)
    };

    data.CodeSign = new CodeSignContext(data)
    {
        WildCard = buildContext.BuildServer.GetVariable("CodeSignWildcard", showValue: true),
        CertificateSubjectName = buildContext.BuildServer.GetVariable("CodeSignCertificateSubjectName", data.Copyright.Company, showValue: true),
        TimeStampUri = buildContext.BuildServer.GetVariable("CodeSignTimeStampUri", "http://timestamp.comodoca.com/authenticode", showValue: true)
    };

    data.Repository = new RepositoryContext(data)
    {
        Url = buildContext.BuildServer.GetVariable("RepositoryUrl", showValue: true),
        BranchName = buildContext.BuildServer.GetVariable("RepositoryBranchName", showValue: true),
        CommitId = buildContext.BuildServer.GetVariable("RepositoryCommitId", showValue: true),
        Username = buildContext.BuildServer.GetVariable("RepositoryUsername", showValue: false),
        Password = buildContext.BuildServer.GetVariable("RepositoryPassword", showValue: false)
    };

    data.SonarQube = new SonarQubeContext(data)
    {
        IsDisabled = buildContext.BuildServer.GetVariableAsBool("SonarDisabled", false, showValue: true),
        Url = buildContext.BuildServer.GetVariable("SonarUrl", showValue: true),
        Username = buildContext.BuildServer.GetVariable("SonarUsername", showValue: false),
        Password = buildContext.BuildServer.GetVariable("SonarPassword", showValue: false),
        Project = buildContext.BuildServer.GetVariable("SonarProject", data.Solution.Name, showValue: true)
    };

    data.Includes = SplitCommaSeparatedList(buildContext.BuildServer.GetVariable("Include", string.Empty, showValue: true));
    data.Excludes = SplitCommaSeparatedList(buildContext.BuildServer.GetVariable("Exclude", string.Empty, showValue: true));

    // Specific overrides, done when we have *all* info
    parentBuildContext.CakeContext.Information("Ensuring correct runtime data based on version");

    var versionContext = data.Version;
    if (string.IsNullOrWhiteSpace(versionContext.NuGet) || versionContext.NuGet == "unknown")
    {
        parentBuildContext.CakeContext.Information("No version info specified, falling back to GitVersion");

        var gitVersion = versionContext.GetGitVersionContext(data);
        
        versionContext.MajorMinorPatch = gitVersion.MajorMinorPatch;
        versionContext.FullSemVer = gitVersion.FullSemVer;
        versionContext.NuGet = gitVersion.NuGetVersionV2;
        versionContext.CommitsSinceVersionSource = (gitVersion.CommitsSinceVersionSource ?? 0).ToString();
    }    

    parentBuildContext.CakeContext.Information("Defined version: '{0}', commits since version source: '{1}'", versionContext.FullSemVer, versionContext.CommitsSinceVersionSource);

    if (string.IsNullOrWhiteSpace(data.Repository.CommitId))
    {
        parentBuildContext.CakeContext.Information("No commit id specified, falling back to GitVersion");

        var gitVersion = versionContext.GetGitVersionContext(data);
        
        data.Repository.CommitId = gitVersion.Sha;
    }

    var versionToCheck = versionContext.FullSemVer;
    if (versionToCheck.Contains("alpha"))
    {
        data.IsAlphaBuild = true;
    }
    else if (versionToCheck.Contains("beta"))
    {
        data.IsBetaBuild = true;
    }
    else
    {
        data.IsOfficialBuild = true;
    }

    return data;
}

//-------------------------------------------------------------

private static string DetermineChannel(GeneralContext context)
{
    var version = context.Version.FullSemVer;

    var channel = "stable";

    if (context.IsAlphaBuild)
    {
        channel = "alpha";
    }
    else if (context.IsBetaBuild)
    {
        channel = "beta";
    }

    return channel;
}

//-------------------------------------------------------------

private static string DeterminePublishType(GeneralContext context)
{
    var publishType = "Unknown";

    if (context.IsOfficialBuild)
    {
        publishType = "Official";
    }
    else if (context.IsBetaBuild)
    {
        publishType = "Beta";
    }
    else if (context.IsAlphaBuild)
    {
        publishType = "Alpha";
    }
    
    return publishType;
}