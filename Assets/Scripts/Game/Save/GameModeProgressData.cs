using System.Collections.Generic;

[System.Serializable]
public class GameModeProgressData
{
    public int gameModeId;
    public List<PhaseProgressData> phases = new();
}