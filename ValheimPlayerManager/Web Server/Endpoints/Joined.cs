using Newtonsoft.Json;
using System.Net;
using ValheimPlayerManager.Discord;
using ValheimPlayerManager.Steam;

namespace ValheimPlayerManager.Web_Server.Endpoints
{
    public class Joined : IWebEndpoint
    {
        /// <summary>
        /// Name of the endpoint for the webserver
        /// </summary>
        public const string NAME = "/Join";

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
            //Set status code to successful
            response.StatusCode = (int)HttpStatusCode.OK;

            //Get request body json data
            string retreivedData = new StreamReader(context.Request.InputStream).ReadToEnd();

            //Check if the body contains data
            if (String.IsNullOrEmpty(retreivedData))
            {
                //If not send back bad request and close response
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Close();
                return;
            }

            //Deserialize json object from the request body
            Dictionary<string, string> bodyParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(retreivedData);

            //Check if Steam ID was provided in request body
            if (!bodyParams.ContainsKey("ID"))
            {
                //If not send back bad request and close response
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Close();
                return;
            }

            //Create new steam manager with API key
            SteamManager steamManager = new SteamManager();
            //Get steam username from ID
            string username = steamManager.GetUsernameFromID(bodyParams["ID"]);

            //Print connect message to log
            Console.WriteLine($"{bodyParams["ID"]} - {username} has Connected!");

            //Create new discord manager with webhook
            DiscordManager discordManager = new DiscordManager();
            //Send message to webhook
            discordManager.SendMessage($"{username} has Connected");

            //Close response
            response.Close();
        }
    }
}
