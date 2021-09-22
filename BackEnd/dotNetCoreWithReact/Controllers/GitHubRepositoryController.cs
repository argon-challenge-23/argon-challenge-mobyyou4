using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dotNetCoreWithReact.Services;
using Infra.HttpClients;
using Infra.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace dotNetCoreWithReact.Controllers
{
    [EnableCors("Mypolicy")]
    [ApiController]
    public class GitHubRepositoryController : Controller
    {
        private readonly IGitHubClient _gitHubClient;
        private readonly IGitHubStateRepository _stateRepository;
        private readonly IGitHubHandler _gitHubHandler;

        public GitHubRepositoryController(IGitHubClient githubClinet, IGitHubStateRepository stateRepo, IGitHubHandler githubHandler)
        {
            _gitHubClient = githubClinet ?? throw new ArgumentNullException(nameof(githubClinet));
            _stateRepository = stateRepo ?? throw new ArgumentNullException(nameof(stateRepo));
            _gitHubHandler = githubHandler ?? throw new ArgumentNullException(nameof(githubHandler));
        }

        /// <summary>
        /// provide the repositories list for a given organization name
        /// </summary>
        /// <param name="orgName">the organization name</param>
        /// <returns></returns>
        [HttpGet("/api/org/{orgName}/repos")]
        public async Task<IActionResult> GetRepos(string orgName)
        {
            (bool res, IEnumerable<Repository> repos) = await _gitHubHandler.GetReposHandler(orgName);
            if (res)
            {
                return new JsonResult(repos);
            }
            return NotFound($"The orgName: {orgName} Not found");
        }

        [HttpPost("/api/org/{orgName}/repo/{repoName}")]
        public IActionResult SetProtectionState(string orgName, string repoName, bool isProtected)
        {
            bool success = _stateRepository.SetRepoProtectionState(orgName, repoName, isProtected);
            if (success)
            {
                return Ok();
            }
            return NotFound($"The orgName:{orgName}. repoName:{repoName} not found");
        }

        [HttpPost("/api/visibilityChanged")]
        public IActionResult RepoVisibilityChanged([FromBody] GitHubRepositoryWebHookEvent repoChanged)
        {
            try
            {
                if (repoChanged == null)
                {
                    // TODO: bunus validate shared secret
                    throw new ArgumentNullException($"repoChanged is null");
                }
                (HttpStatusCode status, string reason) = _gitHubHandler.RepoVisabilityChangedHandler(repoChanged);
                return new ObjectResult(reason) { StatusCode = (int)status };
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
