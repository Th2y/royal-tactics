using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Royal Tactics/King State Definition")]
public class KingStateDefinitionSO : ScriptableObject
{
    public KingState type;
    public LocalizedString nameLocale;
}
