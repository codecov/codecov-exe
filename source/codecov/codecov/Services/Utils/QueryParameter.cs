using System.ComponentModel;

namespace codecov.Services.Utils
{
    public enum QueryParameter
    {
        [Description("branch")]
        Branch,

        [Description("commit")]
        Commit,

        [Description("build")]
        Build,

        [Description("job")]
        Job,

        [Description("pr")]
        Pr,

        [Description("slug")]
        Slug,

        [Description("service")]
        Service,

        [Description("tag")]
        Tag,

        [Description("build_url")]
        BuildUrl,

        [Description("yaml")]
        Yaml
    }
}