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

    protected virtual bool DontDestroy => false;

    private bool wasInitialized = false;

    public void InitAwake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (wasInitialized) return;

        Instance = this as T;
        wasInitialized = true;

        if (DontDestroy) 
            DontDestroyOnLoad(gameObject);

        OnInitAwake();
    }

    public void InitStart()
    {
        OnInitStart();
    }

    public abstract void OnInitAwake();
    public abstract void OnInitStart();
}
