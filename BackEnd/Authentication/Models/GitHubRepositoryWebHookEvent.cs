using System.Text.Json.Serialization;

namespace Infra.Models
{
    /// <summary>
    /// all properties should be with private set except IsProtected, to prevent modified of readonly field
    /// </summary>
    public class GitHubRepositoryWebHookEvent
    {
        /// <summary>
        /// the repository Id
        /// </summary>
        [JsonPropertyName("repository")]
        public RepositoryWebHookEvent Repository { get; set; }

        public class RepositoryWebHookEvent
        {
            /// <summary>
            /// the repository Id
            /// </summary>
            [JsonPropertyName("id")]
            public int Id { get; set; }

            /// <summary>
            /// the repository name
            /// </summary>
            [JsonPropertyName("name")]
            public string Name { get; set; }

            /// <summary>
            /// the repository full name include the org
            /// </summary>
            [JsonPropertyName("full_name")]
            public string Full_Name { get; set; }

            /// <summary>
            /// true if the repo is in private mode
            /// </summary>
            [JsonPropertyName("private")]
            public bool Private { get; set; }
        }

    }
}
