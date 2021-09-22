using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infra.HttpClients;
using Infra.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotNetCoreWithReact.Services
{
    public class GitHubHandler : IGitHubHandler
    {
        private readonly IGitHubStateRepository _stateRepository;
        private readonly IGitHubClient _gitHubClient;
        public GitHubHandler(IGitHubStateRepository stateRepository, IGitHubClient githubClint)
        {
            _stateRepository = stateRepository;
            _gitHubClient = githubClint;

        }
        public (HttpStatusCode status, string reason) RepoVisabilityChangedHandler(GitHubRepositoryWebHookEvent repoChanged)
        {
            GitHubRepositoryWebHookEvent repoVisibilityChanged = repoChanged;
            Repository repofromState = _stateRepository.GetSingleRepoByOrg(repoChanged.Repository.Full_Name);
            if (repofromState == null)
            {
                return (HttpStatusCode.NotFound, "changed repo didnt found in the server state");
            }

            Console.WriteLine($"repo visability was changed: repo:{repofromState.FullName} visability: {repofromState.Private}");
            if (repofromState.IsProtected)
            {
                if (_gitHubClient.SetRepoVisibility(repoChanged.Repository.Name, Convert.ToBoolean(!repoChanged.Repository.Private)))
                {
                    Console.WriteLine($"repo:{repofromState.FullName} visability: {repofromState.Private} was reverted");
                    return (HttpStatusCode.OK, "visibility change prevented");
                }
                else
                {
                    return (HttpStatusCode.InternalServerError, "couldn't prevent visibility change");
                }
            }
            else
            {
                repofromState.Private = repoChanged.Repository.Private;
                return (HttpStatusCode.OK, "repo is not in protection mode, hence the change is not reverted");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public async Task<(bool, IEnumerable<Repository>)> GetReposHandler(string orgName)
        {
            if (string.IsNullOrEmpty(orgName))
                throw new ArgumentNullException("OrgName is missing");

            //currently there is no support for repo visibility push update besides this get call, so we override the state at every get repo call
            var repos = await _gitHubClient.GetRepositories(orgName);
            if (repos == null)
            {
                return (false, null);
            }

            _stateRepository.AddReposByOrg(orgName, repos.ToList());
            _stateRepository.GetReposByOrg(orgName);
            return (true, repos.AsEnumerable());
        }
    }
}
