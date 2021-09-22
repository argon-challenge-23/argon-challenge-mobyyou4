using System.Net;
using System.Threading.Tasks;

namespace Infra.Authentication
{
    public interface IBasicAuthClient
    {
        Task<(HttpStatusCode, string)> GetAsync(string APIAddress);
        public HttpStatusCode PatchAsync(string APIAddress, string body);
    }
}