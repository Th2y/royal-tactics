using UnityEngine;

public abstract class UnityMethods : MonoBehaviour
{
    public abstract InitPriority Priority { get; }

    public abstract void OnInitAwake();
    public abstract void OnInitStart();
}
