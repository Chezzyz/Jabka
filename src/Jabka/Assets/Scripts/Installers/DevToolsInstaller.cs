using UnityEngine;
using Zenject;

public class DevToolsInstaller : MonoInstaller
{
    [SerializeField]
    GameObject _devToolsPrefab;
    public override void InstallBindings()
    {
        Container.Bind<TimeScaler>().FromComponentInNewPrefab(_devToolsPrefab).AsSingle().NonLazy();
    }
}