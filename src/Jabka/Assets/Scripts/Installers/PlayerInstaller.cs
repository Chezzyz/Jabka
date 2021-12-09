using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private PlayerData _playerPrefab;
    [SerializeField]
    private LevelMetaData _levelMetaData;
    [SerializeField]
    private SuperJumpUnlocker _superJumpUnlocker;
    [SerializeField]
    private PlayerCamera _playerCamera;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private Trajectory _trajectory;

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromComponentInNewPrefab(_inputHandler).AsSingle().NonLazy();
        Container.Bind<PlayerCamera>().FromComponentInNewPrefab(_playerCamera).AsSingle().NonLazy();
        Container.Bind<SuperJumpUnlocker>().FromComponentInNewPrefab(_superJumpUnlocker).AsSingle().NonLazy();
        Container.Bind<Trajectory>().FromComponentInNewPrefab(_trajectory).AsSingle().NonLazy();
        Container.Bind<LevelMetaData>().FromScriptableObject(_levelMetaData).AsSingle().NonLazy();

        //делаем так, потому что нужно чтобы пикер был внутри канваса
        GameObject canvas = Container.InstantiatePrefab(_canvas);
        Container.Bind<SuperJumpPicker>().FromInstance(canvas.GetComponentInChildren<SuperJumpPicker>()).AsSingle().NonLazy();

        Container.Bind(typeof(PlayerTransformController), typeof(SimpleJump), typeof(PlayerRotation))
            .FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
    }
}
