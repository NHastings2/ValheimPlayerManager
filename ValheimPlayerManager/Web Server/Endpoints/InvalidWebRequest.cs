using System.Net;
using System.Text;

namespace ValheimPlayerManager.Web_Server.Endpoints
{
    class InvalidWebRequest : IWebEndpoint
    {
        /// <summary>
        /// Name of the endpoint for the webserver
        /// </summary>
        public const string NAME = "/InvalidWebRequest";

        /// <summary>
        /// Return the name of the endpoint for the interface
        /// </summary>
        /// <returns>NAME of endpoint</returns>
        public string GetEndpointName()
        {
            return NAME;
        }

        /// <summary>
        /// Process request sent to endpoint
        /// </summary>
        /// <param name="context">Context of client request</param>
        public void ProcessRequest(HttpListenerContext context)
        {
            //Get response object for request
            HttpListenerResponse response = context.Response;
            //Set status code to Not Found
            response.StatusCode = (int)HttpStatusCode.NotFound;

            //Set message for response
            string message = "Could Not Find Resource";
            //Encode data to byte array
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            //Send message to client
            response.OutputStream.Write(messageBytes, 0, message.Length);

            //Close response
            response.Close();
        }
    }
}
