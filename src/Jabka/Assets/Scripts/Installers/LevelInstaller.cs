using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField]
    private LevelMetaData _levelMetaData;
    
    public override void InstallBindings()
    {
        Container.Bind<LevelMetaData>().FromScriptableObject(_levelMetaData).AsSingle().NonLazy();
    }
}