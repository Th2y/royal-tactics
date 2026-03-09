using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackTheKingModeUI : GameModeUIBase
{
    public override void BuildUI()
    {
        throw new System.NotImplementedException();
    }

    public override void RefreshButtons()
    {
        throw new System.NotImplementedException();
    }

    public override void SetOptions<T>(List<T> optionsT)
    {
        var options = optionsT.Cast<KingStateDefinitionSO>().ToList();
        throw new System.NotImplementedException();
    }
}
