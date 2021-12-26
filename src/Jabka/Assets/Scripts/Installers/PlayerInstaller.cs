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
    private Trajectory _trajectory;

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromComponentInNewPrefab(_inputHandler).AsSingle().NonLazy();
        Container.Bind<PlayerCamera>().FromComponentInNewPrefab(_playerCamera).AsSingle().NonLazy();
        Container.Bind<Trajectory>().FromComponentInNewPrefab(_trajectory).AsSingle().NonLazy();

        Container.Bind(typeof(PlayerTransformController), typeof(SimpleJump), typeof(PlayerRotation), typeof(JumpController))
            .FromComponentInNewPrefab(_playerPrefab).AsSingle().NonLazy();
    }
}
