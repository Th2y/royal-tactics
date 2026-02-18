using UnityEngine;

public interface IUnityMethods
{
    InitPriority Priority { get; }

    void InitAwake();
    void InitStart();
}

public abstract class UnityMethodsSingleton<T> : MonoBehaviour, IUnityMethods
    where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public abstract InitPriority Priority { get; }

    public void InitAwake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        OnInitAwake();
    }

    public void InitStart()
    {
        OnInitStart();
    }

    public abstract void OnInitAwake();
    public abstract void OnInitStart();
}
