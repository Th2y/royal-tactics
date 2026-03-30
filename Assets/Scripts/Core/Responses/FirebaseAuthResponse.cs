using Newtonsoft.Json;

public class FirebaseAuthResponse
{
    [JsonProperty("idToken")]
    public string IdToken;

    [JsonProperty("localId")]
    public string LocalId;
}