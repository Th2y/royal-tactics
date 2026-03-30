using System.Linq;
using UnityEngine;

public class InitAllInOrder : MonoBehaviour
{
    private UnityMethods[] systems;
    private IUnityMethods[] systemsS;

    private void Awake()
    {
        systemsS = FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None)
            .OfType<IUnityMethods>()
            .OrderBy(s => s.Priority)
            .ToArray();

        foreach (var system in systemsS)
        {
            system?.InitAwake();
        }

        systems = FindObjectsByType<UnityMethods>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OrderBy(s => s.Priority)
            .ToArray();

        foreach (var system in systems)
        {
            if (system != null) system.OnInitAwake();
        }
    }

    private void Start()
    {
        foreach (var system in systemsS)
        {
            system?.InitStart();
        }

        foreach (var system in systems)
        {
            if (system != null) system.OnInitStart();
        }
    }
}
