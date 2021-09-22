using System.Collections.Generic;
using Infra.Models;

namespace dotNetCoreWithReact.Services
{
    public interface IGitHubStateRepository
    {
        public bool AddReposByOrg(string orgName, List<Repository> repos);

        public bool SetRepoProtectionState(string orgName, string repoName, bool newProtectionState);

        public IEnumerable<Repository> GetReposByOrg(string orgName);

        public bool isVisibilityStateChanged(bool currentState, bool changedState);

        public Repository GetSingleRepoByOrg(string repoFullname);
    }
}