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
    private PlayerCamera _playerCamera;

    [SerializeField]
    private Canvas _canvas;
    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromComponentInNewPrefab(_inputHandler).AsSingle().NonLazy();
        Container.Bind<PlayerCamera>().FromComponentInNewPrefab(_playerCamera).AsSingle().NonLazy();

        GameObject canvas = Container.InstantiatePrefab(_canvas);
        Container.Bind<SuperJumpPicker>().FromInstance(canvas.GetComponentInChildren<SuperJumpPicker>()).AsSingle().NonLazy();

        Container.Bind(typeof(PlayerTransformController), typeof(SimpleJump))
            .FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
    }
}
