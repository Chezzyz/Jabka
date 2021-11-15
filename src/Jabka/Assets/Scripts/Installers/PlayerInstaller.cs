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

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromComponentsInNewPrefab(_inputHandler).AsSingle().NonLazy();
        Container.Bind<PlayerCamera>().FromComponentsInNewPrefab(_playerCamera).AsSingle().NonLazy();
        Container.Bind<PlayerTransformController>().FromComponentsInNewPrefab(_playerPrefab).AsSingle().NonLazy();

    }
}
