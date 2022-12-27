using RestSharp;

namespace ValheimPlayerManager.Discord
{
    /// <summary>
    /// Discord manager to send discord messages
    /// </summary>
    public class DiscordManager
    {
        /// <summary>
        /// Create new Discord manager
        /// </summary>
        public DiscordManager() { }

        /// <summary>
        /// Send message to discord webhook
        /// </summary>
        /// <param name="message">Message to be sent</param>
        public void SendMessage(string message)
        {
            //Create new rest client with webhook
            RestClient restClient = new RestClient(Environment.GetEnvironmentVariable("DISCORD_WEBHOOK"));

            //Create new request
            RestRequest restRequest = new RestRequest();
            //Add json body to request
            restRequest.AddJsonBody(new Dictionary<string, string>()
            {
                { "username", "Valheim" },
                { "content", message }
            });

            //Post request to Discord webhook
            restClient.Post(restRequest);
        }
    }
}
