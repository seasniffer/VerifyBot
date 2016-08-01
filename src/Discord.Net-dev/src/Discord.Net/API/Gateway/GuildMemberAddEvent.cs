﻿using Newtonsoft.Json;

namespace Discord.API.Gateway
{
    public class GuildMemberAddEvent : GuildMember
    {
        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }
    }
}
