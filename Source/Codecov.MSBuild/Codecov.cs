using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codecov.Coverage.EnviornmentVariables;
using Codecov.Coverage.Report;
using Codecov.Coverage.SourceCode;
using Codecov.Coverage.Tool;
using Codecov.Factories;
using Codecov.Program;
using Codecov.Services.VersionControlSystems;
using Codecov.Upload;
using Codecov.Url;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Serilog;
using Serilog.Sinks.MSBuild;

namespace Codecov.MSBuild
{
    /// <summary>
    /// MSBuild task for uploading coverage reports to Codecov.
    /// </summary>
    public class Codecov : Task, IEnviornmentVariablesOptions, IHostOptions, IQueryOptions, IReportOptions, IVersionControlSystemOptions
    {
        /// <summary>
        /// Gets or sets the report files to upload.
        /// </summary>
        [Required]
        public ITaskItem[] ReportFiles { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the environment variables to be included with this build.
        /// </summary>
        public string[] EnvironmentVariables { get; set; } = new string[0];

        /// <summary>
        /// Gets or sets a value used when not in git/hg project to identify project root directory.
        /// </summary>
        public string RepoRoot { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the target url for Enterprise customers.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a property specifying the branch name.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets a property specifying the build number.
        /// </summary>
        public string Build { get; set; }

        /// <summary>
        /// Gets or sets a property specifying the commit sha.
        /// </summary>
        public string Commit { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the flag the upload to group coverage metrics. See https://docs.codecov.io/docs/flags
        /// </summary>
        public string Flags { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the custom defined name of the upload. Visible in Codecov UI.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the pull request number.
        /// </summary>
        public string Pr { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the owner/repo slug used instead of the private repo token in Enterprise.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the git tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the private repository token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the features to toggle on/off.
        /// </summary>
        public string[] Features { get; set; } = new string[0];

        /// <summary>
        /// Gets or sets a value disabling the upload of the file network.
        /// </summary>
        public bool DisableNetwork { get; set; }

        /// <summary>
        /// Gets or sets the URL at which the uploaded report can be viewed.
        /// </summary>
        [Output]
        public string ReportUrl { get; set; }

        /// <summary>
        /// Execute the task.
        /// </summary>
        /// <returns><c>true</c> if the task executed successfully; otherwise, <c>false</c>.</returns>
        public override bool Execute()
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty(MSBuildProperties.Subcategory, nameof(Codecov))
                .WriteTo.MSBuild(this)
                .CreateLogger();
            try
            {
                var reportFiles = ReportFiles.Select(e => new ReportFile(e.ItemSpec, File.ReadAllText(e.ItemSpec)));
                var coverage = new Coverage(reportFiles);
                var envVars = new EnviornmentVariables(this);
                var continuousIntegrationServer = ContinuousIntegrationServerFactory.Create(envVars);
                envVars.LoadEnviornmentVariables(continuousIntegrationServer);
                var versionControlSystem = VersionControlSystemFactory.Create(this, new Terminal.Terminal());
                var sourceCode = new SourceCode(versionControlSystem);
                var yaml = new Yaml.Yaml(sourceCode);
                var repositories = RepositoryFactory.Create(versionControlSystem, continuousIntegrationServer);
                var url = new Url.Url(new Host(this, envVars), new Route(), new Query(this, repositories, continuousIntegrationServer, yaml, envVars));
                var report = new Report(this, envVars, sourceCode, coverage);
                var upload = new Uploads(url, report, Features);
                var uploadFacade = new UploadFacade(continuousIntegrationServer, versionControlSystem, yaml, coverage, envVars, url, upload);
                uploadFacade.LogContinuousIntegrationAndVersionControlSystem();
                ReportUrl = uploadFacade.UploadReports();
                return true;
            }
            catch (Exception exception)
            {
                Log.LogError("The Codecov MSBuild task failed.");
                Log.LogErrorFromException(exception, showStackTrace: true);
                return false;
            }
        }

        IEnumerable<string> IEnviornmentVariablesOptions.Envs => EnvironmentVariables;
    }
}
