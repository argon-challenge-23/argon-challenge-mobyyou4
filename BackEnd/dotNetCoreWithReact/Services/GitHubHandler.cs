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
            if (_stateRepository.isVisibilityStateChanged(repofromState.Private, repoVisibilityChanged.Repository.Private))
            {
                Console.WriteLine($"repo visability was changed: repo:{repofromState.FullName} visability: {repofromState.Private}");
                if (repofromState.IsProtected)
                {
                    if (_gitHubClient.SetRepoVisibility(repoChanged.Repository.Name, Convert.ToBoolean(repoChanged.Repository.Private)))
                    {
                        Console.WriteLine($"repo:{repofromState.FullName} visability: {repofromState.Private} was reverted");
                        return (HttpStatusCode.OK, "visibility change prevented");
                    }
                    else
                    {
                        return (HttpStatusCode.Conflict, "visibility didnt changed");
                    }
                }
                else
                {
                    return (HttpStatusCode.OK, "repo is not in protection mode, hence the change is not reverted");
                }

            }
            else
            {
                return (HttpStatusCode.Conflict, "visibility didnt changed");
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

            //try get repo from state
            var repos = _stateRepository.GetReposByOrg(orgName);
            if (repos == null)
            {
                //retrive repo from github api
                repos = await _gitHubClient.GetRepositories(orgName);

                //update state
                _stateRepository.AddReposByOrg(orgName, repos.ToList());
            }

            if (repos == null)
            {
                return (false, null);
            }
            return (true, repos.AsEnumerable());
        }
    }
}
