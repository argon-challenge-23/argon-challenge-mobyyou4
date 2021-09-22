using System.Collections.Generic;
using System.Threading.Tasks;
using Infra.Models;

namespace Infra.HttpClients
{
    public interface IGitHubClient
    {
        public Task<List<Repository>> GetRepositories(string orgName);

        public bool SetRepoVisibility(string repoName, bool isPrivate);
    }
}