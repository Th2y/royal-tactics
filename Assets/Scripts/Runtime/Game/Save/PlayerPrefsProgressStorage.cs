using UnityEngine;

public class PlayerPrefsProgressStorage : IProgressStorage
{
    private const string KEY = "PLAYER_PROGRESS";

    public PlayerProgressData Load()
    {
        if (!PlayerPrefs.HasKey(KEY))
            return new PlayerProgressData();

        string json = PlayerPrefs.GetString(KEY);
        return JsonUtility.FromJson<PlayerProgressData>(json);
    }

    public void Save(PlayerProgressData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }
}