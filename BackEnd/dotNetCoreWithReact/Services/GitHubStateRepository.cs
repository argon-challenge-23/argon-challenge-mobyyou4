using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infra.Models;

namespace dotNetCoreWithReact.Services
{
    public class GitHubStateRepository : IGitHubStateRepository
    {
        /// <summary>
        /// private orgs state
        /// the data structure consisnt of nested dictionary to allow optimal search in org level
        /// and in repo level
        /// </summary>
        private readonly static Dictionary<string, Dictionary<string, Repository>> _orgs
            = new Dictionary<string, Dictionary<string, Repository>>();

        /// <summary>
        /// accessable orgs state to handle protection state of the repos
        /// </summary>
        private Dictionary<string, Dictionary<string, Repository>> Orgs { get => _orgs; }

        public GitHubStateRepository()
        {

        }

        /// <summary>
        /// add new repo's org
        /// </summary>
        /// <param name="orgName"></param>
        /// <param name="repos"></param>
        public bool AddReposByOrg(string orgName, List<Repository> repos)
        {
            if (!_orgs.ContainsKey(orgName))
            {
                _orgs.Add(orgName, repos.ToDictionary(r => r.Name, r => r));
                return true;
            }
            else
            {
                repos.ForEach(r => {
                    if (_orgs[orgName].ContainsKey(r.Name))
                    {
                        _orgs[orgName][r.Name].Private = r.Private;
                    }
                });
                
                
            }
            return false;
        }

        /// <summary>
        /// set the protection state of repo by the org name and repo name
        /// </summary>
        /// <param name="orgName"></param>
        /// <param name="repoName"></param>
        /// <param name="newProtectionState"></param>
        public bool SetRepoProtectionState(string orgName, string repoName, bool newProtectionState)
        {
            if (Orgs.TryGetValue(orgName, out Dictionary<string, Repository> matchedOrg))
            {
                if (matchedOrg.TryGetValue(repoName, out Repository matchedRepo))
                {
                    matchedRepo.IsProtected = newProtectionState;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgName"></param>
        /// <returns></returns>
        public IEnumerable<Repository> GetReposByOrg(string orgName)
        {
            if (Orgs.TryGetValue(orgName, out Dictionary<string, Repository> repos))
            {
                return repos.Select(r => r.Value).ToList();
            }

            return null;
        }


        public Repository GetSingleRepoByOrg(string repoFullname)
        {
            if(string.IsNullOrEmpty(repoFullname))
            {
                return null;
            }

            string[] repoTokens = repoFullname.Split('/');
            if (repoTokens.Length != 2)
            {
                throw new ArgumentException($"GitHubStateRepository::GetSingleRepoByOrg: invalid parameter {repoFullname}");
            }
            string orgName = repoTokens[0];
            string repoName = repoTokens[1];

            if (Orgs.TryGetValue(orgName, out Dictionary<string, Repository> repos))
            {
                var res = repos.First(r => r.Key == repoName);
                if (res.Key != string.Empty)
                {
                    return res.Value;
                }
            }
            return null;
        }

        public bool isVisibilityStateChanged(bool currentState, bool changedState)
        {
            return currentState != changedState;
        }
    }
}
