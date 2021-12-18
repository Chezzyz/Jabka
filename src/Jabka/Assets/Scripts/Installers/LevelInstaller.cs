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
        //������ ���, ������ ��� ����� ����� ����� ��� ������ �������
        GameObject canvas = Container.InstantiatePrefab(_canvas);
        Container.Bind<SuperJumpPicker>().FromInstance(canvas.GetComponentInChildren<SuperJumpPicker>()).AsSingle().NonLazy();
        Container.Bind<CollectableCountHandler>().FromInstance(canvas.GetComponentInChildren<CollectableCountHandler>()).AsSingle().NonLazy();
        
        Container.Bind<LevelMetaData>().FromScriptableObject(_levelMetaData).AsSingle().NonLazy();
    }
}