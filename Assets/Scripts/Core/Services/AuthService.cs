using Newtonsoft.Json;
using System.Collections;
using UnityEngine.Networking;

public class AuthService
{
    private FirebaseSO config;

    public string IdToken { get; private set; }
    public string UserId { get; private set; }

    public AuthService(FirebaseSO config)
    {
        this.config = config;
    }

    public IEnumerator SignInAnonymously(System.Action onSuccess)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={config.apiKey}";

        var request = new UnityWebRequest(url, "POST");
        byte[] body = System.Text.Encoding.UTF8.GetBytes("{\"returnSecureToken\": true}");

        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        var json = request.downloadHandler.text;
        var response = JsonConvert.DeserializeObject<FirebaseAuthResponse>(json);

        IdToken = response.IdToken;
        UserId = response.LocalId;

        onSuccess?.Invoke();
    }
}