using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField]
    private LevelMetaData _levelMetaData;
    [SerializeField]
    private Canvas _canvas;

    public override void InstallBindings()
    {
        //делаем так, потому что нужно чтобы пикер был внутри канваса
        GameObject canvas = Container.InstantiatePrefab(_canvas);
        Container.Bind<SuperJumpPicker>().FromInstance(canvas.GetComponentInChildren<SuperJumpPicker>()).AsSingle().NonLazy();
        Container.Bind<CollectableCountHandler>().FromInstance(canvas.GetComponentInChildren<CollectableCountHandler>()).AsSingle().NonLazy();
        
        Container.Bind<LevelMetaData>().FromScriptableObject(_levelMetaData).AsSingle().NonLazy();
    }
}