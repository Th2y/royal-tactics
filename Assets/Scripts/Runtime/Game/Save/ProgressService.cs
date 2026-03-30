public class ProgressService
{
    private readonly IProgressStorage storage;
    private PlayerProgressData data;

    public ProgressService(IProgressStorage storage)
    {
        this.storage = storage;
        data = storage.Load();
    }

    public void CompletePhase(int gameModeId, int phaseId)
    {
        var mode = data.gameModes.Find(m => m.gameModeId == gameModeId);

        if (mode == null)
        {
            mode = new GameModeProgressData
            {
                gameModeId = gameModeId
            };
            data.gameModes.Add(mode);
        }

        if (!mode.phases.Exists(p => p.phaseId == phaseId))
        {
            mode.phases.Add(new PhaseProgressData
            {
                phaseId = phaseId,
                completed = true
            });
        }

        storage.Save(data);
    }

    public bool IsPhaseCompleted(int gameModeId, int phaseId)
    {
        var mode = data.gameModes.Find(m => m.gameModeId == gameModeId);
        return mode?.phases.Exists(p => p.phaseId == phaseId && p.completed) ?? false;
    }
}