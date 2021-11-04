using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ControlsInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject _inputHandler;
    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromComponentsInNewPrefab(_inputHandler).AsSingle().NonLazy();
    }

}
