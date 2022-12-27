using System.Net;

namespace ValheimPlayerManager.Web_Server
{
    public interface IWebEndpoint 
    {
        /// <summary>
        /// Get NAME of Endpoint
        /// </summary>
        /// <returns>NAME of Endpoint</returns>
        string GetEndpointName();

        /// <summary>
        /// Process Request Sent to Endpoint
        /// </summary>
        /// <param name="context">Http request context of client</param>
        void ProcessRequest(HttpListenerContext context);
    }
}
