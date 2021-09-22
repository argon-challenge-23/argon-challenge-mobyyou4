using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Infra.Authentication;
using Infra.Models;

namespace Infra.HttpClients
{
    public class GitHubClient : IGitHubClient
    {
        /// <summary>
        /// the base github api uri
        /// </summary>
        const string GitHubApiAddress = "https://api.github.com";

        private readonly IBasicAuthClient _httpClient;
        public GitHubClient(IBasicAuthClient authProvider)
        {
            _httpClient = authProvider;
        }

        public GitHubClient()
        {
            _httpClient = new BasicAuthClient();
        }

        public async Task<List<Repository>> GetRepositories(string orgName)
        {
            (HttpStatusCode status, string response) = await _httpClient.GetAsync($"{GitHubApiAddress}/orgs/{orgName}/repos");
            try
            {
                switch (status)
                {
                    case HttpStatusCode.OK:
                        return JsonSerializer.Deserialize<List<Repository>>(response);
                    default:
                        throw new Exception($"GitHubClient::GetRepositories: bad status code {status}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"exception in GitHubClient:GetRepositories{ e.Message}");
            }
            return null;
        }

        public bool SetRepoVisibility(string repoName, bool isPrivate)
        {
            try
            {
                string body = "{\"private\": " + isPrivate.ToString().ToLower() + "}";
                HttpStatusCode response = _httpClient.PatchAsync($"{GitHubApiAddress}/repos/argon-challenge-23/{repoName}", body);

                //Im using http status code for the ability to handle multiple http status codes
                switch (response)
                {
                    case HttpStatusCode.OK:
                        return true;
                    default:
                        return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"exception in GitHubClient:SetRepoVisibility{ e.Message}");
            }
            return false;
        }

    }
}