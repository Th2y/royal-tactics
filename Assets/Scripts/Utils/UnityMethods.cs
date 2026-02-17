using UnityEngine;

public abstract class UnityMethods : MonoBehaviour
{
    public abstract InitPriority priority { get; }

    public abstract void InitAwake();
    public abstract void InitStart();
}
