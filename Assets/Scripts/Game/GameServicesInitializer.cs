public class GameServicesInitializer : UnityMethodsSingleton<GameServicesInitializer>
{
    public override InitPriority Priority => InitPriority.Services;

    public override void OnInitAwake()
    {
        GameServices.Initialize();
    }

    public override void OnInitStart()
    {
        
    }
}