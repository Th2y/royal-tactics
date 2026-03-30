using System.Collections;
using UnityEngine.Networking;

public class FirestoreService
{
    private FirebaseSO config;
    private AuthService auth;

    public FirestoreService(FirebaseSO config, AuthService auth)
    {
        this.config = config;
        this.auth = auth;
    }

    public IEnumerator SetDocument(string collection, string documentId, string json)
    {
        string url =
            $"https://firestore.googleapis.com/v1/projects/{config.projectId}/databases/(default)/documents/{collection}/{documentId}";

        var request = new UnityWebRequest(url, "PATCH");

        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + auth.IdToken);

        yield return request.SendWebRequest();
    }

    public IEnumerator GetDocument(string collection, string documentId, System.Action<string> onResult)
    {
        string url =
            $"https://firestore.googleapis.com/v1/projects/{config.projectId}/databases/(default)/documents/{collection}/{documentId}";

        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + auth.IdToken);

        yield return request.SendWebRequest();

        onResult?.Invoke(request.downloadHandler.text);
    }
}