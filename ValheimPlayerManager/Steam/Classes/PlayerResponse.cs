using Newtonsoft.Json;

namespace ValheimPlayerManager.Steam.Classes
{
    public partial class SteamResponse
    {
        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public partial class Response
    {
        [JsonProperty("players")]
        public List<Player> Players { get; set; }
    }

    public partial class Player
    {
        [JsonProperty("steamid")]
        public string Steamid { get; set; }

        [JsonProperty("communityvisibilitystate")]
        public long Communityvisibilitystate { get; set; }

        [JsonProperty("profilestate")]
        public long Profilestate { get; set; }

        [JsonProperty("personaname")]
        public string Personaname { get; set; }

        [JsonProperty("profileurl")]
        public Uri Profileurl { get; set; }

        [JsonProperty("avatar")]
        public Uri Avatar { get; set; }

        [JsonProperty("avatarmedium")]
        public Uri Avatarmedium { get; set; }

        [JsonProperty("avatarfull")]
        public Uri Avatarfull { get; set; }

        [JsonProperty("avatarhash")]
        public string Avatarhash { get; set; }

        [JsonProperty("lastlogoff")]
        public long Lastlogoff { get; set; }

        [JsonProperty("personastate")]
        public long Personastate { get; set; }

        [JsonProperty("personastateflags")]
        public long Personastateflags { get; set; }
    }
}