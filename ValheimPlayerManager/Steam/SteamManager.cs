using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using ValheimPlayerManager.Steam.Classes;

namespace ValheimPlayerManager.Steam
{
    /// <summary>
    /// Steam API Connection Manager
    /// </summary>
    public class SteamManager
    {
        //Create new Steam Manager
        public SteamManager() { }

        /// <summary>
        /// Get Steam Username from ID
        /// </summary>
        /// <param name="steamID">Steam ID to be looked up</param>
        /// <returns>Username of Steam ID</returns>
        public string GetUsernameFromID(string steamID)
        {
            //Create client for Steam API
            RestClient restClient = new RestClient("https://api.steampowered.com/");

            //Create new request to player API
            RestRequest request = new RestRequest("ISteamUser/GetPlayerSummaries/v2/");
            //Set API Key for connection
            request.AddParameter("key", Environment.GetEnvironmentVariable("STEAM_APIKEY"));
            //Set response format
            request.AddParameter("format", "json");
            //Add search steam ID
            request.AddParameter("steamids", steamID);

            //Perform get request to API
            RestResponse response = restClient.Get(request);

            //Deserialize response to player response
            SteamResponse playerResponse = JsonConvert.DeserializeObject<SteamResponse>(response.Content);

            //Return Username
            return playerResponse.Response.Players[0].Personaname;
        }
    }
}
