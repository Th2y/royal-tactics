public static class GameServices
{
    public static ProgressService Progress { get; private set; }

    public static void Initialize()
    {
        Progress = new ProgressService(new PlayerPrefsProgressStorage());
    }
}