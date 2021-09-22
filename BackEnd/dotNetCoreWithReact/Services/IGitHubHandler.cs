using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Infra.Models;

namespace dotNetCoreWithReact.Services
{
    public interface IGitHubHandler
    {
        (HttpStatusCode status, string reason) RepoVisabilityChangedHandler(GitHubRepositoryWebHookEvent repoChanged);

        Task<(bool, IEnumerable<Repository>)> GetReposHandler(string orgName);
    }
}