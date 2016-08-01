﻿using Newtonsoft.Json;

namespace Discord.API.Rpc
{
    public class RpcChannel : Channel
    {
        [JsonProperty("voice_states")]
        public VoiceState[] VoiceStates { get; set; }
    }
}
