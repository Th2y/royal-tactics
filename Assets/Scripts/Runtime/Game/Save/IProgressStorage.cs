public interface IProgressStorage
{
    PlayerProgressData Load();
    void Save(PlayerProgressData data);
}