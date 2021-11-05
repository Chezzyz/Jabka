using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField]
    private InputHandler _inputHandler;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private Transform _spawnPoint;

    [SerializeField]
    private PlayerCamera _playerCamera;

    public override void InstallBindings()
    {
        GameObject player = Container.InstantiatePrefab(_player, _spawnPoint.position, Quaternion.identity, null);
        Container.Bind<PlayerTransformController>().FromNewComponentOn(player).AsSingle();
        Container.Bind<PlayerRotation>().FromNewComponentOn(player).AsSingle();
        Container.Bind<InputHandler>().FromComponentsInNewPrefab(_inputHandler).AsSingle().NonLazy();
        Container.Bind<PlayerCamera>().FromComponentsInNewPrefab(_playerCamera).AsSingle().NonLazy();
    }
}
